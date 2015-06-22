using CommonRessources;
using Core.Network;
using DataStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AppLogic.ServerLogic
{
    public class ServerLogicCore : IServerLogic, ILogic
    {
        private INetworkServer serverReference;
        private IStore store;

        private static readonly Object syncRoot = new Object();
        private static bool isInstantiated = false;

        public ServerLogicCore(INetworkServer server)
        {
            lock (ServerLogicCore.syncRoot)
            {
                if (ServerLogicCore.isInstantiated)
                {
                    throw new InvalidOperationException("No two instances of ServerLogicCore should be active at the same time!");
                }
                else
                {
                    ServerLogicCore.isInstantiated = true;
                    this.serverReference = server;
                    this.serverReference.OnRequestEvent += ServerReference_RequestEvent;
                    this.store = new ComponentStore();
                }
            }
        }

        ~ServerLogicCore()
        {
            lock (syncRoot)
            {
                isInstantiated = false;
            }
        }

        private void ServerReference_RequestEvent(object sender, ComponentRecievedEventArgs e)
        {

        }

        private List<ComponentNode> SimplifyGraph(IEnumerable<ComponentEdge> edgeList)
        {
            var internalEdges = edgeList.Where(edge => edge.InternalInputComponentGuid != Guid.Empty && edge.InternalOutputComponentGuid != Guid.Empty);
            var outerEdges = edgeList.Where(edge => edge.InternalInputComponentGuid == Guid.Empty || edge.InternalOutputComponentGuid == Guid.Empty);
            var result = new List<ComponentNode>();

            foreach (var edge in internalEdges)
            {
                var inputComponentTuple = this.store[edge.InputComponentGuid];
                var outputComponentTuple = this.store[edge.OutputComponentGuid];

                ExtractNode(result, edge, edge.InternalInputComponentGuid, inputComponentTuple);
                ExtractNode(result, edge, edge.InternalOutputComponentGuid, outputComponentTuple);
            }

            foreach (var edge in internalEdges)
            {
                var sourceNode = result.Where(node => node.InternalId == edge.InternalOutputComponentGuid).First();
                var targetNode = result.Where(node => node.InternalId == edge.InternalInputComponentGuid).First();

                sourceNode.Outputs[edge.InputValueID] = targetNode;
                targetNode.Inputs[edge.OutputValueID] = sourceNode;
            }

            List<ComponentNode> complexNodes = result.Where(n => n.InternalEdges != null).ToList();
            List<ComponentNode> simplifiedNodes = new List<ComponentNode>();

            foreach (var node in complexNodes)
            {
                var subGraph = this.SimplifyGraph(node.InternalEdges);
                var subGraphEdges = node.InternalEdges;

                foreach (var subNode in subGraph.Where(subNode => subNode.Inputs.ContainsValue(null)))
                {
                    var ingoingEdges = subGraphEdges.Where(
                        edge =>
                            edge.InternalOutputComponentGuid == subNode.InternalId
                            && (edge.InternalInputComponentGuid == Guid.Empty || edge.InternalInputComponentGuid == node.InternalId));

                    foreach (var inEdge in ingoingEdges)
                    {
                        uint port = inEdge.InputValueID;
                        var outerEdge = edgeList.Single(edge => edge.InternalOutputComponentGuid == node.InternalId && edge.OutputValueID == port);
                        var extNode = result.Single(n => n.InternalId == outerEdge.InternalInputComponentGuid);
                        subNode.Inputs[inEdge.OutputValueID] = extNode;
                        extNode.Outputs[port] = subNode;
                    }
                }

                foreach (var subNode in subGraph.Where(subNode => subNode.Outputs.ContainsValue(null)))
                {
                    var outgoingEdges = subGraphEdges.Where(
                        edge =>
                            edge.InternalInputComponentGuid == subNode.InternalId
                            && (edge.InternalOutputComponentGuid == Guid.Empty || edge.InternalOutputComponentGuid == node.InternalId));

                    foreach (var outEdge in outgoingEdges)
                    {
                        uint port = outEdge.OutputValueID;
                        var outerEdge = edgeList.Single(edge => edge.InternalInputComponentGuid == node.InternalId && edge.InputValueID == port);
                        var extNode = result.Single(n => n.InternalId == outerEdge.InternalOutputComponentGuid);
                        subNode.Outputs[outEdge.InputValueID] = extNode;
                        extNode.Inputs[port] = subNode;
                    }
                }
            }

            complexNodes.ForEach(n => result.Remove(n));

            return result;
        }

        private static void ExtractNode(List<ComponentNode> result, ComponentEdge edge, Guid internalComponentGuid, Tuple<byte[], bool> componentTuple)
        {
            if (componentTuple.Item2 == true)
            {
                var bytes = componentTuple.Item1;
                var ass = Assembly.Load(bytes);
                var loadedType = ass.GetTypes().Where(type => type.IsSubclassOf(typeof(Core.Component.IComponent))).First();
                var instance = Activator.CreateInstance(loadedType) as IComponent;

                var node = new ComponentNode()
                {
                    ComponentId = instance.ComponentGuid,
                    FriendlyName = instance.FriendlyName,
                    InputTypes = instance.InputHints.ToList(),
                    OutputTypes = instance.OutputHints.ToList(),
                    InternalEdges = null,
                    InternalId = internalComponentGuid,
                };

                if (!result.Any(x => x.InternalId == node.InternalId))
                {
                    result.Add(node); 
                }
            }
            else
            {
                var bytes = componentTuple.Item1;
                var byteStream = new MemoryStream(bytes);
                var bf = new BinaryFormatter();
                var component = bf.Deserialize(byteStream) as Core.Network.Component;

                var node = new ComponentNode()
                {
                    ComponentId = component.ComponentGuid,
                    FriendlyName = component.FriendlyName,
                    InputTypes = component.InputHints.ToList(),
                    OutputTypes = component.OutputHints.ToList(),
                    InternalEdges = component.Edges,
                    InternalId = internalComponentGuid,
                };



                if (!result.Any(x => x.InternalId == node.InternalId))
                {
                    result.Add(node);
                }
            }
        }
    }
}
                        //var ass = Assembly.Load((byte[])inputComponent.Item1);
                        //var type = ass.GetTypes().First(t => t.IsSubclassOf(typeof(Core.Component.IComponent)));
                        //Activator.CreateInstance(type);
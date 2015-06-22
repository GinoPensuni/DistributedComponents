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

        private void ServerReference_RequestEvent(object sender, ComponentRecievedEventArgs e)
        {

        }

        private List<ComponentNode> SimplifyGraph(List<ComponentEdge> edgeList)
        {
            var internalEdges = edgeList.Where(edge => edge.InternalInputComponentGuid != Guid.Empty && edge.InternalOutputComponentGuid != Guid.Empty);
            var inputEdges = edgeList.Where(edge => edge.InternalInputComponentGuid == Guid.Empty);
            var outputEdges = edgeList.Where(edge => edge.InternalOutputComponentGuid == Guid.Empty);
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
                    InputTypes = instance.InputHints,
                    OutputTypes = instance.OutputHints,
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
                    InputTypes = component.InputHints,
                    OutputTypes = component.OutputHints,
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
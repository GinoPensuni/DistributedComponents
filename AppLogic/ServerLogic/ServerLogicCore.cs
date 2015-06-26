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
using System.Collections.Concurrent;
using System.Threading;
using Server;

namespace AppLogic.ServerLogic
{
    public class ServerLogicCore : IServerLogic, ILogic
    {
        private readonly Server.Server master;
        private readonly ExternalServersManager serverManager;
        private readonly ComponentStore store;

        private static readonly Object syncRoot = new Object();
        private static bool isInstantiated = false;

        public ServerLogicCore(Server.Server master, ExternalServersManager manager, ComponentStore store)
        {
            lock (ServerLogicCore.syncRoot)
            {
                if (ServerLogicCore.isInstantiated)
                {
                    throw new InvalidOperationException("No two instances of ServerLogicCore should be active at the same time!");
                }
                else
                {
                    this.master = master;
                    this.master.OnJobRequestReceived += ServerReference_RequestEvent;
                    this.master.OnUploadRequestReceived += ServerReference_OnBombRevieced;
                    this.master.OnAllAvailableComponentsRequestReceived += OnAllAvailableComponentsRequestReceived;
                    this.master.Run();

                    //this.serverManager = manager;
                    //this.serverManager.OnExternalServerLoggedOn += ServerManager_OnExternalServerLoggedOn;
                    //this.serverManager.OnExternalServerTerminated += ServerManager_OnExternalServerTerminated;
                    //this.serverManager.StartListening();

                    this.store = store;

                    isInstantiated = true;
                }
            }
        }

        private void OnAllAvailableComponentsRequestReceived(object sender, RequestForAllComponentsReceivedEventArgs e)
        {
            //e.AllAvailableComponents = this.store.LoadAssemblies().Select(ass => new Tuple<ComponentType, byte[]>(ComponentType.Simple, ass.Assembly)).ToList();

            var simpleComponentTuples = this.store.LoadAssemblies().Where(ass => ass.IsAtomic).Select(ass => new Tuple<ComponentType, byte[]>(ComponentType.Simple, ass.Assembly));
            var complexComponentTuples = this.store.LoadAssemblies().Where(ass => !ass.IsAtomic).Select(ass => new Tuple<ComponentType, byte[]>(ComponentType.Complex, ass.Assembly));
            e.AllAvailableComponents = new List<Tuple<ComponentType, byte[]>>();
            e.AllAvailableComponents.AddRange(simpleComponentTuples);
            e.AllAvailableComponents.AddRange(complexComponentTuples);
        }

        private void ServerManager_OnExternalServerTerminated(ExternalServer extServer)
        {
            extServer.OnAssemblyRequestReceived -= OnAssemblyRequestReceived;
            extServer.OnAssemblyResponseReceived -= OnAssemblyResponseReceived;
            extServer.OnExternalClientUpdated -= OnExternalClientUpdated;
            extServer.OnExternalComponentSubmitRequestReceived -= OnExternalComponentSubmitRequestReceived;
            extServer.OnInternalClientUpdatedResponseReceived -= OnInternalClientUpdatedResponseReceived;
            extServer.OnInternalComponentSubmitResponseReceived -= OnInternalComponentSubmitResponseReceived;
            extServer.OnJobRequestReceived -= OnJobRequestReceived;
            extServer.OnJobResponseReceived -= OnJobResponseReceived;
            extServer.OnJobResultRequestReceived -= OnJobResultRequestReceived;
            extServer.OnJobResultResponseReceived -= OnJobResultResponseReceived;
            extServer.OnLogonCompleted -= OnLogonCompleted;
            extServer.OnTerminated -= OnTerminated;
        }

        private void ServerManager_OnExternalServerLoggedOn(ExternalServer extServer)
        {
            extServer.OnAssemblyRequestReceived += OnAssemblyRequestReceived;
            extServer.OnAssemblyResponseReceived += OnAssemblyResponseReceived;
            extServer.OnExternalClientUpdated += OnExternalClientUpdated;
            extServer.OnExternalComponentSubmitRequestReceived += OnExternalComponentSubmitRequestReceived;
            extServer.OnInternalClientUpdatedResponseReceived += OnInternalClientUpdatedResponseReceived;
            extServer.OnInternalComponentSubmitResponseReceived += OnInternalComponentSubmitResponseReceived;
            extServer.OnJobRequestReceived += OnJobRequestReceived;
            extServer.OnJobResponseReceived += OnJobResponseReceived;
            extServer.OnJobResultRequestReceived += OnJobResultRequestReceived;
            extServer.OnJobResultResponseReceived += OnJobResultResponseReceived;
            extServer.OnLogonCompleted += OnLogonCompleted;
            extServer.OnTerminated += OnTerminated;
        }

        private void OnTerminated(object sender, ExternalServerDiedEventArgs e)
        {
            var extServer = e.ExternalServer;
            extServer.OnAssemblyRequestReceived -= OnAssemblyRequestReceived;
            extServer.OnAssemblyResponseReceived -= OnAssemblyResponseReceived;
            extServer.OnExternalClientUpdated -= OnExternalClientUpdated;
            extServer.OnExternalComponentSubmitRequestReceived -= OnExternalComponentSubmitRequestReceived;
            extServer.OnInternalClientUpdatedResponseReceived -= OnInternalClientUpdatedResponseReceived;
            extServer.OnInternalComponentSubmitResponseReceived -= OnInternalComponentSubmitResponseReceived;
            extServer.OnJobRequestReceived -= OnJobRequestReceived;
            extServer.OnJobResponseReceived -= OnJobResponseReceived;
            extServer.OnJobResultRequestReceived -= OnJobResultRequestReceived;
            extServer.OnJobResultResponseReceived -= OnJobResultResponseReceived;
            extServer.OnLogonCompleted -= OnLogonCompleted;
            extServer.OnTerminated -= OnTerminated;
        }

        private void OnLogonCompleted(object sender, ExternalServerLogonCompletedEventArgs e)
        {

        }

        private void OnJobResultResponseReceived(object sender, ExternalJobResultResponseEventArgs e)
        {
            
        }

        private void OnJobResultRequestReceived(object sender, ExternalJobResultRequestEventArgs e)
        {

        }

        private void OnJobResponseReceived(object sender, ExternalJobResponseEventArgs e)
        {

        }

        private void OnJobRequestReceived(object sender, ExternalJobRequestEventArgs e)
        {
            var jobGuid = e.JobGuid;
            var component = e.JobComponent;
            var input = e.InputData;
            var hopCount = e.HopCount + 1;
            var server = sender as ExternalServer;

            if (hopCount > this.serverManager.ExternalServers.Count)
            {
                e.Processed = true;
                e.IsAccepted = false;
            }
            else if (server == null)
            {
                e.Processed = true;
                e.IsAccepted = false;
            }
            else if (!this.serverManager.ExternalServers.ContainsValue(server))
            {
                e.Processed = true;
                e.IsAccepted = false;
            }
            else
            {
                GraphExecutionSet data = new GraphExecutionSet()
                    {
                        Component = component,
                        Input = input.ToList(),
                        JobGuid = jobGuid,
                        Server = server,
                    };

                Thread t = new Thread(new ParameterizedThreadStart(ExecuteGraph));
                t.Start(data);
                e.Processed = true;
                e.IsAccepted = true;
            }
        }

        private void OnInternalComponentSubmitResponseReceived(object sender, InternalComponentSubmitResponseEventArgs e)
        {

        }

        private void OnInternalClientUpdatedResponseReceived(object sender, InternalClientUpdateResponseEventArgs e)
        {
            
        }

        private void OnExternalComponentSubmitRequestReceived(object sender, ExternalComponentSubmittedEventArgs e)
        {
            
        }

        private void OnExternalClientUpdated(object sender, ExternalClientUpdateRequestEventArgs e)
        {
            
        }

        private void OnAssemblyResponseReceived(object sender, ExternalServerAssemblyRequestedEventArgs e)
        {
            
        }

        private void OnAssemblyRequestReceived(object sender, ExternalServerAssemblyRequestedEventArgs e)
        {
            var componentGuid = e.ComponentGuid;

            try
            {
                var componentTuple = this.store[componentGuid];
                var binaryData = componentTuple.Item1;
                var isAtomic = componentTuple.Item2;

                if (isAtomic)
                {
                    e.Processed = false;
                }
                else
                {
                    e.BinaryContent = binaryData;
                    e.Processed = true;
                }
            }
            catch (Exception)
            {
                e.Processed = false;
            }
        }

        ~ServerLogicCore()
        {
            lock (syncRoot)
            {
                isInstantiated = false;
            }
        }

        private void ServerReference_OnBombRevieced(object sender, SaveComponentEventArgs e)
        {
            var component = e.Component;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream outputStream = new MemoryStream();
            try
            {
                bf.Serialize(outputStream, component);
                this.store.Store(component.ComponentGuid, component.FriendlyName, false, outputStream.ToArray());
            }
            catch (Exception)
            {

            }
        }

        private void ServerReference_RequestEvent(object sender, ComponentRecievedEventArgs e)
        {
            Thread t = new Thread(new ParameterizedThreadStart(ExecuteGraph));

            t.Start(new GraphExecutionSet()
            {
                Component = e.Component,
                Input = e.Input,
                JobGuid = e.JobRequestGuid,
                Server = this.master,
            });
        }

        private void ExecuteGraph(object incomingEventArgs)
        {   
            GraphExecutionSet data = incomingEventArgs as GraphExecutionSet;
            var component = data.Component;
            var guid = data.JobGuid;
            var args = data.Input;
            var edges = data.Component.Edges;
            var server = data.Server;
            Dictionary<Guid, ComponentWorker> workerMap = new Dictionary<Guid, ComponentWorker>();

            try
            {
                

                ComponentGraphTools.ExtractNodes(edges, workerMap, new BinaryFormatter(), this.master, this.store);

                ComponentGraphTools.ExtractInnerEdges(edges, workerMap);

                Dictionary<uint, DataGate> inputDataGates = new Dictionary<uint, DataGate>();
                Dictionary<uint, DataGate> outputDataGates = new Dictionary<uint, DataGate>();

                foreach (var inPort in edges.Select(e => new Tuple<Guid, uint>(e.InternalOutputComponentGuid, e.OutputValueID)).Where(t => t.Item1 == Guid.Empty).Select(t => t.Item2))
                {
                    inputDataGates[inPort] = new DataGate();
                }

                foreach (var outPort in edges.Select(e => new Tuple<Guid, uint>(e.InternalInputComponentGuid, e.InputValueID)).Where(t => t.Item1 == Guid.Empty).Select(t => t.Item2))
                {
                    outputDataGates[outPort] = new DataGate();
                }

                ComponentGraphTools.ExtractOuterEdges(edges, workerMap, inputDataGates, outputDataGates);

                var inputList = data.Input.ToList();

                for (int i = 0; i < inputList.Count; i++)
                {
                    inputDataGates[(uint)i].SendData(inputList[i]);
                }

                workerMap.ToList().ForEach(threadEntry => threadEntry.Value.Start());

                var outputList = new List<object>();

                foreach (var endGate in outputDataGates.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value))
                {
                    var finalResult = endGate.ReceiveData();
                    outputList.Add(data);
                }

                if (server == this.master)
                {
                    this.master.SendFinalResult(guid, outputList);
                }
                else
                {
                    var extServer = server as ExternalServer;
                    extServer.SendJobResultRequest(new JobResultRequest()
                    {
                        JobGuid = guid,
                        JobResultGuid = Guid.NewGuid(),
                        OutputData = outputList,
                        State = JobState.ComponentCompleted,
                    });
                }
            }
            catch (Exception ex)
            {
                if (server == this.master)
                {
                    this.master.SendFinalResult(guid, new List<object>() { ex });
                }
                else
                {
                    var extServer = server as ExternalServer;
                    extServer.SendJobResultRequest(new JobResultRequest()
                    {
                        JobGuid = guid,
                        JobResultGuid = Guid.NewGuid(),
                        OutputData = new List<object>() { ex },
                        State = JobState.Exception,
                    });
                }
            }
        }

        //private List<ComponentNode> SimplifyGraph(IEnumerable<ComponentEdge> edgeList)
        //{
        //    var internalEdges = edgeList.Where(edge => edge.InternalInputComponentGuid != Guid.Empty && edge.InternalOutputComponentGuid != Guid.Empty);
        //    var outerEdges = edgeList.Where(edge => edge.InternalInputComponentGuid == Guid.Empty || edge.InternalOutputComponentGuid == Guid.Empty);
        //    var result = new List<ComponentNode>();

        //    foreach (var edge in internalEdges)
        //    {
        //        var inputComponentTuple = this.store[edge.InputComponentGuid];
        //        var outputComponentTuple = this.store[edge.OutputComponentGuid];

        //        ExtractNode(result, edge, edge.InternalInputComponentGuid, inputComponentTuple);
        //        ExtractNode(result, edge, edge.InternalOutputComponentGuid, outputComponentTuple);
        //    }

        //    foreach (var edge in internalEdges)
        //    {
        //        var sourceNode = result.Where(node => node.InternalId == edge.InternalOutputComponentGuid).First();
        //        var targetNode = result.Where(node => node.InternalId == edge.InternalInputComponentGuid).First();

        //        sourceNode.Outputs[edge.InputValueID] = targetNode;
        //        targetNode.Inputs[edge.OutputValueID] = sourceNode;
        //    }

        //    List<ComponentNode> complexNodes = result.Where(n => n.InternalEdges != null).ToList();
        //    List<ComponentNode> simplifiedNodes = new List<ComponentNode>();

        //    foreach (var node in complexNodes)
        //    {
        //        var subGraph = this.SimplifyGraph(node.InternalEdges);
        //        var subGraphEdges = node.InternalEdges;

        //        foreach (var subNode in subGraph.Where(subNode => subNode.Inputs.ContainsValue(null)))
        //        {
        //            var ingoingEdges = subGraphEdges.Where(
        //                edge =>
        //                    edge.InternalOutputComponentGuid == subNode.InternalId
        //                    && (edge.InternalInputComponentGuid == Guid.Empty || edge.InternalInputComponentGuid == node.InternalId));

        //            foreach (var inEdge in ingoingEdges)
        //            {
        //                uint port = inEdge.InputValueID;
        //                var outerEdge = edgeList.Single(edge => edge.InternalOutputComponentGuid == node.InternalId && edge.OutputValueID == port);
        //                var extNode = result.Single(n => n.InternalId == outerEdge.InternalInputComponentGuid);
        //                subNode.Inputs[inEdge.OutputValueID] = extNode;
        //                extNode.Outputs[port] = subNode;
        //            }
        //        }

        //        foreach (var subNode in subGraph.Where(subNode => subNode.Outputs.ContainsValue(null)))
        //        {
        //            var outgoingEdges = subGraphEdges.Where(
        //                edge =>
        //                    edge.InternalInputComponentGuid == subNode.InternalId
        //                    && (edge.InternalOutputComponentGuid == Guid.Empty || edge.InternalOutputComponentGuid == node.InternalId));

        //            foreach (var outEdge in outgoingEdges)
        //            {
        //                uint port = outEdge.OutputValueID;
        //                var outerEdge = edgeList.Single(edge => edge.InternalInputComponentGuid == node.InternalId && edge.InputValueID == port);
        //                var extNode = result.Single(n => n.InternalId == outerEdge.InternalOutputComponentGuid);
        //                subNode.Outputs[outEdge.InputValueID] = extNode;
        //                extNode.Inputs[port] = subNode;
        //            }
        //        }
        //    }

        //    complexNodes.ForEach(n => result.Remove(n));

        //    return result;
        //}

        //    private static void ExtractNode(List<ComponentNode> result, ComponentEdge edge, Guid internalComponentGuid, Tuple<byte[], bool> componentTuple)
        //    {
        //        if (componentTuple.Item2 == true)
        //        {
        //            var bytes = componentTuple.Item1;
        //            var ass = Assembly.Load(bytes);
        //            var loadedType = ass.GetTypes().Where(type => type.IsSubclassOf(typeof(Core.Component.IComponent))).First();
        //            var instance = Activator.CreateInstance(loadedType) as IComponent;

        //            var node = new ComponentNode()
        //            {
        //                ComponentId = instance.ComponentGuid,
        //                FriendlyName = instance.FriendlyName,
        //                InputTypes = instance.InputHints.ToList(),
        //                OutputTypes = instance.OutputHints.ToList(),
        //                InternalEdges = null,
        //                InternalId = internalComponentGuid,
        //            };

        //            if (!result.Any(x => x.InternalId == node.InternalId))
        //            {
        //                result.Add(node); 
        //            }
        //        }
        //        else
        //        {
        //            var bytes = componentTuple.Item1;
        //            var byteStream = new MemoryStream(bytes);
        //            var bf = new BinaryFormatter();
        //            var component = bf.Deserialize(byteStream) as Core.Network.Component;

        //            var node = new ComponentNode()
        //            {
        //                ComponentId = component.ComponentGuid,
        //                FriendlyName = component.FriendlyName,
        //                InputTypes = component.InputHints.ToList(),
        //                OutputTypes = component.OutputHints.ToList(),
        //                InternalEdges = component.Edges,
        //                InternalId = internalComponentGuid,
        //            };



        //            if (!result.Any(x => x.InternalId == node.InternalId))
        //            {
        //                result.Add(node);
        //            }
        //        }
        //    }
        //}
    }
}
                        //var ass = Assembly.Load((byte[])inputComponent.Item1);
                        //var type = ass.GetTypes().First(t => t.IsSubclassOf(typeof(Core.Component.IComponent)));
                        //Activator.CreateInstance(type);
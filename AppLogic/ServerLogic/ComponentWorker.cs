using CommonRessources;
using Core.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppLogic.ServerLogic
{
    public class ComponentWorker
    {
        public readonly Dictionary<uint, DataGate> InputGates;
        public readonly Dictionary<uint, DataGate> OutputGates;

        private static object syncRoot = new object();

        private readonly Guid jobId;
        private readonly Guid componentGuid;
        private readonly byte[] assemblyBytes;
        private readonly IEnumerable<Core.Network.ComponentEdge> innerGraph;
        private readonly INetworkServer processingServer;
        private readonly Thread workerThread;
        private readonly IStore store;

        public ComponentWorker(INetworkServer processingServer, Guid componentId, byte[] assembly)
        {
            this.InputGates = new Dictionary<uint, DataGate>();
            this.OutputGates = new Dictionary<uint, DataGate>();
            this.jobId = Guid.NewGuid();
            this.componentGuid = componentId;
            this.assemblyBytes = assembly;
            this.innerGraph = null;
            this.processingServer = processingServer;
            this.workerThread = new Thread(new ThreadStart(this.ProcessIncomingData));
        }

        public ComponentWorker(INetworkServer processingServer, IStore componentStore, Guid componentId, IEnumerable<Core.Network.ComponentEdge> graph)
        {
            this.InputGates = new Dictionary<uint, DataGate>();
            this.OutputGates = new Dictionary<uint, DataGate>();
            this.jobId = Guid.NewGuid();
            this.componentGuid = componentId;
            this.assemblyBytes = null;
            this.innerGraph = graph;
            this.store = componentStore;
            this.processingServer = processingServer;
            this.workerThread = new Thread(new ThreadStart(this.ProcessInnerGraph));
        }

        public void Start()
        {
            if (this.innerGraph == null)
            {
                lock (syncRoot)
                {
                    this.processingServer.OnResultReceived += ProcessingServer_OnResultReceived;
                }

                this.workerThread.Start();
            }
            else
            {
                this.workerThread.Start();
            }
        }

        private void ProcessIncomingData()
        {
            IList<object> args = new List<object>();

            foreach (var datGate in this.InputGates.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value))
            {
                var arg = datGate.ReceiveData();
                args.Add(arg);
            }

            // Can be replaced by throw new ThreadingException();
            processingServer.SendCalculatedResult(this.jobId, new Tuple<Guid, IEnumerable<object>, byte[]>(this.componentGuid, args, this.assemblyBytes));

            lock (syncRoot)
            {
                processingServer.OnResultReceived -= this.ProcessingServer_OnResultReceived; 
            }
        }

        private void ProcessInnerGraph()
        {
            Dictionary<Guid, ComponentWorker> innerWorkerMap = new Dictionary<Guid, ComponentWorker>();
            var edges = this.innerGraph;

            ComponentGraphTools.ExtractNodes(edges, innerWorkerMap, new BinaryFormatter(), this.processingServer, this.store);

            ComponentGraphTools.ExtractInnerEdges(edges, innerWorkerMap);

            ComponentGraphTools.ExtractOuterEdges(edges, innerWorkerMap, this.InputGates, this.OutputGates);
        }

        private void ExtractInnerNodes(Dictionary<Guid, ComponentWorker> innerWorkerMap, IEnumerable<ComponentEdge> innerEdges)
        {
            foreach (var edge in innerEdges)
            {
                ComponentWorker destinationWorker;
                ComponentWorker sourceWorker;
                Dictionary<uint, DataGate> inputGates = new Dictionary<uint, DataGate>();
                BinaryFormatter bf = new BinaryFormatter();

                if (!innerWorkerMap.ContainsKey(edge.InternalInputComponentGuid))
                {
                    var componentEntry = this.store[edge.InputComponentGuid];
                    var componentBinary = componentEntry.Item1;
                    var isAtomic = componentEntry.Item2;

                    if (isAtomic)
                    {
                        destinationWorker = new ComponentWorker(this.processingServer, edge.InputComponentGuid, componentBinary);
                        innerWorkerMap[edge.InternalInputComponentGuid] = destinationWorker;
                    }
                    else
                    {
                        MemoryStream deserializationStream = new MemoryStream(componentBinary);
                        var componentGraph = (IEnumerable<ComponentEdge>)bf.Deserialize(deserializationStream);
                        destinationWorker = new ComponentWorker(this.processingServer, this.store, edge.InputComponentGuid, componentGraph);
                        innerWorkerMap[edge.InternalInputComponentGuid] = destinationWorker;
                    }
                }
                else
                {
                    destinationWorker = innerWorkerMap[edge.InternalInputComponentGuid];
                }

                if (!innerWorkerMap.ContainsKey(edge.InternalOutputComponentGuid))
                {
                    var componentEntry = this.store[edge.OutputComponentGuid];
                    var componentBinary = componentEntry.Item1;
                    var isAtomic = componentEntry.Item2;

                    if (isAtomic)
                    {
                        sourceWorker = new ComponentWorker(this.processingServer, edge.OutputComponentGuid, this.store[edge.OutputComponentGuid].Item1);
                        innerWorkerMap[edge.InternalInputComponentGuid] = sourceWorker;
                    }
                    else
                    {
                        MemoryStream deserializationStream = new MemoryStream(componentBinary);
                        var componentGraph = (IEnumerable<ComponentEdge>)bf.Deserialize(deserializationStream);
                        sourceWorker = new ComponentWorker(this.processingServer, this.store, edge.OutputComponentGuid, componentGraph);
                        innerWorkerMap[edge.InternalOutputComponentGuid] = sourceWorker;
                    }
                }
                else
                {
                    sourceWorker = innerWorkerMap[edge.InternalOutputComponentGuid];
                }

                DataGate edgeConnector = new DataGate();
                destinationWorker.InputGates[edge.InputValueID] = edgeConnector;
                sourceWorker.OutputGates[edge.OutputValueID] = edgeConnector;
            }
        }

        private void ProcessingServer_OnResultReceived(object sender, ResultReceivedEventArgs e)
        {
            if (e.JobGuid == this.jobId)
            {
                var result = e.Results.ToList();

                for (int i = 0; i < result.Count; i++)
                {
                    this.OutputGates[(uint)i].SendData(result[i]);
                }
            }
        }
    }
}

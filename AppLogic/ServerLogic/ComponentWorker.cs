using CommonRessources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppLogic.ServerLogic
{
    public class ComponentWorker
    {
        public readonly Dictionary<uint, DataGate> InputGates;
        public readonly Dictionary<uint, DataGate> OutputGates;

        private readonly Guid jobId;
        private readonly Guid componentGuid;
        private readonly byte[] assemblyBytes;
        private readonly IEnumerable<Core.Network.ComponentEdge> innerGraph;
        private readonly INetworkServer processingServer;
        private readonly Thread workerThread;

        public ComponentWorker(INetworkServer processingServer, Guid componentId, byte[] assembly)
        {
            this.InputGates = new Dictionary<uint, DataGate>();
            this.OutputGates = new Dictionary<uint, DataGate>();
            this.jobId = Guid.NewGuid();
            this.componentGuid = componentId;
            this.assemblyBytes = assembly;
            this.innerGraph = null;
            this.processingServer = processingServer;
            this.processingServer.OnResultReceived += ProcessingServer_OnResultReceived;

            Thread t = new Thread(new ThreadStart(ProcessIncomingData));
            t.Start();
        }

        public ComponentWorker(INetworkServer processingServer, Guid componentId, IEnumerable<Core.Network.ComponentEdge> graph)
        {
            this.InputGates = new Dictionary<uint, DataGate>();
            this.OutputGates = new Dictionary<uint, DataGate>();
            this.jobId = Guid.NewGuid();
            this.componentGuid = componentId;
            this.assemblyBytes = null;
            this.innerGraph = graph;
            this.processingServer = processingServer;
            this.processingServer.OnResultReceived += ProcessingServer_OnResultReceived;
        }

        public void Start()
        {
            this.workerThread.Start();
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

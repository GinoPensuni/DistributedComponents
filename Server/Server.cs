using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using CommonRessources;

namespace Server
{
    /// <summary>
    /// This class represents a server which only manages all internal clients (slaves).
    /// For external management, see ExternalServersManager
    /// </summary>
    public class Server : CommonServer, INetworkServer
    {
        private Thread listenThread;
        private TcpListener tcpListener;
        private NetworkState state;
        private Slave excludedSlave;

        public List<Slave> Slaves { get; private set; }

        // Guid = From Job Request from Client
        // Slave = handles the component
        // Component Handled component
        // A list of atomic components which will be handled by the slaves
        private List<Tuple<Guid, Slave, IComponent>> PendingComponentJobs
        {
            get;
            set;
        }

        public Dictionary<Guid, Slave> ExecutionCustomers { get; private set; }


        public void Run()
        {
            this.PendingComponentJobs = new List<Tuple<Guid, Slave, IComponent>>();
            this.ExecutionCustomers = new Dictionary<Guid, Slave>();

            this.Slaves = new List<Slave>();
            this.tcpListener = new TcpListener(IPAddress.Any, 8081);
            this.ServerState = NetworkState.Running;
            this.listenThread = new Thread(new ThreadStart(SlaveListening));

            this.listenThread.Start();
        }

        private void SlaveListening()
        {
            this.tcpListener.Start();

            while (this.ServerState == NetworkState.Running)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient();
                Thread slavehandlerthread = new Thread(new ParameterizedThreadStart(SlaveHandler));
                slavehandlerthread.Start(client);
            }
        }

        private void SlaveHandler(object clientobj)
        {
            TcpClient client = (TcpClient)clientobj;
            Slave slave = new Slave(client);

            slave.OnMessageReceived += Slave_OnMessageReceived;
            slave.OnSlaveDied += Slave_OnSlaveDied;
            slave.AssignGuid(Guid.NewGuid());

            this.Slaves.Add(slave);

            //
            // Only for testing purposes!!!!!
            //

            //Thread.Sleep(1000);
            //Component comp = new Component(Guid.NewGuid(), "test", new List<string>() { typeof(int).ToString(), typeof(int).ToString() }, new List<string>() { typeof(int).ToString(), typeof(string).ToString() });

            //ComponentMessage compmsg = new ComponentMessage(Guid.NewGuid());
            //compmsg.Component = comp;
            //compmsg.Values = new List<object>();

            //slave.SendMessage(compmsg);

            //Thread.Sleep(2000);

            //slave.SendInputParameter(compmsg.ID, 2, 0);

            //Thread.Sleep(2000);

            //slave.SendInputParameter(compmsg.ID, 2, 1);
        }

        void Slave_OnSlaveDied(object sender, SlaveDiedEventArgs e)
        {
            this.Slaves.Remove((Slave)sender);
            //Console.WriteLine("Client died");
        }

        public void Slave_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Slave slave = (Slave)sender;
            Console.WriteLine(slave.ClientGuid);
            Console.WriteLine(e.Msg.ToString());

            if (e.Msg is ResultMessage)
            {
                //Console.Write("Result id vom Komponent: ");

                //Console.WriteLine(((ResultMessage)e.Msg).ID);

                //foreach (object msg in ((ResultMessage)e.Msg).Result)
                //{
                //    Console.WriteLine(msg.ToString());
                //}

                if (this.OnResultReceived != null)
                {
                    ResultReceivedEventArgs resultArgs = new ResultReceivedEventArgs();
                    resultArgs.JobGuid = e.Msg.ID;
                    resultArgs.Results = ((ResultMessage)e.Msg).Result;
                    this.OnResultReceived(this, resultArgs);
                }
            }
            else if (e.Msg is ComponentMessage)
            {

            }
            else if (e.Msg is JobRequestMessage)
            {
                this.ExecutionCustomers.Add(e.Msg.ID, slave);
                this.excludedSlave = slave;

                Console.WriteLine("> Got a new job request from the client " + slave.ClientGuid);

                if (this.OnJobRequestReceived != null)
                {
                    ComponentRecievedEventArgs args = new ComponentRecievedEventArgs();

                    args.Component = ((JobRequestMessage)e.Msg).Component;
                    args.JobRequestGuid = e.Msg.ID;
                    args.Input = ((JobRequestMessage)e.Msg).Values.ToList();

                    this.OnJobRequestReceived(this, args);
                }
            }
            else if (e.Msg is SaveComponentMessage)
            {
                SaveComponentEventArgs args = new SaveComponentEventArgs();
                args.Component = ((SaveComponentMessage)e.Msg).Component;

                Console.WriteLine("> Client " + slave.ClientGuid + " asked me to save a component");

                if (this.OnUploadRequestReceived != null)
                {
                    this.OnUploadRequestReceived(this, args);
                }
            }
            else if (e.Msg is AvailableComponentsMessage)
            {
                RequestForAllComponentsReceivedEventArgs args = new RequestForAllComponentsReceivedEventArgs();
                AvailableComponentsMessage msg = (AvailableComponentsMessage)e.Msg;

                Console.WriteLine("> Client " + slave.ClientGuid + " asked me to send him all available components");
                
                if (this.OnAllAvailableComponentsRequestReceived != null)
                {
                    this.OnAllAvailableComponentsRequestReceived(this, args);
                }

                msg.AllAvailableComponents = args.AllAvailableComponents;

                slave.SendMessage(msg);
            }
        }

        public void Stop()
        {
            this.ServerState = NetworkState.Stopped;
            this.listenThread.Join();
        }

        //
        // Interface implementation
        public NetworkState ServerState
        {
            get
            {
                return this.state;
            }
            private set
            {
                this.state = value;

            }
        }

        public event EventHandler<ComponentRecievedEventArgs> OnJobRequestReceived;

        public event EventHandler<CommonRessources.ResultReceivedEventArgs> OnResultReceived;

        public event EventHandler<SaveComponentEventArgs> OnUploadRequestReceived;

        public event EventHandler<RequestForAllComponentsReceivedEventArgs> OnAllAvailableComponentsRequestReceived;

        public bool SendError(Guid jobRequestGuid, Exception logicException)
        {
            ErrorMessage errMsg = new ErrorMessage(Guid.NewGuid());
            errMsg.JobRequestGuid = jobRequestGuid;
            errMsg.Exception = logicException;

            try
            {
                Slave slave = this.ExecutionCustomers[jobRequestGuid];

                Console.WriteLine("Error message has been sent to client " + slave.Client + ". Description: " + errMsg.Exception.Message);

                return slave.SendMessage(errMsg);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendCalculatedResult(Guid id, Tuple<Guid, IEnumerable<object>, byte[]> job)
        {
            ComponentMessage compMsg = new ComponentMessage(Guid.NewGuid());
            compMsg.Values = job.Item2;
            compMsg.Assembly = job.Item3;
            compMsg.ComponentGuid = job.Item1;

            compMsg.ToBeExecuted = id;

            Random rand = new Random();
            var usedSlaves = this.Slaves.Where(sla => this.excludedSlave != sla).ToList();

            Slave s = this.Slaves[rand.Next(0, usedSlaves.Count)];

            Console.WriteLine("Send a component, which has to be executed, to the client " + s.ClientGuid);

            return s.SendComponent(compMsg);
        }

        public bool SendFinalResult(Guid jobRequestGuid, IEnumerable<object> result)
        {
            ResultMessage res = new ResultMessage(ResultStatusCode.Successful, jobRequestGuid);

            res.Result = result;

            try
            {
                Slave slave = this.ExecutionCustomers[jobRequestGuid];

                Console.WriteLine("Send the final result to client " + slave.ClientGuid);

                return slave.SendFinalResult(res);
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}


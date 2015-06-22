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
using AppLogic.ServerLogic;

namespace Server
{
    public class Server : INetworkServer
    {
        private Thread listenThread;
        private TcpListener tcpListener;
        private NetworkState state;

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

        public IServerLogic ServerLogic { get; private set; }

        public void Run()
        {
            this.ServerLogic = new ServerLogicCore(this);

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

            Thread.Sleep(1000);
            Component comp = new Component(Guid.NewGuid(), "test", new List<string>() { typeof(int).ToString(), typeof(int).ToString() }, new List<string>() { typeof(int).ToString(), typeof(string).ToString() });

            ComponentMessage compmsg = new ComponentMessage(Guid.NewGuid());
            compmsg.Component = comp;
            compmsg.Values = new List<object>();

            slave.SendMessage(compmsg);

            Thread.Sleep(2000);

            slave.SendInputParameter(compmsg.ID, 2, 0);

            Thread.Sleep(2000);

            slave.SendInputParameter(compmsg.ID, 2, 1);
        }

        void Slave_OnSlaveDied(object sender, SlaveDiedEventArgs e)
        {
            this.Slaves.Remove((Slave)sender);
            Console.WriteLine("Client died");
        }

        public void Slave_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Slave slave = (Slave)sender;
            Console.WriteLine(slave.ClientGuid);
            Console.WriteLine(e.Msg.ToString());

            if (e.Msg is ResultMessage)
            {
                Console.Write("Result id vom Komponent: ");
                Console.WriteLine(((ResultMessage)e.Msg).ID);
                foreach (object msg in ((ResultMessage)e.Msg).Result)
                {
                    Console.WriteLine(msg.ToString());
                }
            }
            else if (e.Msg is ComponentMessage)
            {
                
            }
            else if (e.Msg is JobRequestMessage)
            {
                this.ExecutionCustomers.Add(e.Msg.ID, slave);

                if (this.OnRequestEvent != null)
                {
                    ComponentRecievedEventArgs args = new ComponentRecievedEventArgs();

                    args.Component = ((JobRequestMessage)e.Msg).Component;
                    args.ToBeExceuted = e.Msg.ID;
                    args.Input = ((ComponentMessage)e.Msg).Values.ToList();

                    this.OnRequestEvent(this, args);
                }
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

        public event EventHandler<ComponentRecievedEventArgs> OnRequestEvent;


        public bool SendCalculatedResult(Guid id, List<Tuple<Guid, IComponent, byte[]>> Assembly)
        {
            Random rand = new Random();

            foreach (Tuple<Guid, IComponent, byte[]> item in Assembly)
            {

            }

            return false;
        }

        private void SendComponentToSlave(Tuple<Guid, IComponent, byte[]> component)
        {
            ComponentMessage mess = new ComponentMessage(component.Item1);
        }

        public bool SendError(Guid id, Exception logicException)
        {
            throw new NotImplementedException();
        }


        public bool SendCalculatedResult(Guid id, List<Tuple<Guid, Core.Network.Component, byte[]>> Assembly)
        {
            return false;
        }
    }
}


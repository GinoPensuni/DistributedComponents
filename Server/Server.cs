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

        public ILogic ServerLogic { get; private set; }

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
            Component comp = new Component(Guid.NewGuid(), "test", null, null);
            slave.OnMessageReceived += Slave_OnMessageReceived;
            slave.OnSlaveDied += Slave_OnSlaveDied;
            slave.AssignGuid(Guid.NewGuid());
            this.Slaves.Add(slave);
            Thread.Sleep(1000);
            slave.SendComponent(comp, null);
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
                if (e.Msg.Type == MessageType.RequestForJob)
                {
                    this.ExecutionCustomers.Add(e.Msg.ID, slave);

                    if (this.RequestEvent != null)
                    {
                        ComponentRecievedEventArgs args = new ComponentRecievedEventArgs();

                        args.Component = ((ComponentMessage)e.Msg).Component;
                        args.External = this.findOutIfExternal();
                        args.ToBeExceuted = e.Msg.ID;
                        args.Input = ((ComponentMessage)e.Msg).Values.ToList();

                        this.RequestEvent(this, args);

                        /*args.Component = ((ComponentMessage)e.Msg).Component;
                        args.ToBeExceuted = Guid.NewGuid();
                        //TODO: External wenns von anderem Server kommt.
                        args.External = false;
                        args.Input = ((ComponentMessage)e.Msg).Values.ToList();

                        this.Jobs.Add(args.ToBeExceuted, slave); // Which slave wanted a component to be executed?
                        this.RequestEvent(this, args);*/
                    }
                }
            }
        }

        private bool findOutIfExternal()
        {
            return false;
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
                return this.ServerState;
            }
            set
            {
                // why?
            }
        }

        public event EventHandler<ComponentRecievedEventArgs> RequestEvent;


        public bool SendResult(Guid id, List<Tuple<Guid, IComponent, byte[]>> Assembly)
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
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonRessources;

namespace Client
{
    public class Client : CommonClient, INetworkClient
    {
        public const int ServerConnectionPort = 8081;

        public const int liveCheckIntervall = 5000; // milli

        private TcpClient client;

        private NetworkStream networkStream;

        private NetworkState state;

        private DateTime lastAliveMessageFromServer;

        private bool isListening;

        // work work

        private List<WorkTask> workTasks; // A list of currently running tasks, which get executed by the client.

        public Client()
        {
            this.lastAliveMessageFromServer = DateTime.Now;
            this.workTasks = new List<WorkTask>();
        }

        
        public bool Connect(IPAddress ip, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            this.client = new TcpClient();

            try
            {
                this.client.Connect(endPoint);

                this.networkStream = this.client.GetStream();

                this.isListening = true;

                // this.NetworkClient = NetworkState.Running; // no guid :-(

                Thread thread = new Thread(new ThreadStart(this.Listen));
                thread.Start();

                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        private void Listen()
        {
            while (this.isListening)
            {
                if (this.networkStream.DataAvailable)
                {
                    byte[] received = new byte[4];

                    networkStream.Read(received, 0, 4);

                    byte[] message = new byte[BitConverter.ToUInt32(received, 0)];

                    networkStream.Read(message, 0, message.Length);

                    Message ma = Protocol.GetComponentMessageFromByteArray(message);

                    this.HandleMessage(ma);   
                }
                else
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("waiting...");

                    if ((DateTime.Now.Ticks - this.lastAliveMessageFromServer.Ticks) / TimeSpan.TicksPerMillisecond > Client.liveCheckIntervall)
                    {
                        this.Disconnect();
                    }
                }
            }
        }

        private void HandleMessage(Message ma)
        {
            if (ma is AssignMessage)
            {
                // Just send it back to let the server know that the client accepted this guid.
                this.clientGuid = ((AssignMessage)ma).ClientGuid;
                this.state = NetworkState.Running; // we have a guid, so we are running ^_^

                Console.WriteLine("> Got a GUID from the server: " + this.clientGuid);

                this.SendMessage((AssignMessage)ma);
            }
            else if (ma is AliveMessage)
            {
                // Just send it back to let the server know that the client still exists.
                this.SendMessage((AliveMessage)ma);

                this.lastAliveMessageFromServer = DateTime.Now;
            }
            else if (ma is ComponentMessage)
            {
                ComponentMessage compmsg = (ComponentMessage)ma;

                Thread thread = new Thread(new ParameterizedThreadStart(this.HandleComponentMessage));
                thread.Start(compmsg);
            }
            else if (ma is InputParameterMessage)
            {
                InputParameterMessage ipM = (InputParameterMessage)ma;

                List<WorkTask> foundTask = this.workTasks.Where(i => i.ID.Equals(ipM.WorkTaskGuid)).ToList();

                if (foundTask.Count > 0)
                {
                    foundTask[0].SetParameter(ipM.Value, ipM.Index);
                }
            }
        }

        // Please run in a new thread!!
        private void HandleComponentMessage(object data)
        {
            ComponentMessage msg = (ComponentMessage)data;
            WorkTask task = new WorkTask(msg.ID, msg.Component);

            // Maybe the message already contains all values?
            if (msg.Values.Count() == msg.Component.InputHints.Count())
            {
                int index = 0;

                foreach (object val in msg.Values)
                {
                    task.SetParameter(val, index);
                    index++;
                }
            }

            this.workTasks.Add(task);

            // Nah, stuipd server :-(
            while (!task.HasAllInputParameters())
            {
                Thread.Sleep(1000);
                Console.WriteLine("Waiting for input parameter");
            }

            msg.Values = task.InputParameters.ToList();

            if (this.OnRequestEvent != null)
            {
                ClientComponentEventArgs e = new ClientComponentEventArgs();

                //e.Component = msg.Component;
                e.Input = msg.Values.ToList();
                e.ToBeExceuted = msg.ToBeExecuted;
                e.Assembly = msg.Assembly;

                this.OnRequestEvent(this, e);
            }

            // will be taken over to the logic class
            this.ExecuteComponent(msg);
        }

        private void ExecuteComponent(object obj)
        {
            ComponentMessage compmsg = (ComponentMessage)obj;

            List<object> ho = compmsg.Component.Evaluate(compmsg.Values).ToList();

            for (int i = 0; i < ho.Count; i++)
            {
                Console.WriteLine(compmsg.Component.ComponentGuid.ToString());
            }

            ResultMessage rsm = new ResultMessage(ResultStatusCode.Successful, compmsg.ID);

            rsm.Result = compmsg.Component.Evaluate(compmsg.Values);

            byte[] response = Protocol.GetByteArrayFromMessage(rsm);

            networkStream.Write(response, 0, response.Length);
        }

        public bool SendMessage(Message message)
        {
            if (this.state != NetworkState.Running)
            {
                return false;
            }

            byte[] arr = Protocol.GetByteArrayFromMessage(message);

            try
            {
                this.networkStream.Write(arr, 0, arr.Length);

                return true;
            }
            catch(SocketException)
            {
                return false;
            }
        }

        //
        // Interface implementation

        public NetworkState NetworkClient
        {
            get
            {
                return this.state;
            }
            set
            {
                // why?
                //this.state = value;
            }
        }

        public bool SendResult(List<object> Result, Guid id)
        {
            ResultMessage resMessage = new ResultMessage(ResultStatusCode.Successful, id);
            resMessage.Result = Result;

            this.workTasks.Remove(this.workTasks.First(i => i.ID.Equals(id)));

            return this.SendMessage(resMessage);
        }

        public bool SendJobRequest(IComponent component)
        {
            /*
            ComponentMessage compMessage = new ComponentMessage(MessageType.RequestForJob, Guid.NewGuid());
            compMessage.Component = component;
            compMessage.Values = new List<object>();

            return this.SendMessage(compMessage);
            */
            return false;
        }

        public bool SendJobRequest(Core.Network.Component component)
        {
            JobRequestMessage jrMess = new JobRequestMessage(Guid.NewGuid());
            jrMess.Component = component;
            jrMess.Values = new List<object>();

            return this.SendMessage(jrMess);
        }

        public event EventHandler<ClientComponentEventArgs> OnRequestEvent;

        public void Connect(string ip)
        {
            IPAddress outIP;

            if (IPAddress.TryParse(ip, out outIP))
            {
                this.Connect(outIP, Client.ServerConnectionPort);
            }
        }


        public void Disconnect()
        {
            this.isListening = false;
            this.networkStream.Close();
            this.client.Close();
            this.state = NetworkState.Stopped;
        }
    }
}
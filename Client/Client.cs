using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonInterfaces;

namespace Client
{
    public class Client : CommonClient, INetworkClient
    {
        public const int ServerConnectionPort = 8081;

        private TcpClient client;

        private NetworkStream networkStream;

        private bool running;

        private NetworkState state;

        public Client()
        {
            this.running = false;
        }

        
        public bool Connect(IPAddress ip, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            this.client = new TcpClient();

            try
            {
                this.client.Connect(endPoint);

                this.networkStream = this.client.GetStream();

                this.running = true;

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
            while (running)
            {
                if (this.networkStream.DataAvailable)
                {
                    byte[] received = new byte[4];

                    networkStream.Read(received, 0, 4);

                    byte[] message = new byte[BitConverter.ToUInt32(received, 0)];

                    networkStream.Read(message, 0, message.Length);

                    Message ma = Protocol.GetComponentMessageFromByteArray(message);

                    if (ma is ComponentMessage)
                    {
                        ComponentMessage compmsg = (ComponentMessage)ma;

                        Thread thread = new Thread(new ParameterizedThreadStart(HandleComponent));

                        thread.Start(compmsg); 

                        if (this.RequestEvent != null)
                        {
                            ClientComponentEventArgs e = new ClientComponentEventArgs();

                            e.Component = compmsg.Component;
                            e.Input = compmsg.Values.ToList();
                            e.External = compmsg.External; // not neccessary
                            e.ToBeExceuted = compmsg.ToBeExecuted;
                            e.Assembly = compmsg.Assembly;

                            this.RequestEvent(this, e);
                        }
                    }
                    else if (ma is AliveMessage)
                    {
                        byte[] response = Protocol.GetByteArrayFromMessage((AliveMessage)ma);

                        networkStream.Write(response, 0, response.Length);
                    }
                    else if (ma is AssignMessage)
                    {
                        this.clientGuid = ((AssignMessage)ma).ClientGuid;
                        this.state = NetworkState.Running; // we have a guid, so we are running ^_^

                        Console.WriteLine("guid: " + this.clientGuid);

                        byte[] response = Protocol.GetByteArrayFromMessage((AssignMessage)ma);

                        networkStream.Write(response, 0, response.Length);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("waiting...");
                }
            }
        }

        private void HandleComponent(object obj)
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

            return this.SendMessage(resMessage);
        }

        public bool SendJobRequest(IComponent component)
        {
            ComponentMessage compMessage = new ComponentMessage(MessageType.RequestForJob, Guid.NewGuid());
            compMessage.Component = component;
            compMessage.Values = new List<object>();

            return this.SendMessage(compMessage);
        }

        public event EventHandler<ClientComponentEventArgs> RequestEvent;


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
            this.running = false;
            this.networkStream.Close();
            this.client.Close();
            this.state = NetworkState.Stopped;
        }
    }
}
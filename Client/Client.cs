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
        private TcpClient client;

        private NetworkStream networkStream;

        private bool running;

        private NetworkState networkClient;

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
                        Thread thread = new Thread(new ParameterizedThreadStart(HandleComponent));

                        ComponentMessage compmsg = (ComponentMessage)ma;

                        thread.Start(compmsg);                      
                    }
                    else if (ma is AliveMessage)
                    {
                        byte[] response = Protocol.GetByteArrayFromMessage((AliveMessage)ma);

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

        public NetworkState NetworkClient
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool SendResult(List<object> Result, Guid id)
        {
            throw new NotImplementedException();
        }

        public bool SendJobRequest(IComponent component)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<ComponentRecievedEventArgs> RequestEvent;
    }
}
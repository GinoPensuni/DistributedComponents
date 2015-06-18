using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using CommonInterfaces;

namespace Server
{
    public class Server : INetworkServer      
    {
        private Thread listenThread;
        private TcpListener tcpListener;
        public bool isRunning { get; private set; }

        public List<TcpClient> ClientList { get; private set; }

        public List<Slave> Slaves { get; private set; }
        public NetworkState ServerState
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

        public event EventHandler<ComponentRecievedEventArgs> RequestEvent;

        public void Run()
        {
            this.Slaves = new List<Slave>();
            this.ClientList = new List<TcpClient>();
            this.tcpListener = new TcpListener(IPAddress.Any, 8081);
            this.isRunning = true;
            this.listenThread = new Thread(new ThreadStart(SlaveListening));
            this.listenThread.Start();
        }

        public bool SendResult(List<object> Result, Guid id)
        {
            return false;
        }

        private void SlaveListening()
        {
            this.tcpListener.Start();

            while (this.isRunning)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient();
                Slave slave = new Slave(client);
                slave.OnMessageReceived += slave_OnMessageReceived;
                this.Slaves.Add(slave);
            }
        }

        void slave_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Msg.ToString());
        }

        public void ClientWorker(object clientobj)
        {
            //TODO: connection trennen
            TcpClient client = (TcpClient)clientobj;
            NetworkStream clientStream = client.GetStream();

            this.SendComponents(clientStream);          

            while (this.ClientList.Contains(client))
            {
                if (clientStream.DataAvailable)
                {
                    byte[] length = new byte[4];

                    clientStream.Read(length, 0, length.Length);

                    byte[] response = new byte[BitConverter.ToInt32(length, 0)];

                    clientStream.Read(response, 0, response.Length);

                    Message msg = Protocol.GetComponentMessageFromByteArray(response);
                    Console.WriteLine("Result: ");
                    if (msg is ResultMessage)
                    {
                        List<object> dd = ((ResultMessage)msg).Result.ToList();

                        for (int i = 0; i < dd.Count; i++)
                        {
                            Console.WriteLine(dd[i].ToString());
                        }
                    }
                }
                else
                {
                    Console.WriteLine("waiting...");
                    Thread.Sleep(1000);
                }
            }

            clientStream.Close();
            client.Close();
        }

        public void SendComponents(NetworkStream clientStream)
        {
            ComponentMessage msg = new ComponentMessage(Guid.NewGuid());
            msg.Component = new Component(Guid.NewGuid(), "test", null, null);

            byte[] test = Protocol.GetByteArrayFromMessage(msg);

            clientStream.Write(test, 0, test.Length);
            Console.WriteLine("Component sent");
        }

        public void Stop()
        {
            this.isRunning = false;
            this.listenThread.Join();
        }

        public void AskClientWorker(object clientobj)
        {
            TcpClient client = (TcpClient)clientobj;
            NetworkStream stream = client.GetStream();
            bool clientAlive = true;
            stream.ReadTimeout = 3000;

            while (clientAlive)
            {
                Thread.Sleep(30000);
                Guid aliveGuid = Guid.NewGuid();
                AliveMessage aliveMsg = new AliveMessage(aliveGuid);
                Message msg = null;

                try
                {

                    byte[] alivebytes = Protocol.GetByteArrayFromMessage(aliveMsg);
                    stream.Write(alivebytes, 0, alivebytes.Length);

                    Console.WriteLine("asked client");
                    byte[] response = new byte[alivebytes.Length - 4];
                    byte[] lengthBytes = new byte[4];
                    stream.Read(lengthBytes, 0, lengthBytes.Length);

                    if (BitConverter.ToInt32(lengthBytes, 0) == alivebytes.Length - 4)
                    {
                        stream.Read(response, 0, response.Length);
                    }

                    msg = Protocol.GetComponentMessageFromByteArray(response);
                    if (!msg.ID.Equals(aliveGuid) || !(msg is AliveMessage))
                    {
                        clientAlive = false;
                    }
                }
                catch (Exception e)
                {
                    clientAlive = false;
                }


            }

            this.RemoveClientFromList(client);
        }

        public void RemoveClientFromList(TcpClient client)
        {
            this.ClientList.Remove(client);
            Console.WriteLine("Client disconnected!");
        }
    }
}


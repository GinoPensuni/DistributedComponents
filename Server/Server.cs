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
    public class Server
    {
        private Thread listenThread;
        private TcpListener tcpListener;
        public bool isRunning { get; private set; }

        public List<TcpClient> ClientList { get; private set; }

        public void Run()
        {
            this.ClientList = new List<TcpClient>();
            this.tcpListener = new TcpListener(IPAddress.Any, 8081);
            this.isRunning = true;
            this.listenThread = new Thread(new ThreadStart(ClientListening));
            this.listenThread.Start();
        }

        public void ClientListening()
        {
            this.tcpListener.Start();

            while (this.isRunning)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient();
                this.ClientList.Add(client);
                Thread clientWorker = new Thread(new ParameterizedThreadStart(ClientWorker));
                clientWorker.Start(client);
            }
        }

        public void ClientWorker(object clientobj)
        {
            TcpClient client = (TcpClient)clientobj;

            NetworkStream clientStream = client.GetStream();

            this.SendComponents(clientStream);

            while (true)
            {
                if (clientStream.DataAvailable)
                {
                    /*StreamReader str = new StreamReader(clientStream);
                    string packet = str.ReadToEnd();
                    Console.WriteLine(packet);*/

                    byte[] length = new byte[4];

                    clientStream.Read(length, 0, length.Length);

                    byte[] response = new byte[BitConverter.ToInt32(length, 0)];

                    clientStream.Read(response, 0, response.Length);

                    Message msg = Protocol.GetComponentMessageFromByteArray(response);

                    if (msg is ResultMessage)
                    {
                        List<object> dd = ((ResultMessage)msg).Result.ToList();

                        for (int i = 0; i< dd.Count; i++)
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
        }

        public void Stop()
        {
            this.isRunning = false;
            this.listenThread.Join();
        }
    }
}


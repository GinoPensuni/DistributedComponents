using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    public class Server
    {
        private Thread listenThread;
        private TcpListener tcpListener;
        private bool isRunning;

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

            if (clientStream.DataAvailable)
            {
                StreamReader str = new StreamReader(clientStream);
                string packet = str.ReadToEnd();
                Console.WriteLine(packet);
            }
        }

        public void SendComponents(NetworkStream clientStream)
        {
            byte[] test = Encoding.UTF8.GetBytes("test");
            clientStream.Write(test, 0, test.Length);
        }

        public void Stop()
        {
            this.isRunning = false;
            this.listenThread.Join();
        }
    }
}


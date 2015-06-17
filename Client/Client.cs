using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Client
    {
        private TcpClient client;

        private NetworkStream networkStream;

        private bool running;

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
                }
                else
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("waiting...");
                }
            }
        }
    }
}
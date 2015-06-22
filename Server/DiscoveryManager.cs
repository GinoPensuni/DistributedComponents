using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class DiscoveryManager
    {
        public const int BroadcastPort = 8080;

        private bool isListening;

        public DiscoveryManager()
        {
            this.isListening = false;
        }

        public void StartListening()
        {
            this.isListening = true;

            Thread t = new Thread(new ThreadStart(this.ListenToBroadcast));
            t.Start();
        }

        public void StopListening()
        {
            this.isListening = false;
        }

        private void ListenToBroadcast()
        {
            UdpClient listener = new UdpClient(DiscoveryManager.BroadcastPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, DiscoveryManager.BroadcastPort);

            byte[] receivedData;

            while (this.isListening)
            {
                Console.WriteLine("waiting...");

                receivedData = listener.Receive(ref groupEP);

                Console.WriteLine("data received: " + Encoding.UTF8.GetString(receivedData, 0, receivedData.Length));
            }
        }
    }
}

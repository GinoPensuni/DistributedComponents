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
        private bool isListening;

        private bool isSending;

        private UdpClient client;

        public event EventHandler<BroadcastReceivedEventArgs> OnBroadcastReceived;

        public DiscoveryManager()
        {
            this.client = new UdpClient(Protocol.BroadcastPort);
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

        public void StartSendingBroadcasts()
        {
            this.isSending = true;

            Thread t = new Thread(new ThreadStart(this.SendBroadcasts));
            t.Start();
        }

        public void StopSendingBroadcasts()
        {
            this.isSending = false;
        }

        private void ListenToBroadcast()
        {
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, Protocol.BroadcastPort);

            byte[] receivedData;

            while (this.isListening)
            {
                Console.WriteLine("  > Waiting for broadcasts!");

                receivedData = this.client.Receive(ref groupEP);
                Console.WriteLine("  > Broadcast received.");

                IPHostEntry Host = Dns.GetHostEntry(Dns.GetHostName());

                if (this.OnBroadcastReceived != null && !Host.AddressList.Contains(groupEP.Address))
                {
                    BroadcastReceivedEventArgs args = new BroadcastReceivedEventArgs();
                    args.ReceivedData = Encoding.UTF8.GetString(receivedData);
                    args.SenderEndPoint = groupEP;
                    this.OnBroadcastReceived(this, args);
                }
            }
        }

        private void SendBroadcasts()
        {
            while (this.isSending)
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, Protocol.BroadcastPort);
                byte[] bytes = Protocol.GetBytesFromBroadcastText();
                this.client.Send(bytes, bytes.Length, ip);
                Console.WriteLine("  > Broadcast sent");
                Thread.Sleep(Protocol.BroadcastSendingInterval);
            }
        }
    }
}

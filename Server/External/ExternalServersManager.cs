using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Network;
using Core.Component;

namespace Server
{
    public class ExternalServersManager
    {
        private bool isListening;

        private CommonServer masterServer;

        private DiscoveryManager discMan;

        public ExternalServersManager(CommonServer masterServer)
        {
            this.masterServer = masterServer;
            this.ExternalServers = new Dictionary<IPAddress, ExternalServer>();

            discMan = new DiscoveryManager();
            discMan.OnBroadcastReceived += discMan_OnBroadcastReceived;
        }

        private void discMan_OnBroadcastReceived(object sender, BroadcastReceivedEventArgs e)
        {
            this.AddExternalServer(e.SenderEndPoint.Address);
        }

        public Dictionary<IPAddress, ExternalServer> ExternalServers { get; private set; }

        public delegate void ExternalServerLoggedOn(ExternalServer extServer);

        public delegate void ExternalServerTerminated(ExternalServer extServer);

        public event ExternalServerLoggedOn OnExternalServerLoggedOn;

        public event ExternalServerTerminated OnExternalServerTerminated;

        public void StartListening()
        {
            this.isListening = true;

            Thread t = new Thread(new ThreadStart(this.ListenToExternalServers));
            t.Start();

            discMan.StartSendingBroadcasts();
            discMan.StartListening();
        }

        public void StopListening()
        {
            this.isListening = false;

            discMan.StopSendingBroadcasts();
            discMan.StopListening();
        }

        public bool AddExternalServer(IPAddress address)
        {
            if (!this.ExternalServers.ContainsKey(address))
            {
                ExternalServer extServer = new ExternalServer(address, masterServer);

                extServer.OnLogonCompleted += extServer_OnLogonCompleted;
                extServer.OnTerminated += extServer_OnDied;

                this.ExternalServers.Add(address, extServer);
            }

            return false;
        }

        private void extServer_OnDied(object sender, ExternalServerDiedEventArgs e)
        {
            this.ExternalServers.Remove(e.ExternalServer.Address);

            if (this.OnExternalServerTerminated != null)
            {
                this.OnExternalServerTerminated(e.ExternalServer);
            }
        }

        private void extServer_OnLogonCompleted(object sender, ExternalServerLogonCompletedEventArgs e)
        {
            if (this.OnExternalServerLoggedOn != null)
            {
                this.OnExternalServerLoggedOn(e.ExternalServer);
            }
        }

        private void ListenToExternalServers()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, Protocol.ExternalServerCommunicationPort);
            listener.Start();

            while (this.isListening)
            {
                TcpClient client = listener.AcceptTcpClient();

                IPAddress address = ((IPEndPoint)client.Client.RemoteEndPoint).Address;

                if (!ExternalServers.ContainsKey(address))
                {
                    this.AddExternalServer(address);
                }

                ExternalServer extServer = this.ExternalServers[address];

                // handle external server

                extServer.StartListening(client);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Network;
using Core.Component;

namespace Server
{
    /// <summary>
    /// CommonServer = intern or external server
    /// </summary>
    public class CommonServer
    {
        public CommonServer(int t)
        {
            this.ServerGuid = Guid.NewGuid();
            this.Terminate = false;
            this.CpuLoad = 0;
            this.AvailableClients = new List<ClientInfo>();
            this.AvailableComponents = new List<Component>();
            this.FriendlyName = "Gino der Kaninchenfresser";
            this.NumberOfClients = 0;
        }

        public CommonServer()
        {

        }

        public Guid ServerGuid { get; protected set; }

        public string FriendlyName { get; protected set; }

        public List<Component> AvailableComponents { get; protected set; }

        public List<ClientInfo> AvailableClients { get; protected set; }

        public int NumberOfClients { get; protected set; }

        public int CpuLoad { get; protected set; }

        public bool Terminate { get; protected set; }
    }
}

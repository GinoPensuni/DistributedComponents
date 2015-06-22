using CommonRessources;
using DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLogic.ServerLogic
{
    public class ServerLogicCore : IServerLogic, ILogic
    {
        private INetworkServer serverReference;
        private IStore store;

        private static readonly Object syncRoot = new Object();
        private static bool isInstantiated = false;

        public ServerLogicCore(INetworkServer server)
        {
            lock (ServerLogicCore.syncRoot)
            {
                if (ServerLogicCore.isInstantiated)
                {
                    throw new InvalidOperationException("No two instances of ServerLogicCore should be active at the same time!");
                }
                else
                {
                    ServerLogicCore.isInstantiated = true;
                    this.serverReference = server;
                    this.serverReference.OnRequestEvent += ServerReference_RequestEvent;
                    this.store = new ComponentStore();
                }
            }
        }

        private void ServerReference_RequestEvent(object sender, ComponentRecievedEventArgs e)
        {

        }

        private void DeliverComponent(ComponentRecievedEventArgs e)
        {
            if (e.Component.IsAtomic)
            {
                byte[] assemblyBytes = this.store[e.Component.ComponentGuid];
                this.serverReference.SendCalculatedResult()
            }
        }

    }
}

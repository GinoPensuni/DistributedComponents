using CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLogic.ServerLogic
{
    public class ServerLogicCore : ILogic
    {
       private INetworkServer serverReference;

        private static readonly Object syncRoot = new Object();
        private static bool isInstaniated = false;

        public ServerLogicCore(INetworkServer server)
       {
           lock (ServerLogicCore.syncRoot)
           {
               if (ServerLogicCore.isInstaniated)
              {
                   throw new InvalidOperationException("No two instances of ServerLogicCore should be active at the same time!");
               }
               else
               {
                   ServerLogicCore.isInstaniated = true;
                   this.serverReference = server;
                   this.serverReference.RequestEvent += serverReference_RequestEvent;
               }
           }
       }

        private void serverReference_RequestEvent(object sender, ComponentRecievedEventArgs e)
        {
            
        }

        //private void Process
        public Task<bool> DisconnectFromServer()
        {
            throw new NotImplementedException();
        }

        public Task SaveComponent(IComponent component)
        {
            throw new NotImplementedException();
        }

        public void ConnenctToServer()
        {
            throw new NotImplementedException();
        }

        public Task<List<Tuple<Type, IComponent>>> LoadComponents()
        {
            throw new NotImplementedException();
        }
    }
}

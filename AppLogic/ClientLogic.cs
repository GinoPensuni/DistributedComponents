using CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkClient = Client.Client;
namespace AppLogic
{
    public class ClientLogic  : ILogic
    {
        private static readonly INetworkClient client = new NetworkClient();

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

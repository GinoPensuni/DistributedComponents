using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public interface IClientLogic : ILogic
    {
        Task<bool> DisconnectFromServer();

        Task SaveComponent(IComponent component);

        Task ConnenctToServer(string ip);

        Task<List<Tuple<Type, IComponent>>> LoadComponents();
    }
}

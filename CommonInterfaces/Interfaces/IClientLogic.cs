using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;

namespace CommonRessources
{
    public interface IClientLogic : ILogic
    {
        Task<bool> DisconnectFromServer();

        Task SaveComponent(Core.Network.Component component);

        Task ConnenctToServer(string ip);

        Task<List<Tuple<ComponentType, Core.Network.Component>>> LoadComponents();

        event EventHandler<LoadedCompoentEventArgs> OnComponentsLoaded;
    }
}

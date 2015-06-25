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

        void ConnenctToServer(string ip);

        Task LoadComponents();

        event EventHandler<LoadedCompoentEventArgs> OnComponentsLoaded;
    }
}

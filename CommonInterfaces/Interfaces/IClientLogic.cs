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

        Task RunCompoenet(Core.Network.Component component);

        event EventHandler<LoadedCompoentEventArgs> OnComponentsLoaded;
    }
}

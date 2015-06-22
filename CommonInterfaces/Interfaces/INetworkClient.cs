using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public interface INetworkClient
    {
        NetworkState NetworkClient
        {
            get;
            set;
        }

        bool SendResult(List<object> Result, Guid id);

        bool SendJobRequest(Core.Network.Component component);

        event EventHandler<ClientComponentEventArgs> OnRequestEvent;

        void Connect(string ip);

        void Disconnect();
    }
}

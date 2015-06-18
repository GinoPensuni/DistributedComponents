using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    public interface INetworkClient
    {
        NetworkState NetworkClient
        {
            get;
            set;
        }

        bool SendResult(List<object> Result, Guid id);

        bool SendJobRequest(IComponent component);

        event EventHandler<ComponentRecievedEventArgs> RequestEvent;

        void Connect(string ip);

        void DisconnectConnect();
    }
}

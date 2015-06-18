using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    public interface INetworkServer
    {
        NetworkState ServerState
        {
            get;
            set;
        }


        bool SendResult(List<object> Result, Guid id);

        event EventHandler<ComponentRecievedEventArgs> RequestEvent;
    }
}

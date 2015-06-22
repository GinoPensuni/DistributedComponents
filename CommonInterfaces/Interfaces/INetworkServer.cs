using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources.Interfaces;

namespace CommonRessources
{
    public interface INetworkServer
    {
        NetworkState ServerState
        {
            get;
        }

        bool SendCalculatedResult(Guid id, Tuple<Guid, Core.Network.Component, IEnumerable<object>, byte[]> job);

        bool SendError(Guid id, Exception logicException);

        event EventHandler<ComponentRecievedEventArgs> OnRequestEvent;

        event EventHandler<ResultReceivedEventArgs> OnResultReceived;
    }
}

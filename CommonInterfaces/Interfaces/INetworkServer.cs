using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public interface INetworkServer
    {
        NetworkState ServerState
        {
            get;
        }

        bool SendCalculatedResult(Guid id, List<Tuple<Guid, IComponent, byte[]>> Assembly);

        bool SendError(Guid id, Exception logicException);

        event EventHandler<ComponentRecievedEventArgs> OnRequestEvent;
    }
}

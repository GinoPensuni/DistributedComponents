using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;

namespace CommonRessources
{
    public interface INetworkServer
    {
        NetworkState ServerState
        {
            get;
        }

        /// <summary>
        /// The component to be uploaded.
        /// </summary>
        event EventHandler<SaveComponentEventArgs> OnUploadRequestReceived;

        event EventHandler<ResultReceivedEventArgs> OnResultReceived;

        event EventHandler<ComponentRecievedEventArgs> OnJobRequestReceived;

        /// <summary>
        /// Set property AllAvailableComponents!!
        /// </summary>
        event EventHandler<RequestForAllComponentsReceivedEventArgs> OnAllAvailableComponentsRequestReceived;

        bool SendCalculatedResult(Guid id, Tuple<Guid, IEnumerable<object>, byte[]> job);

        bool SendFinalResult(Guid id, IEnumerable<object> result);

        bool SendError(Guid id, Exception logicException);

        void Run();
    }
}

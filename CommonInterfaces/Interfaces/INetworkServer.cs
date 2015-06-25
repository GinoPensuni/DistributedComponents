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

        bool SendCalculatedResult(Guid id, Tuple<Guid, IEnumerable<object>, byte[]> job);

        bool SendError(Guid id, Exception logicException);

        event EventHandler<ComponentRecievedEventArgs> OnJobRequestReceived;

        event EventHandler<ResultReceivedEventArgs> OnResultReceived;
       
        /// <summary>
        /// The component to be uploaded.
        /// </summary>
        event EventHandler<SaveComponentEventArgs> OnUploadRequestReceived;
        bool SendFinalResult(Guid id, IEnumerable<object> result);

        void Run();
    }
}

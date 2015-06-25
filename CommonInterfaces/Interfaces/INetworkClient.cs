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

        /// <summary>
        /// Uplpoads the component to the server which sends it to the logic for storing propuses.
        /// </summary>
        /// <param name="dynamit">The component</param>
        /// <returns>True if uploaded</returns>
        bool UploadComponent(Core.Network.Component component);

        bool SendResult(List<object> Result, Guid id);

        bool SendJobRequest(Guid jobRequestGuid, Core.Network.Component component);

        bool RequestAllAvailableComponents();

        event EventHandler<ClientComponentEventArgs> OnComponentExecutionRequestEvent;

        event EventHandler<ResultReceivedEventArgs> OnFinalResultReceived;

        event EventHandler<ErrorReceivedEventArgs> OnErrorReceived;

        event EventHandler<RequestForAllComponentsReceivedEventArgs> OnAllAvailableComponentsResponseReceived;

        void Connect(string ip);

        void Disconnect();
    }
}

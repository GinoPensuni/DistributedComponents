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

        /// <summary>
        /// Uplpoads the component to the server which sends it to the logic for storing propuses.
        /// </summary>
        /// <param name="dynamit">The component</param>
        /// <returns>True if uploaded</returns>
        bool uploadComponent(Core.Network.Component bomb);
    }
}

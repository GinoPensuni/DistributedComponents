using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Network;
using Core.Component;

namespace Server
{
    public class ExternalClientUpdateRequestEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the unique id for this message.
        /// </summary>
        /// <value>A unique identifier.</value>
        public Guid ClientUpdateRequestGuid { get; set; }

        /// <summary>
        /// Gets or sets the ClientInfo of the client that is being updated.
        /// </summary>
        /// <value>The client information.</value>
        public ClientInfo ClientInfo { get; set; }

        /// <summary>
        /// Gets or sets the updated state of the client.
        /// </summary>
        /// <value>A ClientState.</value>
        public ClientState ClientState { get; set; }
    }
}

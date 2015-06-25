using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Network;
using Core.Component;

namespace Server
{
    public class InternalClientUpdateResponseEventArgs : EventArgs
    {
        public InternalClientUpdateResponseEventArgs()
        {
            this.Processed = true;
        }

        /// <summary>
        /// Gets or sets the unique id of the confirmed message.
        /// </summary>
        /// <value>A unique identifier.</value>
        public Guid ClientUpdateRequestGuid { get; set; }

        public bool Processed { get; set; }
    }
}

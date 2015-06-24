using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ExternalJobResponseEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the unique id of the received message.
        /// </summary>
        /// <value>A unique identifier.</value>
        public Guid JobRequestGuid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the job was accepted.
        /// Usually true - only when there are no clients, hop count too high etc. false.
        /// </summary>
        /// <value>True if accepted.</value>
        public bool IsAccepted { get; set; }

        public bool Processed { get; set; }
    }
}

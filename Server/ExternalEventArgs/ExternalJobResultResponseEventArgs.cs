using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ExternalJobResultResponseEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the unique id of the received message.
        /// </summary>
        /// <value>A unique identifier.</value>
        public Guid JobResultGuid { get; set; }
    }
}

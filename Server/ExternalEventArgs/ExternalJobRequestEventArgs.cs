using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Network;
using Core.Component;

namespace Server
{
    public class ExternalJobRequestEventArgs : System.EventArgs
    {
        public ExternalJobRequestEventArgs()
        {
            this.Processed = true;
        }

        /// <summary>
        /// Gets or sets the unique id for this message.
        /// </summary>
        /// <value>A unique identifier.</value>
        public Guid JobRequestGuid { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the job.
        /// </summary>
        /// <value>A unique identifier.</value>
        public Guid JobGuid { get; set; }

        /// <summary>
        /// Gets or sets the display name of the job.
        /// </summary>
        /// <value>A name string.</value>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets a collection of input arguments.
        /// </summary>
        /// <value>Collection of objects.</value>
        public IEnumerable<object> InputData { get; set; }

        /// <summary>
        /// Gets or sets the component which should be executed.
        /// </summary>
        /// <value>A Component.</value>
        public Component JobComponent { get; set; }

        /// <summary>
        /// Gets or sets the source client id.
        /// </summary>
        /// <value>A unique identifier.</value>
        public Guid JobSourceClientGuid { get; set; }

        /// <summary>
        /// Gets or sets an optional client to redirect the output data to.
        /// </summary>
        /// <value>A null-able unique identifier.</value>
        public Guid? TargetDisplayClient { get; set; }

        /// <summary>
        /// Gets or sets an optional client on which to compute the job on.
        /// </summary>
        /// <value>A unique identifier.</value>
        public Guid? TargetCalcClientGuid { get; set; }

        /// <summary>
        /// Gets or sets the number of times the message was forwarded.
        /// </summary>
        /// <value>An unsigned integer.</value>
        public uint HopCount { get; set; }

        /// <summary>
        /// This property will be sent back to the request sender by the response.
        /// </summary>
        public bool IsAccepted { get; set; }

        public bool Processed { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    /// <summary>
    /// This message is only for errors on job request level (not single atomic component execution).
    /// </summary>
    [Serializable]
    public class ErrorMessage : Message
    {
        public ErrorMessage(MessageType type, Guid ID) : base(ID,type)
        {
        }
        public ErrorMessage(Guid ID)
            : this(MessageType.Unknown, ID)
        {
        }

        public Guid JobRequestGuid { get; set; }

        public Exception Exception {get; set;}
    }
}

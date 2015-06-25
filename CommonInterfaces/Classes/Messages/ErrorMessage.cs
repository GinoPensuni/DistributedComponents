using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    /// <summary>
    /// This message will be sent when the client connects to a server. It assigns the client a guid.
    /// </summary>
    [Serializable] 
    public class AssignMessage : Message
    {
        public AssignMessage(MessageType type, Guid ID) : base(ID,type)
        {
        }
        public AssignMessage(Guid ID) : this(MessageType.Unknown, ID)
        {
        }

        public Guid ClientGuid
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{   
    /// <summary>
    /// This message will be used to check if the client is still alive.
    /// </summary>
    [Serializable]
    public class AliveMessage : Message
    {
        public AliveMessage(MessageType type, Guid ID) : base(ID,type)
        {
        }
        public AliveMessage(Guid ID) : this(MessageType.Unknown, ID)
        {
        }
    }
}

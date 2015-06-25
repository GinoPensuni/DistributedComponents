using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    /// <summary>
    /// This message will be used for the available request and respones from client and server.
    /// Request requires the property AllAvailableComponents to be null
    /// Response requires the property AllAvailableComponents to be not null!!
    /// </summary>
    public class AvailableComponentsMessage : Message
    {
        public AvailableComponentsMessage(MessageType type, Guid ID) : base(ID,type)
        {
        }
        public AvailableComponentsMessage(Guid ID)
            : this(MessageType.Unknown, ID)
        {
        }

        public List<Tuple<ComponentType, byte[]>> AllAvailableComponents
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public class AvailableComponentsMessage : Message
    {
        public AvailableComponentsMessage(MessageType type, Guid ID) : base(ID,type)
        {
        }
        public AvailableComponentsMessage(Guid ID)
            : this(MessageType.Unknown, ID)
        {
        }
    }
}

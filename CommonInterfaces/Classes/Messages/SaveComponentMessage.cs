using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    [Serializable]
    public class SaveComponentMessage : Message
    {
        public SaveComponentMessage(MessageType type, Guid ID) : base(ID, type)
        {
        }

        public SaveComponentMessage(ResultStatusCode status, Guid ID)
            : this(MessageType.Unknown, ID)
        {
            
        }

        public Core.Network.Component Component { get; set; }
    }
}

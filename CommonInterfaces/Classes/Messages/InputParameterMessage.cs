using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    [Serializable]
    public class InputParameterMessage : Message
    {
        public InputParameterMessage(MessageType type, Guid ID)
            : base(ID, type)
        {
        }
        public InputParameterMessage(Guid ID)
            : this(MessageType.Unknown, ID)
        {
        }

        // Guid of the work task, whose input parameters will be modified at the given Index (set with the given Value).
        public Guid WorkTaskGuid
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }
    }
}

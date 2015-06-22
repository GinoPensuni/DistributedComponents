using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    [Serializable]
    public class JobRequestMessage : Message
    {
        public JobRequestMessage(MessageType type, Guid ID)
            : base(ID, type)
        {
        }
        public JobRequestMessage(Guid ID)
            : this(MessageType.Unknown, ID)
        {
        }

        public byte[] Assembly
        {
            get;
            set;
        }

        public Guid ToBeExecuted
        {
            get;
            set;
        }

        public bool External
        {
            get;
            set;
        }

        public Core.Network.Component Component
        {
            get;
            set;
        }

        public IEnumerable<object> Values
        {
            get;
            set;
        }
    }
}

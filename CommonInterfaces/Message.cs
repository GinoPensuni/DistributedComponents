using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    [Serializable]
    public class Message
    {
        private Guid id;

        public Message(Guid id, MessageType type)
        {
            this.id = id;
            this.Type = type;
        }

        public Message(Guid id) : this(id, MessageType.Unknown)
        {
        }

        public MessageType Type
        {
            get;
            set;
        }

        public Guid ID
        {
            get
            {
                return this.id;
            }
        }
    }
}

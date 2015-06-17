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
        public Message(MessageType type)
        {
            this.Type = type;
        }

        public Message() : this(MessageType.Unknown)
        {
        }

        public MessageType Type
        {
            get;
            set;
        }
    }
}

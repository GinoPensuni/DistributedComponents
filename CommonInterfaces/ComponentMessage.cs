using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    [Serializable]
    public class ComponentMessage : Message
    {
        public ComponentMessage(MessageType type) : base(type)
        {
        }
        public ComponentMessage()
        {
        }

        public Component Component
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

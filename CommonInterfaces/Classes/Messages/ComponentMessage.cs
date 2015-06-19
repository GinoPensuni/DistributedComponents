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
        

        public ComponentMessage(MessageType type, Guid ID) : base(ID,type)
        {
        }
        public ComponentMessage(Guid ID) : this(MessageType.Unknown, ID)
        {
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

        public IComponent Component
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

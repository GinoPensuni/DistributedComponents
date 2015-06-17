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
        public Component Component
        {
            get;
            set;
        }
    }
}

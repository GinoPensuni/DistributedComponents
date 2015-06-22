using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public class ClientComponentEventArgs : EventArgs
    {
        public Core.Component.IComponent Component
        {
            get;
            set;
        }


        public Guid ToBeExceuted
        {
            get;
            set;
        }

        public List<object> Input
        {
            get;
            set;
        }

        public byte[] Assembly
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonRessources
{
    public class ComponentRecievedEventArgs : EventArgs
    {
        public Core.Network.Component Component
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces
{
    public class ComponentRecievedEventArgs : EventArgs
    {
        public bool External
        {
            get;
            set;
        }

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
    }
}

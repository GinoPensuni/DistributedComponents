using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public class SaveComponentEventArgs : EventArgs
    {
        public Core.Network.Component Component
        {
            get;
            set;
        }
    }
}

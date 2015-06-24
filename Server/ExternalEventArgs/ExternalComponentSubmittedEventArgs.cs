using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ExternalComponentSubmittedEventArgs : System.EventArgs
    {
        public Core.Network.Component Component { get; set; }

        public bool IsAccepted { get; set; }
    }
}

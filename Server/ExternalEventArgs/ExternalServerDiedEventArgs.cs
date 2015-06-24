using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Network;
using Core.Component;

namespace Server
{
    public class ExternalServerDiedEventArgs : System.EventArgs
    {
        public ExternalServer ExternalServer
        {
            get;
            set;
        }
    }
}

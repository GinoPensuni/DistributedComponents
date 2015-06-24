using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ExternalServerLogonCompletedEventArgs : System.EventArgs
    {
        public ExternalServer ExternalServer
        {
            get;
            set;
        }
    }
}

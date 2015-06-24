using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class InternalComponentSubmitResponseEventArgs : System.EventArgs
    {
        public Guid ComponentSubmitRequestGuid
        {
            get;
            set;
        }

        public bool IsAccepted
        {
            get;
            set;
        }
    }
}

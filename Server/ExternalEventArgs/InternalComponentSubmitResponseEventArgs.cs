using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class InternalComponentSubmitResponseEventArgs : System.EventArgs
    {
        public InternalComponentSubmitResponseEventArgs()
        {
            this.Processed = true;
        }

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

        public bool Processed
        {
            get;
            set;
        }
    }
}

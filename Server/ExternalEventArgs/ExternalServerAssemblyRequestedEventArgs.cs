using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ExternalServerAssemblyRequestedEventArgs : System.EventArgs
    {
        public ExternalServerAssemblyRequestedEventArgs()
        {
            this.Processed = true;
        }

        public Guid AssemblyRequestGuid { get; set; }

        public Guid ComponentGuid { get; set; }

        public byte[] BinaryContent { get; set; }

        public bool Processed { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public class ClientComponentEventArgs : ComponentRecievedEventArgs
    {
        public byte[] Assembly
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    public class ClientComponentEventArgs : ComponentRecievedEventArgs
    {
        byte[] Assembly
        {
            get;
            set;
        }
    }
}

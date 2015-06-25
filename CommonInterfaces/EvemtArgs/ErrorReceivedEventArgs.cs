using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public class ErrorReceivedEventArgs
    {
        public Guid ID
        {
            get;
            set;
        }

        public Exception Exception
        {
            get;
            set;
        }
    }
}

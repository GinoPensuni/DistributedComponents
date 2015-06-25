using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public class ErrorReceivedEventArgs
    {
        public Guid JobRequestGuid
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

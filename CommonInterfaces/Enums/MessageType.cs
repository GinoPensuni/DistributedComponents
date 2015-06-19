using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    [Serializable]
    public enum MessageType
    {
        RequestForComponentExecution = 1,
        RequestForJob = 2,

        Unknown = 9999
    }
}

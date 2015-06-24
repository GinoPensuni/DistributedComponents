using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLogic.ServerLogic
{
    public class GraphExecutionSet
    {
        public Guid JobGuid { get; set; }

        public Core.Network.Component Component { get; set; }

        public IEnumerable<object> Input { get; set; }

        public CommonServer Server { get; set; }
    }
}

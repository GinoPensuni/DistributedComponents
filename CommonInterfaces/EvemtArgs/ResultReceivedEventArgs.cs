using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonRessources
{
    public class ResultReceivedEventArgs
    {
        public Guid JobGuid { get; set; }

        public IEnumerable<object> Results { get; set; }
    }
}

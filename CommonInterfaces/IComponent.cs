using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    public interface IComponent
    {
        string friendlyName { get; }

        Guid uniqueID { get; }

        IEnumerable<string> inputHints { get; set; }

        IEnumerable<string> outputHints { get; set; }

        IEnumerable<object> evaluate();
    }
}

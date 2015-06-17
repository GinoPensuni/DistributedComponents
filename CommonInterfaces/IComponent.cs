using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    public interface IComponent
    {
        string FriendlyName { get; }

        Guid UniqueID { get; }

        IEnumerable<string> InputHints { get; set; }

        IEnumerable<string> OutputHints { get; set; }

        IEnumerable<object> Evaluate(IEnumerable<object> values);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public class RequestForAllComponentsReceivedEventArgs : EventArgs
    {
        public List<Tuple<ComponentType, byte[]>> AllAvailableComponents
        {
            get;
            set;
        }
    }
}

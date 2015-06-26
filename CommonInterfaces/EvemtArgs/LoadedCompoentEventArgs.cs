using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonRessources
{
    public class LoadedCompoentEventArgs : EventArgs
    {
        public List<Tuple<ComponentType, Core.Network.Component>> Components
        {
            get;
            set;
        }
    }
}

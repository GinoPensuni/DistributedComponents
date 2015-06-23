using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    [Serializable]
    public class SaveComponentMessage : Message
    {
        public Core.Network.Component Component { get; set; }
    }
}

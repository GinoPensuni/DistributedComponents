using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRessources
{
    public enum NetworkState
    {
        /// <summary>
        /// Server client communication established.
        /// </summary>
        Running = 1,

        /// <summary>
        /// Error during communication between client and Server.
        /// </summary>
        Error = 2,

        /// <summary>
        /// Communication between 
        /// </summary>
        Stopped = 3,
    }
}

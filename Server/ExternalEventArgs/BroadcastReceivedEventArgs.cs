using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class BroadcastReceivedEventArgs : EventArgs
    {
        public string ReceivedData { get; set; }

        public IPEndPoint SenderEndPoint { get; set; }
    }
}

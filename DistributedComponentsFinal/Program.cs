using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Client;
using Server;

namespace DistributedComponentsFinal
{
    class Program
    {
        static void Main(string[] args)
        {
            Client.Client c = new Client.Client();

            c.Connect(IPAddress.Parse("10.101.150.27"), 8081);


        }
    }
}

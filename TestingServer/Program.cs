using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace TestingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Server server = new Server.Server();
            server.Run();
        }
    }
}

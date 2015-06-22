using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Client;
using InputComponentWpf;

namespace DistributedComponentsFinal
{
    class Program
    {
        static void Main(string[] args)
        {
        //    dynamic dd = (new Int32()).GetType();

        //    Console.WriteLine(dd.ToString());

        //    Console.WriteLine("hello world!");

        //    //Console.ReadLine();
            
        //    Client.Client c = new Client.Client();

        //    c.Connect(IPAddress.Parse("10.101.150.27"), 8081);

            Input i = new Input();

            List<object> t = new List<object>();
            i.Evaluate(t);

            Console.ReadLine();
        }
    }
}

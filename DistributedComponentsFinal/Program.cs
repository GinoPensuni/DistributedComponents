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
using StringInputComponent;
using Server;

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

            // StringInputComponent.TextInput e = new StringInputComponent.TextInput();

            // e.Evaluate(new List<object>() { });

            CommonServer cm = new CommonServer(2);

            ExternalServersManager esm = new ExternalServersManager(cm);
            esm.StartListening();

            esm.OnExternalServerLoggedOn += esm_OnExternalServerLoggedOn;
            esm.OnExternalServerTerminated += esm_OnExternalServerTerminated;

            Console.WriteLine(uint.MaxValue);

            Console.WriteLine((uint)0);

            Console.ReadLine();
        }

        static void esm_OnExternalServerTerminated(ExternalServer extServer)
        {
            Console.WriteLine("external server died :-( " + extServer.FriendlyName);
        }

        static void esm_OnExternalServerLoggedOn(ExternalServer extServer)
        {
            Console.WriteLine("external server logged on " + extServer.FriendlyName);
            extServer.OnExternalComponentSubmitRequestReceived += extServer_OnExternalComponentSubmitRequestReceived;
            extServer.SendAssemblyRequest(Guid.NewGuid(), Guid.NewGuid());
        }

        static void extServer_OnExternalComponentSubmitRequestReceived(object sender, ExternalComponentSubmittedEventArgs e)
        {
            Console.WriteLine("external component submitted ");
        }
    }
}

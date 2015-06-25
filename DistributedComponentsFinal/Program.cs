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
using Client;
using Core.Component;
using Core.Network;
using System.Threading;

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

            Client.Client c = new Client.Client();

            c.Connect("10.101.100.11");

            Thread.Sleep(3000);

            Component aaa = new Component();
            aaa.ComponentGuid = Guid.NewGuid();
            aaa.IsAtomic = true;
            aaa.InputDescriptions = new List<string>();
            aaa.InputHints = new List<string>();
            aaa.OutputDescriptions = new List<string>();
            aaa.OutputHints = new List<string>();
            aaa.FriendlyName = "aaa";

            c.OnComponentExecutionRequestEvent += c_OnComponentExecutionRequestEvent;
            c.OnFinalResultReceived += c_OnFinalResultReceived;
            c.OnErrorReceived += c_OnErrorReceived;
            c.OnAllAvailableComponentsResponseReceived += c_OnAllAvailableComponentsResponseReceived;

            if (c.UploadComponent(aaa))
            {
                Console.WriteLine("uploaded component!!!!!!!!!!!!!!!!!!");
            }

            if (c.SendJobRequest(Guid.NewGuid(), aaa))
            {
                Console.WriteLine("job request sent!!!!!!!!!!!!!!!!!!");
            }

            if (c.RequestAllAvailableComponents())
            {
                Console.WriteLine("al req sent!!!");
            }


            /*CommonServer cm = new CommonServer(2);

            ExternalServersManager esm = new ExternalServersManager(cm);
            esm.StartListening();

            esm.OnExternalServerLoggedOn += esm_OnExternalServerLoggedOn;
            esm.OnExternalServerTerminated += esm_OnExternalServerTerminated;

            Console.WriteLine(uint.MaxValue);

            Console.WriteLine((uint)0);*/

            Console.ReadLine();
        }

        static void c_OnAllAvailableComponentsResponseReceived(object sender, CommonRessources.RequestForAllComponentsReceivedEventArgs e)
        {
            Console.WriteLine("all available components received!");
            int b = 839;
        }

        static void c_OnErrorReceived(object sender, CommonRessources.ErrorReceivedEventArgs e)
        {
            Console.WriteLine("error received >:-((((");
            int a = 45;
        }

        static void c_OnFinalResultReceived(object sender, CommonRessources.ResultReceivedEventArgs e)
        {
            Console.WriteLine("final result received!");
            int a = 0;
        }

        static void c_OnComponentExecutionRequestEvent(object sender, CommonRessources.ClientComponentEventArgs e)
        {
            Console.WriteLine("i must execute a component :-(");
            int a = 22;
            ((Client.Client)sender).SendResult(new List<object>() { "hallo", "jürgen" }, e.ToBeExceuted);
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

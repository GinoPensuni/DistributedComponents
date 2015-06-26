using AppLogic;
using AppLogic.ServerLogic;
using DataStore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComponentServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var filesInAssemblyDirectory = Directory.EnumerateFiles("Assemblies");
            var assemblies = GetLoadableAssemblies(filesInAssemblyDirectory);

            ComponentManager componentManager = ComponentManager.Instance;
            assemblies.ToList().ForEach(ass => componentManager.LoadAssemblyContents(ass));

            ComponentStore store = new ComponentStore();
            store.ClearDatabase();

            foreach (var loadedComponent in componentManager.LoadedComponents.Select(x => (LoadedComponent)x.Item2))
            {
                try
                {
                    store.Store(loadedComponent.ComponentGuid, loadedComponent.FriendlyName, true, loadedComponent.AssemblyCode);
                }
                catch (DuplicateNameException)
                {

                }
            }

            Server.Server masterServer = new Server.Server();
            Server.ExternalServersManager serverManager = new Server.ExternalServersManager((Server.CommonServer)masterServer);
            ServerLogicCore serverLogic = new ServerLogicCore(masterServer, serverManager, store);

            Console.WriteLine();
            Console.ReadLine();
        }

        private static IEnumerable<byte[]> GetLoadableAssemblies(IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                bool success;
                byte[] fileData = null;

                try
                {
                    Assembly.Load(File.ReadAllBytes(Path.GetFullPath(filePath)));
                    fileData = File.ReadAllBytes(filePath);
                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                }
                
                if(success) yield return fileData;
            }
        }
    }
}

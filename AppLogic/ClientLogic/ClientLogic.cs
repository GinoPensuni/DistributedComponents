using CommonRessources;
using DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkClient = Client.Client;
using System.Reflection;
using System.IO;

namespace AppLogic.ClientLogic
{
    public class ClientLogic : ILogic, IClientLogic
    {
        private static readonly IClientLogic instance = new ClientLogic();
        private static readonly INetworkClient client = new NetworkClient();
        private static readonly ComponentManager componentManager = ComponentManager.Instance;
        private static readonly ComponentStore componentStore = new ComponentStore();

        internal ComponentManager ComponentManager
        {
            get
            {
                return componentManager;
            }
            set
            {

            }
        }

        internal ComponentStore ComponentStore
        {
            get
            {
                return componentStore;
            }
            set
            {

            }
        }

        internal INetworkClient NetworkClient
        {
            get
            {
                return client;
            }
            set
            {

            }
        }

        public IClientLogic LogicClient
        {
            get
            {
                return ClientLogic.instance;
            }
            set
            {

            }
        }

        public Task SaveComponent(IComponent component)
        {
            var saveTask = new Task(() =>
            {
                Type componentType = component.GetType();
                Assembly assembly = componentType.Assembly;
                var stream = assembly.GetFiles().FirstOrDefault();

                try
                {
                    var length = (int)stream.Length;
                    byte[] content = new byte[length];
                    stream.Read(content, 0, length);
                    this.ComponentStore.Store(content);
                }
                catch
                {
                    throw new FileLoadException();
                }
            });

            saveTask.Start();
            return saveTask;
        }

        public Task<List<Tuple<ComponentType, Core.Network.Component>>> LoadComponents()
        {
            var loadingTask = new Task<List<Tuple<ComponentType, Core.Network.Component>>>(() =>
            {
                var assemblyData = this.ComponentStore.LoadAssemblies();
                foreach (var entry in assemblyData)
                {
                    this.ComponentManager.LoadAssemblyContents(entry.Assembly);
                }

                // TODO Make this shit work

                //return this.ComponentManager.LoadedComponents.Select(
                //    comp =>
                //    {
                //        return new Core.Network.Component()
                //        {
                //            ComponentGuid = comp.Item2.ComponentGuid,
                //            Edges = comp.Item1 == ComponentType.Complex ? comp.Item2.
                //        };
                //    });

                return new List<Tuple<ComponentType, Core.Network.Component>>();
            });

            loadingTask.Start();
            return loadingTask;

            throw new NotImplementedException();
        }

        Task<bool> IClientLogic.DisconnectFromServer()
        {
            var disconnectionTask = new Task<bool>(() =>
            {
                try
                {
                    this.NetworkClient.Disconnect();
                    return true;
                }
                catch
                {
                    return false;
                }
            });

            disconnectionTask.Start();
            return disconnectionTask;
        }

        Task IClientLogic.ConnenctToServer(string ip)
        {
            var connectionTask = new Task(() =>
            {
                try
                {
                    this.NetworkClient.Connect(ip);
                }
                catch
                {
                }
            });

            connectionTask.Start();
            return connectionTask;
        }


        public Task SaveComponent(Core.Network.Component component)
        {
            var saveTask = new Task(() =>
            {
                this.NetworkClient.UploadComponent(component);
            });

            saveTask.Start();
            return saveTask;
        }
    }
}

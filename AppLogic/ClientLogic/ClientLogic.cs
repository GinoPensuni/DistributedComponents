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
        private static readonly IClientLogic instance;
        private static readonly INetworkClient client; 
        private static readonly ComponentManager componentManager;
        private List<Tuple<ComponentType, Core.Component.IComponent>> LoadedCompoents;

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

        public static IClientLogic LogicClient
        {
            get
            {
                return ClientLogic.instance;
            }
            set
            {

            }
        }

        static ClientLogic() : this()
        {
            client = new NetworkClient();
            instance = new ClientLogic();
            componentManager = ComponentManager.Instance;
            
        }

        private  ClientLogic()
        {
            client.OnAllAvailableComponentsResponseReceived += client_OnAllAvailableComponentsResponseReceived;
        }

        void client_OnAllAvailableComponentsResponseReceived(object sender, RequestForAllComponentsReceivedEventArgs e)
        {
            this.LoadedCompoents = new List<Tuple<ComponentType, Core.Component.IComponent>>();
            foreach (var entry in e.AllAvailableComponents)
            {
                this.ComponentManager.LoadAssemblyContents(entry.Item2);
            }

            this.LoadedCompoents = this.ComponentManager.LoadedIComponents;
            if (this.OnComponentsLoaded != null)
            {
                try
                {
                    this.OnComponentsLoaded(this, new LoadedCompoentEventArgs() { Components = this.LoadedCompoents });
                }
                catch
                {

                }
            }
        }

        public Task LoadComponents()
        {
            var loadingTask = new Task(() =>
            {
                this.NetworkClient.RequestAllAvailableComponents();
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


        public event EventHandler<LoadedCompoentEventArgs> OnComponentsLoaded;
    }
}

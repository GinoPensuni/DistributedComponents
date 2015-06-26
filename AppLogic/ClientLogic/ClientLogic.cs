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
using System.Runtime.Serialization.Formatters.Binary;

namespace AppLogic.ClientLogic
{
    public class ClientLogic : ILogic, IClientLogic
    {
        private static readonly IClientLogic instance;
        private static readonly INetworkClient client; 
        private static readonly ComponentManager componentManager;
        private List<Tuple<ComponentType, Core.Network.Component>> LoadedCompoents;

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

        static ClientLogic()
        {
            client = new NetworkClient();
            instance = new ClientLogic();
            componentManager = ComponentManager.Instance;
            
        }

        private  ClientLogic()
        {
            client.OnAllAvailableComponentsResponseReceived += client_OnAllAvailableComponentsResponseReceived;
            client.OnComponentExecutionRequestEvent += client_OnComponentExecutionRequestEvent;
        }

        void client_OnComponentExecutionRequestEvent(object sender, ClientComponentEventArgs e)
        {
            if (sender == this.NetworkClient)
            {
                try
                {
                    this.ComponentManager.LoadAssemblyContents(e.Assembly);
                    this.evalResult =  this.ComponentManager.LoadedComponents.Single(comp => e.ComponentGuid == comp.Item2.ComponentGuid).Item2.Evaluate(e.Input);
                    this.NetworkClient.SendResult(this.evalResult.ToList(),e.ToBeExceuted);
                }
                catch
                {

                }
            }
        }

        void client_OnAllAvailableComponentsResponseReceived(object sender, RequestForAllComponentsReceivedEventArgs e)
        {
            if (sender == this.NetworkClient)
            {
                this.LoadedCompoents = new List<Tuple<ComponentType, Core.Network.Component>>();
                foreach (var entry in e.AllAvailableComponents)
                {
                    if (entry.Item1 == ComponentType.Simple)
                    {
                        this.ComponentManager.LoadAssemblyContents(entry.Item2);
                    }
                    else if (entry.Item1 == ComponentType.Complex)
                    {
                        MemoryStream ms = new MemoryStream(entry.Item2);
                        BinaryFormatter bf = new BinaryFormatter();
                        var component = bf.Deserialize(ms) as Core.Network.Component;
                        this.LoadedCompoents.Add(new Tuple<ComponentType, Core.Network.Component>(ComponentType.Complex, component));
                    }
                }

                Func<Tuple<ComponentType, Core.Component.IComponent>, Tuple<ComponentType, Core.Network.Component>> simpleComponentSelector =
                    tuple =>
                    {
                        Core.Component.IComponent comp = tuple.Item2;

                        var compTrans = new Core.Network.Component()
                        {
                            ComponentGuid = comp.ComponentGuid,
                            Edges = null,
                            FriendlyName = comp.FriendlyName,
                            InputDescriptions = comp.InputDescriptions,
                            InputHints = comp.InputHints,
                            IsAtomic = true,
                            OutputDescriptions = comp.OutputDescriptions,
                            OutputHints = comp.OutputHints,
                        };

                        return new Tuple<ComponentType, Core.Network.Component>(ComponentType.Simple, compTrans);
                    };


                this.LoadedCompoents.AddRange(this.ComponentManager.LoadedIComponents.Select(simpleComponentSelector));

                if (this.OnComponentsLoaded != null)
                {
                    //try
                    //{
                        this.OnComponentsLoaded(this, new LoadedCompoentEventArgs() { Components = this.LoadedCompoents });
                    //}
                    //catch
                    //{

                    //}
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

        void IClientLogic.ConnenctToServer(string ip)
        {
                try
                {
                    this.NetworkClient.Connect(ip);
                }
                catch
                {
                }

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
        private IEnumerable<object> evalResult;

        public Task RunComponenet(Core.Network.Component component)
        {
            var runTask = new Task(() =>
            {
                this.NetworkClient.SendJobRequest(Guid.NewGuid(), component);
            });

            runTask.Start();
            return runTask;

        }
    }
}

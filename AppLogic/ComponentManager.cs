using CommonRessources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppLogic
{
    public class ComponentManager
    {
        public static readonly ComponentManager Instance;
        private readonly HashSet<LoadedComponent> loadedComponents;
        private readonly HashSet<Assembly> loadedAssemblies;


        static ComponentManager()
        {
            Instance = new ComponentManager();
        }

        private ComponentManager()
        {
            this.loadedComponents = new HashSet<LoadedComponent>();
            this.loadedAssemblies = new HashSet<Assembly>();
        }

        public List<Tuple<ComponentType, IComponent>> LoadedComponents
        {
            get
            {
                return this.loadedComponents.Select(component => new Tuple<ComponentType, IComponent>(ComponentType.Simple, component)).ToList();
            }
        }

        public void LoadAssemblyContents(byte[] assemblyBytes) 
        {
            if (assemblyBytes == null)
            {
                throw new ArgumentNullException("assemblyBytes");
            }
            else
            {
                try
                {
                    var newAssembly = Assembly.Load(assemblyBytes);

                    if (!this.loadedAssemblies.Add(newAssembly))
                    {

                    }
                    else
                    {
                        this.loadedComponents.UnionWith(newAssembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Core.Component.IComponent))).Select(component => new LoadedComponent(component)));
                    }
                }
                catch (System.BadImageFormatException)
                {
                    throw;
                }
            }
        }

        public IEnumerable<object> EvaluateComponent(Guid componentId, IEnumerable<object> values)
        {
            try
            {
                return this.loadedComponents.First(loadedComponent => loadedComponent.ComponentGuid == componentId).Evaluate(values ?? Enumerable.Empty<object>());
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("This guid is not a valid guid for a component!");
            }
        }
    }
}

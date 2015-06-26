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

        public List<Tuple<ComponentType, LoadedComponent>> LoadedComponents
        {
            get
            {
                return this.loadedComponents.Select(component => new Tuple<ComponentType, LoadedComponent>(ComponentType.Simple, component)).ToList();
            }
        }

        public List<Tuple<ComponentType, Core.Component.IComponent>> LoadedIComponents
        {
            get
            {
                return this.loadedComponents.Select(component => new Tuple<ComponentType, Core.Component.IComponent>(ComponentType.Simple, (Core.Component.IComponent)component)).ToList();
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
                    var newComp = newAssembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(Core.Component.IComponent))).Select(component => new LoadedComponent(assemblyBytes, component));

                    if (newComp.Any(ncomp => this.loadedComponents.Any(lcomp => ncomp.ComponentGuid == lcomp.ComponentGuid)))
                    {
                    }
                    else
                    {
                        this.loadedAssemblies.Add(newAssembly);
                        this.loadedComponents.UnionWith(newComp);
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

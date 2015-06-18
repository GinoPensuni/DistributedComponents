using CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppLogic
{
    class LoadedComponent : CommonInterfaces.IComponent
    {

        private readonly Type componentType;
        private readonly 

        public LoadedComponent(Type component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            else
            {
                this.componentType = component;
            }
        }

        public Guid ComponentGuid
        {
            get { return this.InstanceOfComponent.ComponentGuid; }
        }

        public string FriendlyName
        {
            get { return this.InstanceOfComponent.FriendlyName; }
        }

        public IEnumerable<string> InputHints
        {
            get { return this.InstanceOfComponent.InputHints; }
        }

        public IEnumerable<string> OutputHints
        {
            get { return this.InstanceOfComponent.OutputHints; }
        }

        private IComponent InstanceOfComponent
        {
            get
            {
                return Activator.CreateInstance<Core.Component.IComponent>();
            }
        }


        public IEnumerable<object> Evaluate(IEnumerable<object> values)
        {
            var hints = this.InputHints.ToList();

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            else if (values.Count() != hints.Count)
            {
                throw new ArgumentException("The number of arguments does not match the count of input hints!");
            }
            else if(!values.Select((value, index) => value.GetType().ToString() == hints[index]).All(b => b == true))
            {
                throw new ArgumentException("The type of some arguments does not match the specified type!");
            }
            else
            {
                return this.InstanceOfComponent.Evaluate(values);
            }
        }
    }
}

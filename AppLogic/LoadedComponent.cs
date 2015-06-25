using CommonRessources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppLogic
{
    public class LoadedComponent : Core.Component.IComponent
    {

        public readonly byte[] AssemblyCode;
        public readonly Type ComponentType;

        public LoadedComponent(byte[] assemblyCode, Type component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            else if (assemblyCode == null)
            {
                throw new ArgumentNullException("assemblyCode");
            }
            else
            {
                this.ComponentType = component;
                this.AssemblyCode = assemblyCode;
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

        public IEnumerable<string> InputDescriptions
        {
            get
            {
                return this.InstanceOfComponent.InputDescriptions;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public IEnumerable<string> OutputDescriptions
        {
            get
            {
                return this.InstanceOfComponent.OutputDescriptions;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        private Core.Component.IComponent InstanceOfComponent
        {
            get
            {
                return (Core.Component.IComponent) Activator.CreateInstance(this.ComponentType);
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

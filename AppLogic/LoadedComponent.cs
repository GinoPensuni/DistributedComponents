using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppLogic
{
    class LoadedComponent : Core.Component.IComponent
    {
        Core.Component.IComponent instance;

        public LoadedComponent(Type component)
        {
            this.instance = Activator.CreateInstance<Core.Component.IComponent>();
        }

        public Guid ComponentGuid
        {
            get { return this.instance.ComponentGuid; }
        }

        public string FriendlyName
        {
            get { return this.instance.FriendlyName; }
        }

        public IEnumerable<string> InputHints
        {
            get { return this.instance.InputHints; }
        }

        public IEnumerable<string> OutputHints
        {
            get { return this.instance.OutputHints; }
        }

        public IEnumerable<object> Evaluate(IEnumerable<object> values)
        {
            throw new NotImplementedException();
        }
    }
}

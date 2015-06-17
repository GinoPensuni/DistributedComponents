using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    class Component : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        public Component(Guid guid, string name, IEnumerable<string> inputHints, IEnumerable<string> outputHints)
        {
            this.componentGuid = guid;

            this.friendlyName = name;

            this.inputHints = inputHints;

            this.outputHints = outputHints;
        }

        public Guid ComponentGuid
        {
            get 
            {
                return this.componentGuid;
            }
        }

        public string FriendlyName
        {
            get { return this.friendlyName; }
        }

        public IEnumerable<string> InputHints
        {
            get { return this.inputHints; }
        }

        public IEnumerable<string> OutputHints
        {
            get { return this.outputHints; }
        }
        public IEnumerable<object> Evaluate(IEnumerable<object> values)
        {
            throw new NotImplementedException();
        }
    }
}

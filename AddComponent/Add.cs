using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonInterfaces;

namespace AddComponent
{
    public class Add : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;


        public Guid ComponentGuid
        {
            get { }
        }

        public string FriendlyName
        {
            get { }
        }

        public IEnumerable<string> InputHints
        {
            get { }
        }

        public IEnumerable<string> OutputHints
        {
            get { }
        }

        public IEnumerable<object> Evaluate(IEnumerable<object> values)
        {

        }
    }
}

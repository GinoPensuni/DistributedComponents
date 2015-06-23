using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReverseListComponent
{
    public class ReverseList
    {
         private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        public ReverseList()
        {
            this.componentGuid = new Guid("CD212CBC-B79A-48A3-9498-372769BEB541");

            this.friendlyName = "Reverse List";

            this.inputHints = new List<string>() { typeof(List<object>).ToString() };

            this.outputHints = new List<string>() { typeof(List<object>).ToString() };            
        }
        public Guid ComponentGuid
        {
            get { return this.componentGuid; }
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
            bool checkValues = this.CheckIfAllowedValues(values);

            if (checkValues)
            {
                IEnumerable<object> result = values.Reverse();

                return result;
            }
            else
            {
                throw new ArgumentException("The value must be of the type as described in the input hints.");
            }
        }
        private bool CheckIfAllowedValues(IEnumerable<object> values)
        {
            if (values.GetType().ToString() == this.InputHints.GetType().ToString())
            {
                return true;
            }            
            else
            {
                return false;
            }
        }
    }
}

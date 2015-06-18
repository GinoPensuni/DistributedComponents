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

        public Add()
        {
            this.componentGuid = new Guid("82870640-2A79-4FE5-8F34-6075C0C4863F");

            this.friendlyName = "Addition";

            List<string> inputhints = new List<string>() {"System.Int32",
                                                          "System.Int32"};
            this.inputHints = inputhints;

            List<string> outputhint = new List<string>() { "System.Int32" };

            this.outputHints = outputhint;
                                                          
        }

        public Guid ComponentGuid
        {
            get { return this.componentGuid;  } 
        }

        public string FriendlyName
        {
            get { return this.friendlyName;  }
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
                int sum = 0;

                foreach (object value in values)
                {
                    sum = sum + (int)value;
                }

                List<object> result = new List<object>() { sum };

                return result;
            }
            else
            { throw new ArgumentException("The inputs must be the same as described in the input hints."); }
        }
        private bool CheckIfAllowedValues(IEnumerable<object> values)
        {
            var array = values.ToArray();
            var inputHintsArray = this.InputHints.ToArray();

            if (array.Length != this.InputHints.Count())
            {
                return false;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].GetType().ToString() != inputHintsArray[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }
}

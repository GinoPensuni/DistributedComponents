using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int16ToStringComponent
{
    public class Int16ToString
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        public Int16ToString()
        {
            this.componentGuid = new Guid("4BBEB7AB-BFCF-47B6-87B4-CE082DB9D462");

            this.friendlyName = "Int16ToString";

            this.inputHints = new List<string>() { typeof(Int16).ToString() };

            this.outputHints = new List<string>() { typeof(String).ToString() };
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
            bool checkNumberOfValues = this.CheckIfAllowedValues(values);

                if (checkNumberOfValues)
                {
                    var array = values.ToArray();

                    string result = array[0].ToString();

                    List<object> converted = new List<object>() { result };

                    return converted;
                }
                else
                {
                    throw new ArgumentException("The number and type of inputs must be the same as described in the input hints!");
                }
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

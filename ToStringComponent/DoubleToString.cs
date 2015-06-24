using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;

namespace ToStringComponent
{
    public class DoubleToString : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private IEnumerable<string> inputDescriptions;

        private IEnumerable<string> outputDescriptions;

        public DoubleToString()
        {
            this.componentGuid = new Guid("F3C0DF08-BECA-4B88-8083-B868D7E0C90D");

            this.friendlyName = "DoubleToString";

            this.inputHints = new List<string>() { typeof(Double).ToString() };

            this.outputHints = new List<string>() { typeof(String).ToString() };

            this.inputDescriptions = new List<string>() { "Parameter: A number representing a number of datatype double."  };
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


        public IEnumerable<string> InputDescriptions
        {
            get
            {
                return this.inputDescriptions;
            }
            set
            {
                this.inputDescriptions = value;
            }
        }

        public IEnumerable<string> OutputDescriptions
        {
            get
            {
                return this.outputDescriptions;
            }
            set
            {
                this.outputDescriptions = value;
            }
        }
    }
}

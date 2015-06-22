using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;

namespace MultiplyComponent
{
    public class Multiply : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        public Multiply()
        {
            this.componentGuid = new Guid("18E6E6EE-D725-4B39-AEE3-D2226A36A676");

            this.friendlyName = "Multiply";

            //List<string> inputhints = new List<string>() { "System.Double", "System.Double" };

            //List<string> outputHints = new List<string>() { "System.Double" };

            List<string> inputhints = new List<string>() { typeof(double).ToString(), typeof(double).ToString() };

            List<string> outputHints = new List<string>() { typeof(double).ToString() };

            this.inputHints = inputhints;

            this.outputHints = outputHints;

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
            var values2 = values.ToArray();

            bool checkValues = this.CheckIfAllowedValues(values);

            if (checkValues)
            {
                double product = (double)values2[0] * (double)values2[1];

                return new List<object>() { product };
            }
            else
            {
                throw new ArgumentException("The input values must be of the same type described in the input hints!");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;

namespace ToInt32
{
    public class DoubleToInt32 : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private IEnumerable<string> inputDescriptions;

        private IEnumerable<string> outputDescriptions;

        public DoubleToInt32()
        {
            this.componentGuid =  new Guid("80FC8BEC-E213-4DF2-8129-F70197CA4210");

            this.friendlyName = "DoubleToInt32";

            this.inputHints = new List<string>() { typeof(System.Double).ToString() };

            this.outputHints = new List<string>() { typeof(System.Int32).ToString() };

            this.inputDescriptions = new List<string>() { "Parameter: A number representing a number of datatype double." };

            this.outputDescriptions = new List<string>() { "Output: A number of the datatype Int32" };
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
            int integer = 0;

            bool checkValues = this.CheckIfAllowedValues(values);

            if (checkValues)
            {
                var array = values.ToArray();


                try
                {
                    integer = Convert.ToInt32(array[0]);

                    List<object> obj = new List<object>();

                    obj.Add(integer);

                    return obj;
                }
                catch
                {
                    throw new ArgumentException("The value must be of the type described in the input hints!");
                }
            }
            else
            {
                throw new ArgumentException("The number of values must be the same as described in the input hints!");
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

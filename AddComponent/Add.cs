﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddComponent
{
    public class Add : Core.Component.IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private IEnumerable<string> inputDescription;

        private IEnumerable<string> outputDescription;
        public Add()
        {
            this.componentGuid = new Guid("82870640-2A79-4FE5-8F34-6075C0C4863F");

            this.friendlyName = "Addition";

            List<string> inputhints = new List<string>() { typeof(Int32).ToString(), typeof(Int32).ToString() };
            this.inputHints = inputhints;

            List<string> outputhint = new List<string>() { typeof(Int32).ToString() };

            this.outputHints = outputhint;

            this.inputDescription = new List<string>() { "First parameter = number", "Second parameter = number" };

            this.outputDescription = new List<string> { "Number representing the sum of the two parameters" };
                                                          
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



        public IEnumerable<string> InputDescriptions
        {
            get
            {
                return this.inputDescription;
            }
            set
            {
                this.inputDescription = value;
            }
        }

        public IEnumerable<string> OutputDescriptions
        {
            get
            {
                return this.outputDescription;
            }
            set
            {
                this.outputDescription = value;
            }
        }
    }
}

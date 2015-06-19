﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonInterfaces;

namespace DivideComponent
{
    public class Divide : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        public Divide()
        {
            this.componentGuid = new Guid("EAC5C086-8745-4E2A-A931-41A176CBFC57");

            this.friendlyName = "Division";

            //this.inputHints = new List<string> { "System.Double", "System.Double" };

            //this.outputHints = new List<string> { "System.Double" };

            this.inputHints = new List<string> { typeof(double).ToString(), typeof(double).ToString() };

            this.outputHints = new List<string> { typeof(double).ToString() };
        }
        public Guid ComponentGuid
        {
            get { throw new NotImplementedException(); }
        }

        public string FriendlyName
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> InputHints
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> OutputHints
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<object> Evaluate(IEnumerable<object> values)
        {
            bool checkValues = this.CheckIfAllowedValues(values);

            if (checkValues)
            {
                var array = values.ToArray();

                List<object> result = new List<object>();

                double quotient = (double)array[0] / (double)array[1];

                result.Add(quotient);

                return result;
            }
            else
            {
                throw new ArgumentException("The input values must be of the same type as described in the input hints!");
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
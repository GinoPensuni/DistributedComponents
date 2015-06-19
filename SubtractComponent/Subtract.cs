﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonInterfaces;

namespace SubtractComponent
{
    public class Subtract : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        public Subtract()
        {
            this.componentGuid = new Guid("8129E40D-40D3-461E-86E6-A02AF74211F6");

            this.friendlyName = "Subtraction";

            //this.inputHints = new List<string>() { "System.Double, System.Double" };

            //this.outputHints = new List<string>() { "System.Double" };

            this.inputHints = new List<string>() { typeof(double).ToString(), typeof(double).ToString() };

            this.outputHints = new List<string>() { typeof(double).ToString() };
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
            bool checkValues = this.CheckIfAllowedValues(values);

            double difference = 0;

            if(checkValues)
            {
                List<object> result = new List<object>();

                foreach (object value in values)
                {
                    if (difference == 0)
                    {
                        difference -= (double)value;
                    }
                    else
                    {
                        difference = difference - (double)value;
                    }
                }

                result.Add(difference);

                return result;
            }
            else
            {
                throw new ArgumentException("The input must be of the same type as described in the input hints!");
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
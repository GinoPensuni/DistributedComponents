﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddTwoMatricesComponent
{
    public class AddTwoMatrices
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        public AddTwoMatrices()
        {
            this.componentGuid = new Guid("99F3B8AC-EEAB-4587-B55C-57B0C0D75B5E");

            this.friendlyName = "Add Matrices";

            this.inputHints = new List<string>() { typeof(int[,]).ToString(), typeof(int[,]).ToString() };

            this.outputHints = new List<string>() { typeof(int[,]).ToString() };
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
            
            if (CheckIfAllowedValues(values))
            {
                List<int[,]> matrices = values.Cast<int[,]>().ToList();

                Matrix first = new Matrix(matrices[0]);

                Matrix second = new Matrix(matrices[1]);

                 Matrix result = Matrix.AddMatrices(first, second);

                 return new List<object>() { result };
            }
            else
            {
                throw new ArgumentException("The number and type of inputs must be the same as described in the input hints!");
            }

        }


        private bool CheckIfAllowedValues(IEnumerable<object> values)
        {
            var array = values.ToArray();
            var inputHints = this.inputHints.ToArray();

            if (array.Length != this.InputHints.Count())
            {
                return false;
            }
            else
            {
                if (array[0].GetType().ToString() == typeof(int[,]).ToString() && array[1].GetType().ToString() == typeof(int[,]).ToString())
                {
                    return true;
                }

                return false;
            }
        }
    }
}

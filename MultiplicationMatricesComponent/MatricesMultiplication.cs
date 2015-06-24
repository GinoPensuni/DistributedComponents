using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;

namespace MultiplicationMatricesComponent
{
    public class MatricesMultiplication : IComponent
    {
         private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private IEnumerable<string> inputDescriptions;

        private IEnumerable<string> outputDescriptions;

        public MatricesMultiplication()
        {
            this.componentGuid = new Guid("60E37598-2570-4DBA-83F2-30096D45212E");

            this.friendlyName = "Multiply Matrices";

            this.inputHints= new List<string>() { typeof(int[,]).ToString(), typeof(int[,]).ToString() };

            this.outputHints = new List<string>() { typeof(int[,]).ToString() };

            this.inputDescriptions = new List<string>() { "First parameter: A matrix as 2 dimensional integer array", "Second parameter: A matrix as 2 dimensional integer array" };
            this.outputDescriptions = new List<string>() { "Output: The two input matrices multiplied." };                          
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
            if (CheckIfAllowedValues(values))
            {
                List<int[,]> matrices = values.Cast<int[,]>().ToList();

                Matrix first = new Matrix(matrices[0]);

                Matrix second = new Matrix(matrices[1]);

                Matrix result = Matrix.Multiply(first, second);

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

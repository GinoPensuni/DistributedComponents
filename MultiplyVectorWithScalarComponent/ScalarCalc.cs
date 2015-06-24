using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;

namespace MultiplyVectorWithScalarComponent
{
    class ScalarCalc : IComponent
    {
         private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        public ScalarCalc()
        {
            this.componentGuid = new Guid("414B1977-70D8-4275-88AC-E722DBD416A9");

            this.friendlyName = "Multiply vector with scalar";

            this.inputHints = new List<string>() { typeof(int[]).ToString(), typeof(int).ToString() };
            
            this.outputHints = new List<string>() { typeof(int[]).ToString() };    
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
           if(this.CheckIfAllowedValues(values))
           {
               var array = values.ToArray();

               Vector vector = new Vector((int[])array[0]);

               int scalar = (int)array[1];

               Vector result = Vector.MultiplyWithScalar(vector, scalar);

               return new List<object>() { result._Vector };
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
                if (array[0].GetType().ToString() == typeof(int[]).ToString() && array[1].GetType().ToString() == typeof(int).ToString())
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<string> OutputDescriptions
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

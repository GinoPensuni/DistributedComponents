using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;

namespace VectorAngleCalculator
{
    public class AngleCalculator : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private IEnumerable<string> inputDescriptions;

        private IEnumerable<string> outputDescriptions;

        public AngleCalculator()
        {
            this.componentGuid = new Guid("1417062F-4D3E-472F-97B7-F7E260F4BAA4");

            this.friendlyName = "Calculate Angle of two vectors";

            this.inputHints = new List<string>() { typeof(int[]).ToString(), typeof(int[]).ToString() };
            
            this.outputHints = new List<string>() { typeof(double).ToString() };    
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
               List<int[]> vectors = values.Cast<int[]>().ToList();

               Vector first = new Vector(vectors[0]);

               Vector second = new Vector(vectors[1]);

               double rad = Vector.CalcAngle(first, second);

               double degree = rad * (180 / Math.PI);

               return new List<object>() { degree };
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
                if (array[0].GetType().ToString() == typeof(int[]).ToString() && array[1].GetType().ToString() == typeof(int[]).ToString())
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

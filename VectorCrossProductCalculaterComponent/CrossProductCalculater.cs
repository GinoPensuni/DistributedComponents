using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;

namespace VectorCrossProductCalculaterComponent
{
    public class CrossProductCalculater : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private IEnumerable<string> inputDescriptions;

        private IEnumerable<string> outputDescriptions;

        public CrossProductCalculater()
        {
            this.componentGuid = new Guid("CC64D913-FC9E-4419-920F-B03CA6FF0142");

            this.friendlyName = "Cross Product Calculater";

            this.inputHints = new List<string>() { typeof(int[]).ToString(), typeof(int[]).ToString() };

            this.outputHints = new List<string>() { typeof(int[]).ToString() };

            this.inputDescriptions = new List<string>() { "An integer array, which represents the first vector.", "An integer array, which represents the second vector." };

            this.outputDescriptions = new List<string>() { "An integer array, which represents the cross product of the two vectors." };
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

               Vector result = Vector.CrossProduct(first, second);

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

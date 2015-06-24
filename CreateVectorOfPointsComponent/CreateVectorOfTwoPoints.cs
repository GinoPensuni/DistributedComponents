using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;

namespace CreateVectorOfPointsComponent
{
    class CreateVectorOfTwoPoints : IComponent
    {
         private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        public CreateVectorOfTwoPoints()
        {
            this.componentGuid = new Guid("1478A605-8261-4E19-A178-D6A1BD4606C0");

            this.friendlyName = "Create Vector of Points";

            this.inputHints = new List<string>() { typeof(int[]).ToString(), typeof(int[]).ToString() };
            
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
               List<int[]> points = values.Cast<int[]>().ToList();

               Point p1 = new Point(points[0]);

               Point p2 = new Point(points[1]);

               Vector result = Vector.CreateVector(p1, p2);

               return new List<object>() { result.Vector };
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
    }
}

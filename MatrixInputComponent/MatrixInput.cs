using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixInputComponent
{
    public class MatrixInput
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        public MatrixInput()
        {
            this.componentGuid = new Guid("5B37BFAB-413C-4F02-91E7-E34B982CA059");

            this.friendlyName = "Matrix Input";

            
            List<string> inputhints = new List<string>() { typeof(Int32).ToString(), typeof(Int32).ToString() };
            this.inputHints = inputhints;

            

            List<string> outputhint = new List<string>() { typeof(Int32).ToString() };

            this.outputHints = outputhint;
                                                          
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
            
        }
    }
}

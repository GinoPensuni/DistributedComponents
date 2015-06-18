using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonInterfaces;

namespace ToInt32
{
    public class ToInt32 : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        public ToInt32()
        {
            this.componentGuid =  new Guid("80FC8BEC-E213-4DF2-8129-F70197CA4210");

            this.friendlyName = "DoubleToInt32";

            
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
            throw new NotImplementedException();
        }
    }
}

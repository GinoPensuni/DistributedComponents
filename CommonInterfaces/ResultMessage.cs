using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    [Serializable]
    public class ResultMessage : Message
    {
        private ResultStatusCode status;

        public ResultMessage(ResultStatusCode status, MessageType type) : base(type)
        {
            this.status = status;
        }
        public ResultMessage(ResultStatusCode status) : this(status, MessageType.Unknown)
        {
        }

        public IEnumerable<object> Result
        {
            get;
            set;
        }

        public ResultStatusCode Status
        {
            get { return this.status; }
        }
    }
}

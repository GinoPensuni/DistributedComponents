using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    public class CommonClient
    {
        protected Guid clientGuid;

        protected double load;

        public Guid ClientGuid
        {
            get;
            set;
        }

        public double Load
        {
            get;
            set;
        }
    }
}

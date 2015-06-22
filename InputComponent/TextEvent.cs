using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InputComponent
{
    public class TextEvent :EventArgs
    {

        public string Message
        {
            get;
            set;
        }
    }
}

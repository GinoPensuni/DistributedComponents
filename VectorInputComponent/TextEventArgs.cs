﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorInputComponent
{
    public class TextEventArgs
    {
        public string Message
        {
            get;
            set;
        }

        public bool Valid
        {
            get;
            set;
        }
    }
}

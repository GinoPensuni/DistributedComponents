using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace GuiClientWPF
{
    public class LineContainer
    {
        public Polyline InnerLine { get; set; }

        public static implicit operator LineContainer(Polyline innerLine) 
        {
            return new LineContainer() { InnerLine = innerLine };
        }

        public static implicit operator Polyline(LineContainer lineContainer)
        {
            return lineContainer.InnerLine;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace GuiClientWPF
{
    public class ConnectionNodeClickedEventArgs
    {
        public readonly InputNodeComponent ClickedComponent;
        public readonly Ellipse ClickedEllipse;

        public ConnectionNodeClickedEventArgs(InputNodeComponent inputNodeComponent, Ellipse clickedEllipse)
        {
            this.ClickedComponent = inputNodeComponent;
            this.ClickedEllipse = clickedEllipse;
        }
    }
}

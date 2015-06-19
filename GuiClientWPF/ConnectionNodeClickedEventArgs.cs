using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace GuiClientWPF
{
    public class ConnectionNodeClickedEventArgs
    {
        public readonly InputNodeComponent ClickedComponent;
        public readonly Ellipse ClickedEllipse;
        public Point? Offset;

        public ConnectionNodeClickedEventArgs(InputNodeComponent inputNodeComponent, Ellipse clickedEllipse, Point? offset)
        {
            this.ClickedComponent = inputNodeComponent;
            this.ClickedEllipse = clickedEllipse;
            this.Offset = offset;
        }
    }
}

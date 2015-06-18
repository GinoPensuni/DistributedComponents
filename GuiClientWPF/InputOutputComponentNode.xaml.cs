using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GuiClientWPF
{
    /// <summary>
    /// Interaction logic for InputNodeComponent.xaml
    /// </summary>
    public partial class InputNodeComponent : UserControl
    {
        public event EventHandler<ConnectionNodeClickedEventArgs> ConnectionNodeClicked;

        public InputNodeComponent(string fullTypeName, Direction drawDirection)
        {
            InitializeComponent();

            switch (drawDirection)
            {
                case Direction.Left:
                    Grid.SetColumn(this.TypeLabel, 1);
                    Grid.SetColumn(this.Marker, 0);
                    break;
                case Direction.Right:
                    break;
            }

            this.TypeLabel.Text = fullTypeName;
            this.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            this.Arrange(new Rect(0, 0, this.DesiredSize.Width, this.DesiredSize.Height));
        }

        private void Marker_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.ConnectionNodeClicked != null)
            {
                this.ConnectionNodeClicked(this, new ConnectionNodeClickedEventArgs(this, (Ellipse)sender));
            }
        }
    }
}

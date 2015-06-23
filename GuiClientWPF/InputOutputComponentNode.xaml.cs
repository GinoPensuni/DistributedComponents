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
        private string fullTypeName;
        public string Hint
        {
            get
            {
                return this.fullTypeName;
            }
        }

        public InputNodeComponent(string fullTypeName)
        {
            InitializeComponent();
            this.fullTypeName = fullTypeName;
            this.Marker.ToolTip = fullTypeName;
        }

        private void Marker_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.ConnectionNodeClicked != null)
            {
                this.ConnectionNodeClicked(this, new ConnectionNodeClickedEventArgs(this, (Ellipse)sender, null));
            }
        }
    }
}

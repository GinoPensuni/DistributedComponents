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
    /// Interaction logic for GuiComponent.xaml
    /// </summary>
    public partial class GuiComponent : UserControl
    {
        private Components entry;

        public GuiComponent()
        {
            InitializeComponent();
        }

        public GuiComponent(Components entry)
            :this()
        {
            this.entry = entry;
            this.FriendlyName.Text = entry.FriendlyName;

            double currentY = 0.0;
            double largestExcessWidth = 10.0;

            foreach (var inputNode in entry.InputHints)
            {
                var newInputNode = new InputNodeComponent(inputNode);
                this.ComponentCanvas.Children.Add(newInputNode);
                Canvas.SetTop(newInputNode, currentY);
                Canvas.SetLeft(newInputNode, -1 * newInputNode.ActualWidth);
                newInputNode.Margin = new Thickness(3);
                currentY += newInputNode.ActualHeight + 3;
                largestExcessWidth = newInputNode.ActualWidth > largestExcessWidth ? newInputNode.ActualWidth : largestExcessWidth;
            }

            this.Width += largestExcessWidth * 2;
        }
    }
}

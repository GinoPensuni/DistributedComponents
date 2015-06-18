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
        public event EventHandler<ConnectionNodeClickedEventArgs> InputOutputNodeClicked;

        private readonly List<InputNodeComponent> inputNodes;
        private readonly List<InputNodeComponent> outputNodes;
        private Components entry;

        public GuiComponent()
        {
            InitializeComponent();
            this.inputNodes = new List<InputNodeComponent>();
            this.outputNodes = new List<InputNodeComponent>();
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
                var newInputNode = new InputNodeComponent(inputNode, Direction.Left);
                this.ComponentCanvas.Children.Add(newInputNode);
                Canvas.SetTop(newInputNode, currentY);
                Canvas.SetLeft(newInputNode, -1 * newInputNode.ActualWidth - 5);
                //newInputNode.Margin = new Thickness(3);
                currentY += newInputNode.ActualHeight + 3;
                largestExcessWidth = newInputNode.ActualWidth > largestExcessWidth ? newInputNode.ActualWidth : largestExcessWidth;
                newInputNode.ConnectionNodeClicked += InputOutputNode_ConnectionNodeClicked;
                this.inputNodes.Add(newInputNode);
            }

            currentY = 0.0;

            foreach (var outputNode in entry.OutputHints)
            {
                var newOutputNode = new InputNodeComponent(outputNode, Direction.Right);
                this.ComponentCanvas.Children.Add(newOutputNode);
                Canvas.SetTop(newOutputNode, currentY);
                Canvas.SetLeft(newOutputNode, this.Width + 5);
                //newOutputNode.Margin = new Thickness(3);
                currentY += newOutputNode.ActualHeight + 3;
                largestExcessWidth = newOutputNode.ActualWidth > largestExcessWidth ? newOutputNode.ActualWidth : largestExcessWidth;
                newOutputNode.ConnectionNodeClicked += InputOutputNode_ConnectionNodeClicked;
                this.outputNodes.Add(newOutputNode);
            }

            this.Width += largestExcessWidth * 2;
        }

        private void InputOutputNode_ConnectionNodeClicked(object sender, ConnectionNodeClickedEventArgs e)
        {
            if (this.InputOutputNodeClicked != null)
            {
                this.InputOutputNodeClicked(this, e);
            }
        }
    }
}

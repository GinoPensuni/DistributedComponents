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
        private List<InputNodeComponent> freeInputNodes;
        private List<InputNodeComponent> freeOutputNodes;
        private Guid id;
        public Guid Id
        {
            get
            {
                return id;
            }
        }
        public int FreeInputNodes
        {
            get;
            set;
        }

        public int FreeOutputNodes
        {
            get;
            set;
        }

        public Components Entry
        {
            get
            {
                return entry;
            }
        }

        public List<InputNodeComponent> InputNodesList
        {
            get
            {
                return inputNodes;
            }
        }

        public List<InputNodeComponent> OutputNodesList
        {
            get
            {
                return outputNodes;
            }
        }

        public List<InputNodeComponent> FreeInputNodesList
        {
            get
            {
                return freeInputNodes;
            }
        }

        public List<InputNodeComponent> FreeOutputNodesList
        {
            get
            {
                return freeOutputNodes;
            }
        }

        public GuiComponent()
        {
            InitializeComponent();
            this.inputNodes = new List<InputNodeComponent>();
            this.outputNodes = new List<InputNodeComponent>();
            this.freeInputNodes = new List<InputNodeComponent>();
            this.freeOutputNodes = new List<InputNodeComponent>();
            this.id = Guid.NewGuid();
        }

        public GuiComponent(Components entry)
            :this()
        {
            this.entry = entry;
            this.FriendlyName.Text = entry.FriendlyName;

            double currentY = 0.0;

            foreach (var inputNode in entry.InputHints)
            {
                var newInputNode = new InputNodeComponent(inputNode);
                this.InputCanvas.Children.Add(newInputNode);
                Canvas.SetTop(newInputNode, currentY);
                Canvas.SetLeft(newInputNode, 0);
                //newInputNode.Margin = new Thickness(3);
                currentY += 15;
                newInputNode.ConnectionNodeClicked += InputOutputNode_ConnectionNodeClicked;
                this.inputNodes.Add(newInputNode);
                this.FreeInputNodes++;
            }

            currentY = 0.0;

            foreach (var outputNode in entry.OutputHints)
            {
                var newOutputNode = new InputNodeComponent(outputNode);
                this.OutputCanvas.Children.Add(newOutputNode);
                Canvas.SetTop(newOutputNode, currentY);
                Canvas.SetLeft(newOutputNode, 0);
                //newOutputNode.Margin = new Thickness(3);
                currentY += 15;
                newOutputNode.ConnectionNodeClicked += InputOutputNode_ConnectionNodeClicked;
                this.outputNodes.Add(newOutputNode);
                this.FreeOutputNodes ++;
            }

            freeOutputNodes.AddRange(this.outputNodes);
            freeInputNodes.AddRange(this.inputNodes);
        }

        private void InputOutputNode_ConnectionNodeClicked(object sender, ConnectionNodeClickedEventArgs e)
        {
            double x;

            if (this.InputCanvas.Children.Contains((UIElement)sender))
            {
                x = 5.0 + 0;
            }
            else
            {
                x = 5.0 + 90;
            }

            double y = 5.0 + Canvas.GetTop((UIElement)sender);

            if (this.InputOutputNodeClicked != null)
            {
                e.Offset = new Point(x, y);
                this.InputOutputNodeClicked(this, e);
            }
        }
    }
}

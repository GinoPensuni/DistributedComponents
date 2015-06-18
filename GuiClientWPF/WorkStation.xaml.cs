using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// Interaction logic for WorkStation.xaml
    /// </summary>
    public partial class WorkStation : UserControl
    {
        private UIElement draggedObject = null;
        private GuiComponent selectedComponent = null;
        private Point? mouseOffset = null;
        private List<Tuple<GuiComponent, GuiComponent, Line>> connections = new List<Tuple<GuiComponent, GuiComponent, Line>>();


        private ClientManager Manager
        {
            get
            {
                return ClientManager.Manager;
            }
        }
        public WorkStation()
        {
            InitializeComponent();
            Manager.CanvasComponents.CollectionChanged += CathegoryCollection_CollectionChanged;
        }

        void CathegoryCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                     this.CreateComponents(e.NewItems.Cast<Components>());
                     break;
                default:
                    break;
            }
        }

        private void CreateComponents(IEnumerable<Components> list)
        {
            foreach (var entry in list)
            {
                var comp = new GuiComponent(entry);
                this.ComponentBuilder.MouseMove += comp_MouseMove;
                comp.MouseLeftButtonDown += comp_MouseLeftButtonDown;
                comp.MouseLeftButtonUp += comp_MouseLeftButtonUp;
                comp.MouseRightButtonDown += comp_MouseRightButtonDown;
                this.ComponentBuilder.Children.Add(comp);

            }
        }

        void comp_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.selectedComponent == null)
            {
                this.selectedComponent = sender as GuiComponent;
                this.selectedComponent.Background = Brushes.Red;
            }
            else if (this.selectedComponent == sender)
            {
                this.selectedComponent.Background = Brushes.White;
                this.selectedComponent = null;
            }
            else
            {
                var secondSelectedComponent = sender as GuiComponent;
                if(sender == null) return;
                var line = new Line();
                this.connections.Add(new Tuple<GuiComponent, GuiComponent, Line>(this.selectedComponent, secondSelectedComponent, line));
                line.Stroke = Brushes.Violet;
                line.StrokeThickness = 3;
                line.StrokeDashArray = new DoubleCollection() { 1.0, 2.0 };


                line.X1 = double.IsNaN(Canvas.GetLeft(this.selectedComponent)) ? 1 : Canvas.GetLeft(this.selectedComponent);
                line.Y1 = double.IsNaN(Canvas.GetTop(this.selectedComponent)) ? 1 : Canvas.GetTop(this.selectedComponent);
                line.X2 = double.IsNaN(Canvas.GetLeft(secondSelectedComponent)) ? 1 : Canvas.GetLeft(secondSelectedComponent);
                line.Y2 = double.IsNaN(Canvas.GetTop(secondSelectedComponent)) ? 1 : Canvas.GetTop(secondSelectedComponent);
                Canvas.SetZIndex(line, -100);
                this.ComponentBuilder.Children.Add(line);
                this.selectedComponent.Background = Brushes.White;
                this.selectedComponent = null;
            }
        }

        void comp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.draggedObject = null;
            this.mouseOffset = null;
        }

        void comp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (draggedObject != null)
            {
                return;
            }

            this.draggedObject = sender as UIElement;
            this.mouseOffset = e.GetPosition(this.draggedObject);
        }

        void comp_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.draggedObject == null)
            {
                return;
            }

            var canvas = this.draggedObject as GuiComponent;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var position = e.GetPosition(this.ComponentBuilder);
                Canvas.SetLeft(canvas, position.X - this.mouseOffset.Value.X);
                Canvas.SetTop(canvas, position.Y - this.mouseOffset.Value.Y);
                foreach (var entry in this.connections.Where(tuple => tuple.Item1.Equals(canvas) || tuple.Item2.Equals(canvas)))
                {
                    if (canvas == entry.Item1)
                    {
                        entry.Item3.X1 = Canvas.GetLeft(canvas);
                        entry.Item3.Y1 = Canvas.GetTop(canvas);
                    }
                    else if (canvas == entry.Item2)
                    {
                        entry.Item3.X2 = Canvas.GetLeft(canvas);
                        entry.Item3.Y2 = Canvas.GetTop(canvas);
                    }
                }
            }
        }
    }
}

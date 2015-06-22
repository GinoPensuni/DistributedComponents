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
        private Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>  selectedComponent = null;
        private Point? mouseOffset = null;
        private List<Tuple<Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, LineContainer>> connections =
            new List<Tuple<Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, LineContainer>>();


        public IReadOnlyList<Tuple<Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, LineContainer>> Connections
        {
            get
            {
                return this.connections;
            }
        }

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
                //comp.MouseRightButtonDown += comp_MouseRightButtonDown;
                comp.InputOutputNodeClicked += comp_InputOutputNodeClicked;
                this.ComponentBuilder.Children.Add(comp);
            }
        }

        private void comp_InputOutputNodeClicked(object sender, ConnectionNodeClickedEventArgs e)
        {
            var sendingComponent = sender as GuiComponent;

            if (sendingComponent == null)
            {
                return;
            }
            
            if (this.selectedComponent == null)
            {
                this.selectedComponent = new Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>(sendingComponent, e.ClickedComponent, e.ClickedEllipse, e.Offset.Value);
                //this.selectedComponent.Background = Brushes.Red;
            }
            else if (this.selectedComponent.Item3 == e.ClickedEllipse)
            {
                //this.selectedComponent.Background = Brushes.White;
                this.selectedComponent = null;
            }
            else
            {
                var secondSelectedComponent = new Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>(sendingComponent, e.ClickedComponent, e.ClickedEllipse, e.Offset.Value);
                //if (sender == null) return;

                //Point p1 = new Point(
                //    double.IsNaN(Canvas.GetLeft(this.selectedComponent)) ? 1 : Canvas.GetLeft(this.selectedComponent),
                //    double.IsNaN(Canvas.GetTop(this.selectedComponent)) ? 1 : Canvas.GetTop(this.selectedComponent));
                //Point p2 = new Point(
                //    double.IsNaN(Canvas.GetLeft(secondSelectedComponent)) ? 1 : Canvas.GetLeft(secondSelectedComponent),
                //    double.IsNaN(Canvas.GetTop(secondSelectedComponent)) ? 1 : Canvas.GetTop(secondSelectedComponent));

                double lefti = Canvas.GetLeft(this.selectedComponent.Item1);
                double leftii = Canvas.GetLeft(secondSelectedComponent.Item1);
                double topi = Canvas.GetTop(this.selectedComponent.Item1);
                double topii = Canvas.GetTop(secondSelectedComponent.Item1);

                double x1 = (double.IsNaN(lefti) ? 1 : lefti)  + this.selectedComponent.Item4.X;
                double y1 = (double.IsNaN(topi) ? 1 : topi) + this.selectedComponent.Item4.Y;
                double x2 = (double.IsNaN(leftii) ? 1 : leftii) + secondSelectedComponent.Item4.X;
                double y2 = (double.IsNaN(topii) ? 1 : topii) + secondSelectedComponent.Item4.Y;

                Point p1 = new Point(
                    double.IsNaN(x1) ? 1 : x1,
                    double.IsNaN(y1) ? 1 : y1);
                Point p2 = new Point(
                    double.IsNaN(x2) ? 1 : x2,
                    double.IsNaN(y2) ? 1 : y2);

                Polyline line;

                // TODO Don't hardcode this shit

                if (this.selectedComponent.Item4.X > secondSelectedComponent.Item4.X)
                {
                    line = this.DrawPolyLine(p1, p2, 90);
                    this.connections.Add(new Tuple<Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, LineContainer>
                    (this.selectedComponent, secondSelectedComponent, line));
                }
                else
                {
                    line = this.DrawPolyLine(p2, p1, 90);

                    this.connections.Add(new Tuple<Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, LineContainer>
                    (secondSelectedComponent,this.selectedComponent, line));
                }

               
                Canvas.SetZIndex(line, -100);
                this.ComponentBuilder.Children.Add(line);
                //this.selectedComponent.Background = Brushes.White;
                this.selectedComponent = null;
            }
        }

        //void comp_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (this.selectedComponent == null)
        //    {
        //        this.selectedComponent = sender as GuiComponent;
        //        this.selectedComponent.Background = Brushes.Red;
        //    }
        //    else if (this.selectedComponent == sender)
        //    {
        //        this.selectedComponent.Background = Brushes.White;
        //        this.selectedComponent = null;
        //    }
        //    else
        //    {
        //        var secondSelectedComponent = sender as GuiComponent;
        //        if(sender == null) return;


        //        line.X1 = double.IsNaN(Canvas.GetLeft(this.selectedComponent)) ? 1 : Canvas.GetLeft(this.selectedComponent);
        //        line.Y1 = double.IsNaN(Canvas.GetTop(this.selectedComponent)) ? 1 : Canvas.GetTop(this.selectedComponent);
        //        line.X2 = double.IsNaN(Canvas.GetLeft(secondSelectedComponent)) ? 1 : Canvas.GetLeft(secondSelectedComponent);
        //        line.Y2 = double.IsNaN(Canvas.GetTop(secondSelectedComponent)) ? 1 : Canvas.GetTop(secondSelectedComponent);

        //        Point p1 = new Point(
        //            double.IsNaN(Canvas.GetLeft(this.selectedComponent)) ? 1 : Canvas.GetLeft(this.selectedComponent),
        //            double.IsNaN(Canvas.GetTop(this.selectedComponent)) ? 1 : Canvas.GetTop(this.selectedComponent));
        //        Point p2 = new Point(
        //            double.IsNaN(Canvas.GetLeft(secondSelectedComponent)) ? 1 : Canvas.GetLeft(secondSelectedComponent),
        //            double.IsNaN(Canvas.GetTop(secondSelectedComponent)) ? 1 : Canvas.GetTop(secondSelectedComponent));

        //        var line = this.DrawPolyLine(p1, p2);
        //        this.connections.Add(new Tuple<GuiComponent, GuiComponent, LineContainer>(this.selectedComponent, secondSelectedComponent, line));
        //        Canvas.SetZIndex(line, -100);
        //        this.ComponentBuilder.Children.Add(line);
        //        this.selectedComponent.Background = Brushes.White;
        //        this.selectedComponent = null;
        //    }
        //}

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
                foreach (var entry in this.connections.Where(tuple => tuple.Item1.Item1.Equals(canvas) || tuple.Item2.Item1.Equals(canvas)))
                {
                    this.ComponentBuilder.Children.Remove(entry.Item3);

                    double lefti = Canvas.GetLeft(entry.Item1.Item1);
                    double leftii = Canvas.GetLeft(entry.Item2.Item1);
                    double topi = Canvas.GetTop(entry.Item1.Item1);
                    double topii = Canvas.GetTop(entry.Item2.Item1);

                    double x1 = (double.IsNaN(lefti) ? 1 : lefti) + entry.Item1.Item4.X;
                    double y1 = (double.IsNaN(topi) ? 1 : topi) + entry.Item1.Item4.Y;
                    double x2 = (double.IsNaN(leftii) ? 1 : leftii) + entry.Item2.Item4.X;
                    double y2 = (double.IsNaN(topii) ? 1 : topii) + entry.Item2.Item4.Y;

                    Point p1 = new Point(
                        double.IsNaN(x1) ? 1 : x1,
                        double.IsNaN(y1) ? 1 : y1);
                    Point p2 = new Point(
                        double.IsNaN(x2) ? 1 : x2,
                        double.IsNaN(y2) ? 1 : y2);

                    Polyline line;
                    
                    if (entry.Item1.Item4.X > entry.Item2.Item4.X)
                    {
                        line = this.DrawPolyLine(p1, p2, 90);
                    }
                    else
                    {
                        line = this.DrawPolyLine(p2, p1, 90);
                    }

                    entry.Item3.InnerLine = line;
                    Canvas.SetZIndex(line, -100);
                    this.ComponentBuilder.Children.Add(line);
                }
            }
        }


        Polyline DrawPolyLine(Point input, Point output, double  yComponentSize = 70)
        {
            var complex = true;
            var deltaX = input.X - output.X;
            var deltaY = input.Y - output.Y;
            var p1 = new Point();
            var p2 = new Point();
            var p3 = new Point();
            var p4 = new Point();


            if (deltaX >= 0 && deltaY >= 0 )
            {
                p1.X = output.X - deltaX / 4;
                p1.Y = output.Y;
                p2.X = output.X - deltaX / 4;
                p2.Y = output.Y + yComponentSize;
                p3.X = input.X + deltaX / 4;
                p3.Y = input.Y - yComponentSize;
                p4.X = input.X + deltaX / 4;
                p4.Y = input.Y;
            }

            else if (deltaX >= 0 && deltaY < 0)
            {
                p1.X = output.X - deltaX / 4;
                p1.Y = output.Y;
                p2.X = output.X - deltaX / 4;
                p2.Y = output.Y - yComponentSize;
                p3.X = input.X + deltaX / 4;
                p3.Y = input.Y + yComponentSize;
                p4.X = input.X + deltaX / 4;
                p4.Y = input.Y;
            }

            else if (deltaX < 0 && deltaY >= 0)
            {

                p1.X = output.X + deltaX / 4;
                p1.Y = output.Y;
                p2.X = input.X - deltaX / 4;
                p2.Y = input.Y;
                complex = false;
            }

            else if (deltaX < 0 && deltaY < 0)
            {

                p1.X = output.X + deltaX / 4;
                p1.Y = output.Y;
                p2.X = input.X - deltaX / 4;
                p2.Y = input.Y;
                complex = false;

                //p1.X = output.X - deltaX / 4;
                //p1.Y = output.Y;
                //p2.X = output.X - deltaX / 4;
                //p2.Y = output.Y - yComponentSize;
                //p3.X = input.X + deltaX / 4;
                //p3.Y = input.Y +  yComponentSize;
                //p4.X = input.X + deltaX / 4;
                //p4.Y = input.Y;
            }

            var polyline = new Polyline();

            if (!complex)
            {
                polyline.Points.Add(output);
                polyline.Points.Add(p1);
                polyline.Points.Add(p2);
                polyline.Points.Add(input);
            }

            else
            {
                polyline.Points.Add(output);
                polyline.Points.Add(p1);
                polyline.Points.Add(p2);
                polyline.Points.Add(p3);
                polyline.Points.Add(p4);
                polyline.Points.Add(input);
            }

            polyline.Margin = new Thickness(3);
            polyline.Stroke = Brushes.Black;
            polyline.StrokeThickness = 4;
            polyline.Visibility = Visibility.Visible;
            return polyline;
        }
    }
}

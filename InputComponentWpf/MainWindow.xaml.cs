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
using System.Windows.Threading;

namespace InputComponentWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event EventHandler<TextEventArgs> OnSubmitted;

        private static Dispatcher disp;

        public static Dispatcher Disp
        {
            get
            {
                return MainWindow.disp;
            }
            private set
            {
                MainWindow.disp = value;
            }
        }

        public MainWindow()          
        {
            InitializeComponent();
        }


        public MainWindow(string info = "no parameter")
            :this()
        {
            this.InputBox.Text = info;   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int result = 0;

            if (OnSubmitted != null)
            {
                try
                {
                    if (int.TryParse(InputBox.Text, out result))
                    {
                        OnSubmitted(this, new TextEventArgs() { Message = InputBox.Text });
                        this.Close();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void InputBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                int.Parse(InputBox.Text);
                InputBox.BorderBrush = Brushes.Green;
                InputBox.Foreground = Brushes.Green;
                InputBox.BorderThickness = new Thickness(1);
                btn.IsEnabled = true;
            }
            catch
            {
                InputBox.BorderBrush = Brushes.Red;
                InputBox.Foreground = Brushes.Red;
                InputBox.BorderThickness = new Thickness(1);
                btn.IsEnabled = false;
            }
        }
    }
}

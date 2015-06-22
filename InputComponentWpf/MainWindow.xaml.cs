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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OnSubmitted != null)
            {
                try
                {
                    OnSubmitted(this, new TextEventArgs() { Message = InputBox.Text });
                }
                catch (Exception)
                {
                }
            }
        }
    }
}

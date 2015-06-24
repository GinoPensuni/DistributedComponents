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
using System.Text.RegularExpressions;

namespace MatrixInputComponentWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InputBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.testRegEx(this.InputBox.Text))
            {
                this.SetColor(true);
                return;
            }
            else
                this.SetColor(false);
        }

        private bool testRegEx(string eval)
        {
            string query = "(\\[([0-9]+(,[0-9]+)*;)*([0-9]+(,[0-9]+)*\\]))";
            string query1 = "([[]])";
            if(Regex.IsMatch(eval.Trim(), query1))
                return true;
            
            return Regex.IsMatch(eval.Trim(), query);
        }

        private void SetColor(bool error)
        {
            if (error)
            {
                this.InputBox.BorderBrush = Brushes.Red;
                this.InputBox.Foreground = Brushes.Red;
                this.InputBox.BorderThickness = new Thickness(1);
                return;
            }

            this.InputBox.BorderBrush = Brushes.Green;
            this.InputBox.Foreground = Brushes.Green;
            this.InputBox.BorderThickness = new Thickness(1); 
        }


        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Click");
        }
    }
}

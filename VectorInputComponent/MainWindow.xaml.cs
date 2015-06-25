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

namespace VectorInputComponent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event EventHandler<TextEventArgs> OnSubmitted;
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string info = "no parameter")
            : this()
        {
            this.InputBox.Text = info;
        }
        private bool testRegEx(string eval)
        {
            string regex = @"(\[([0-9]*,)*[0-9]\])";
            string empty = @"\[\]";
            if (Regex.IsMatch(eval.Trim(), empty))
                return true;

            return Regex.IsMatch(eval.Trim(), regex);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OnSubmitted != null)
            {
                TextEventArgs args = new TextEventArgs() { Message = InputBox.Text };

                OnSubmitted(this, args);
                
                if (args.Valid)
                {
                    this.Close();
                }
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.testRegEx(this.InputBox.Text))
            {
                this.SetColor(true);
                return;
            }
            else
                this.SetColor(false);
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

    }
}

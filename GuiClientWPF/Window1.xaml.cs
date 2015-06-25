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
using System.Windows.Shapes;

namespace GuiClientWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public string IpAddress { get; set; }

        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string balls = @"(?:[0-9]{1,3}\.){3}[0-9]{1,3}";

            if (System.Text.RegularExpressions.Regex.IsMatch(IpBox.Text, balls))
            {
                this.IpAddress = IpBox.Text;
                this.DialogResult = true;
            }
        }
    }
}

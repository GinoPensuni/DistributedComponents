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

namespace InputComponent
{
    /// <summary>
    /// Interaction logic for InputWindow1.xaml
    /// </summary>
    public partial class InputWindow1 : Window
    {
        public event EventHandler<TextEvent> submit;
        public InputWindow1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (submit != null)
            {
                submit(this, new TextEvent() { Message = UserInputBox.Text });
            }
        }
    }
}

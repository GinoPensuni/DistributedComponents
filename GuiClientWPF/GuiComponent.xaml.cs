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
        private Components entry;

        public GuiComponent()
        {
            InitializeComponent();
        }

        public GuiComponent(Components entry)
            :this()
        {
            this.entry = entry;
            this.FriendlyName.Text = entry.FriendlyName;
        }
    }
}

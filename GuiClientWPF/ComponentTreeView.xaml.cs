using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TreeView.xaml
    /// </summary>
    public partial class ComponentTreeView : UserControl
    {
        
        public ClientManager Manager
        {
            get
            {
                return ClientManager.Manager;
            }
            set
            {

            }
        }

        public ComponentTreeView()
        {
            InitializeComponent();
            this.SimpleComponents.ItemsSource = this.Manager.SimpleComponents;
            this.ComplexComponents.ItemsSource = this.Manager.ComplexComponents;
            this.OtherComponents.ItemsSource = this.Manager.OtherComponents;
        }

        private void Component_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void Component_DragOver(object sender, DragEventArgs e)
        {

        }

        private void Component_Drop(object sender, DragEventArgs e)
        {

        }


    }
}

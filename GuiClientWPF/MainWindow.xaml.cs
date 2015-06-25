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
using GuiClientWPF.Assets.Windows;

namespace GuiClientWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private HelpWindowUI uihelper;
        private HelpWindowControl controlhelper;
        private ClientManager Manager { get { return ClientManager.Manager; } }

        public MainWindow()
        {

            InitializeComponent();
        }

        private void ExitAction_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private async void DisconnectAction_Click(object sender, RoutedEventArgs e)
        {
            await Manager.Disconnect();
        }

        private void ConnectAction_Click(object sender, RoutedEventArgs e)
        {
            Manager.ConnectAction();
        }

        private async void SaveAction_Click(object sender, RoutedEventArgs e)
        {
           await this.Manager.SaveComponent(this.WorkingSTATION.Connections, this.Dispatcher);
        }

        private void RunAction_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConnectionOptions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PreferencesOptions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HelpControls_Click(object sender, RoutedEventArgs e)
        {
            controlhelper = new HelpWindowControl();
            controlhelper.Visibility = Visibility.Visible;
        }

        private void HelpUI_Click(object sender, RoutedEventArgs e)
        {
            uihelper = new HelpWindowUI();
            uihelper.Visibility = Visibility.Visible;
        }

        private void InsertDebug_Click(object sender, RoutedEventArgs e)
        {
            this.Manager.FillTestDataAsync();
        }

        private void DeleteDebug_Click(object sender, RoutedEventArgs e)
        {
            this.Manager.CathegoryCollection[0].Components.Clear();
            this.Manager.CathegoryCollection[1].Components.Clear();
            this.Manager.CathegoryCollection[2].Components.Clear();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using CommonRessources;
using System.Threading;


namespace InputComponentWpf
{
    public class Input : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private List<object> integer;
        private bool finish;
        private MainWindow inputBox;

        public Input()
        {
            this.componentGuid = new Guid("AFF922C1-6EE1-4DD5-A8C8-8A3A8EA7563C");

            this.friendlyName = "Integer-Input";

            this.inputHints = new List<string>() { typeof(String).ToString() };

            this.outputHints = new List<string>() { typeof(Int32).ToString() };
        }
        public Guid ComponentGuid
        {
            get { return this.componentGuid; }
        }

        public string FriendlyName
        {
            get { return this.friendlyName; }
        }

        public IEnumerable<string> InputHints
        {
            get { return this.inputHints; }
        }

        public IEnumerable<string> OutputHints
        {
            get { return this.outputHints; }
        }

        public IEnumerable<object> Evaluate(IEnumerable<object> values)
        {
            var t = new Thread(new ThreadStart(Instantiate));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.IsBackground = true;    
            t.Join();
            return integer;
        }

        private void Instantiate()
        {
            inputBox = new MainWindow();
            inputBox.OnSubmitted += inputBox_OnSubmitted;
            System.Windows.Application application = new System.Windows.Application();
            application.Run(inputBox);      
        }


        void inputBox_OnSubmitted(object sender, TextEventArgs e)
        {
            integer = new List<object>();
            integer.Add(e.Message);
        }
    }
}

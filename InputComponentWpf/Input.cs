using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;


namespace InputComponentWpf
{
    public class Input : Core.Component.IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private IEnumerable<string> inputDescriptions;

        private IEnumerable<string> outputDescriptions;

        private List<object> integer;

        private MainWindow inputBox;

        public Input()
        {
            this.componentGuid = new Guid("AFF922C1-6EE1-4DD5-A8C8-8A3A8EA7563C");

            this.friendlyName = "Integer-Input";

            this.inputHints = new List<string>() { };

            this.outputHints = new List<string>() { typeof(Int32).ToString() };

            this.inputDescriptions = new List<string>() { /*"Parameter: A number typed in as string."*/ };
            this.outputDescriptions = new List<string>() { "Output: The input string converted to an integer." };
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
            var t = new Thread(new ParameterizedThreadStart(Instantiate));
            t.SetApartmentState(ApartmentState.STA);
            var x = values.Any() ? values.First() : "no Parameter";
            t.Start(x);
            t.IsBackground = true;    
            t.Join();
            return integer;
        }

        private void Instantiate(object obj)
        {
            string info = (string)obj;
            inputBox = new MainWindow(info);
            inputBox.OnSubmitted += inputBox_OnSubmitted;
            System.Windows.Application application = new System.Windows.Application();
            application.Run(inputBox);      
        }


        void inputBox_OnSubmitted(object sender, TextEventArgs e)
        {
            
            integer = new List<object>();
            integer.Add(int.Parse(e.Message)    );
        }


        public IEnumerable<string> InputDescriptions
        {
            get
            {
                return this.inputDescriptions;
            }
            set
            {
                this.inputDescriptions = value;
            }
        }

        public IEnumerable<string> OutputDescriptions
        {
            get
            {
                return this.outputDescriptions;
            }
            set
            {
                this.outputDescriptions = value;
            }
        }
    }
}

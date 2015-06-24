using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;
using System.Threading;

namespace StringInputComponent
{
    public class TextInput : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private List<object> text;

        private MainWindow textbox; 

        public TextInput()
        {
            this.componentGuid = new Guid("93B15EA3-9B3A-4568-8B58-3640F9D13978");

            this.friendlyName = "Enter Text";

            this.inputHints = new List<string>() { typeof(String).ToString() };

            this.outputHints = new List<string>() { typeof(String).ToString() };
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
            get {return this.inputHints; }
        }

        public IEnumerable<string> OutputHints
        {
            get { return this.outputHints; }
        }

        public IEnumerable<object> Evaluate(IEnumerable<object> values)
        {
            var thread = new Thread(new ThreadStart(Initialize));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.IsBackground = true;
            thread.Join();
            return this.text;
        }

        private void Initialize()
        {
            textbox = new MainWindow();
            textbox.OnSubmitted += textbox_OnSubmitted;
            System.Windows.Application application = new System.Windows.Application();
            application.Run(textbox);  

        }

        private void textbox_OnSubmitted(object sender, TextInputEventArgs e)
        {
            this.text = new List<object>();
            this.text.Add(e.Text);
        }


        public IEnumerable<string> InputDescriptions
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<string> OutputDescriptions
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

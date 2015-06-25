using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;
using System.Threading;

namespace ObjectOutputComponent
{
    class ObjectOutput : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private IEnumerable<string> inputDescriptions;

        private IEnumerable<string> outputDescriptions;

        private MainWindow outputBox;

        public ObjectOutput()
        {
            this.componentGuid = new Guid("6B09693B-9C75-44F5-9339-8A32597CFD9E");

            this.friendlyName = "Integer Output";

            this.inputHints = new List<string>() { typeof(System.Int32).ToString() };

            this.outputHints = new List<string>() { };

            this.inputDescriptions = new List<string> { "Parameter: An object to output" };

            this.OutputDescriptions = new List<string> { "Empty List of objects" };
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

        public IEnumerable<object> Evaluate(IEnumerable<object> values)
        {
            var test = values.ToArray();

            if (test.Length != 1 )
            {
                throw new ArgumentException("Only one element can be displayed, too much elements in the list!");
            }
            else
            {
            var t = new Thread(new ParameterizedThreadStart(Initialize));
            t.SetApartmentState(ApartmentState.STA);
            var x = values.Any() ? values.First() : "no parameter";
            t.Start(x);
            t.IsBackground = true;
            t.Join();
            return new List<object>();
            }
        }

        private void Initialize(object obj)
        {
            outputBox = new MainWindow(obj);

            System.Windows.Application app = new System.Windows.Application();

            app.Run(this.outputBox);
        }
    }
}

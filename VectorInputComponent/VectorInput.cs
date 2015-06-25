using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;
using System.Threading;
using System.Text.RegularExpressions;

namespace VectorInputComponent
{
    public class VectorInput : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private IEnumerable<string> inputDescriptions;

        private IEnumerable<string> outputDescriptions;

        private MainWindow inputBox;

        private List<object> _vector;

        public VectorInput()
        {
            this.componentGuid = new Guid("20553FDA-330B-4520-B866-C7041E2837EA");

            this.friendlyName = "Vector Input";

            this.inputHints = new List<string>() { typeof(string).ToString() };

            this.outputHints = new List<string>() { typeof(int[]).ToString() };

            this.inputDescriptions = new List<string> { "Parameter: string" };

            this.OutputDescriptions = new List<string> { "One dimensional integer [] representing a vector." };

            this._vector = new List<object>();
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
            var t = new Thread(new ParameterizedThreadStart(Instantiate));
            t.SetApartmentState(ApartmentState.STA);
            var x = values.Any() ? values.First() : "";
            t.Start(x);
            t.IsBackground = true;
            t.Join();
            return _vector;
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
            string toConvert = e.Message;

            if (!testRegEx(toConvert))
                return;

            toConvert = toConvert.Replace(" ", string.Empty);

            string[] splitted = toConvert.Split(new char[] { '[', ']' });

            if (splitted.Length == 3)
            {
                e.Valid = true;

                string[] splittedcomma = splitted[1].Split(new char[] { ',' });

                int[] vector = new int[splittedcomma.Length];

                for (int i = 0; i < splittedcomma.Length; i++)
                {
                    vector[i] = Convert.ToInt32(splittedcomma[i]);
                }

                _vector.Add(vector);
            }
        }
        private bool testRegEx(string eval)
        {
            string regex = @"(\[([0-9]*,)*[0-9]\])";
            string empty = @"\[\]";
            if (Regex.IsMatch(eval.Trim(), empty))
                return true;

            return Regex.IsMatch(eval.Trim(), regex);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;
using System.Runtime.InteropServices;

namespace InputComponent
{
    public class Input : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private IEnumerable<object> integer;
        private bool finish;

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
            return Eval().Result;
        }

        private async Task<IEnumerable<object>> Eval()
        {

            var inputBox = new InputWindow1();

            inputBox.Show();

            inputBox.submit += inputBox_submit;
            await Await();
            return this.integer;

        }

        void inputBox_submit(object sender, TextEvent e)
        {
            integer = new List<object>();
            integer.Concat(new List<object>() { int.Parse(e.Message) });
        }

        private Task Await() {
            var task = new Task(() => {
                while (!finish);

                return;
            });

            task.Start();
            return task;
        }
    }
}

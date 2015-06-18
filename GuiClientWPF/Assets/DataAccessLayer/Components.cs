using CommonInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GuiClientWPF
{
    public class Components : CommonInterfaces.IComponent, INotifyPropertyChanged
    {
        private IEnumerable<string> inputHints;
        private string name;
        private IEnumerable<string> outputHints;

        public Components()
        {
            this.UniqueID = Guid.NewGuid();
            this.inputHints = new List<string> { "System.Int32", "System.String" };
            this.outputHints = new List<string> { "System.String" };
        }

        public string FriendlyName
        {
            get 
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.NotifyPropertyChanged("FriendlyName");
            }
        }

        public Guid UniqueID
        {
            get;
            private set;
        }

        public IEnumerable<string> InputHints
        {
            get
            {
                return this.inputHints;
            }
            set
            {
                inputHints = value;
            }
        }

        public IEnumerable<string> OutputHints
        {
            get
            {
                return this.outputHints;
            }
            set
            {
                this.outputHints = value;
            }
        }

        public IEnumerable<object> Evaluate(IEnumerable<object> objects)
        {
            throw new NotImplementedException();
        }

        protected void NotifyPropertyChanged(string chage)
        {
            if (this.PropertyChanged != null)
            {
                try
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(chage));
                }
                catch
                {
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Guid ComponentGuid
        {
            get { return this.UniqueID; }
        }
    }
}

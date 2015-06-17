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
            get
            {
                return new Guid();
            }
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
    }
}

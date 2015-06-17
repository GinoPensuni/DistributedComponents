using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiClientWPF
{
    public class Category
    {
        public Category()
        {
            Components = new ObservableCollection<Components>();
        }

        public string Name
        {
            get;
            set;
        }

        public ObservableCollection<Components> Components
        {
            get;
            set;
        }

    }
}

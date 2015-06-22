using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;

namespace Client
{
    public class WorkTask
    {
        private int addedParameters;

        public WorkTask(Guid id, IComponent component)
        {
            this.ID = id;
            this.Component = component;

            this.InputParameters = new object[component.InputHints.Count()];
            this.addedParameters = 0;
        }

        public Guid ID { get; private set; }

        public object[] InputParameters { get; private set; }

        public IComponent Component { get; private set; }

        public bool HasAllInputParameters()
        {
            return addedParameters == this.Component.InputHints.Count();
        }

        public bool SetParameter(object value, int index)
        {
            if (index < 0)
            {
                return this.SetParameter(value);
            }
            else if (this.Component.InputHints.ToList()[index].Equals(value.GetType().ToString()))
            {
                this.addedParameters++;
                this.InputParameters[index] = value;

                return true;
            }

            return false;
        }

        // Should only be called when its irrelevant which parameter becomes the value. (for example addition)
        public bool SetParameter(object value)
        {
            int index = 0;

            foreach (string hint in this.Component.InputHints)
            {
                if (hint.Equals(value.GetType().ToString()) && this.InputParameters[index] == null)
                {
                    return this.SetParameter(value, index);
                }

                index++;
            }

            return false;
        }
    }
}
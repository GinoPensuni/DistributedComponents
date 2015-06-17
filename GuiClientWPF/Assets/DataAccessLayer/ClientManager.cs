using CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Shapes;

namespace GuiClientWPF
{
    public class ClientManager
    {
        private ILogic logic;
        private readonly static ClientManager manager = new ClientManager();

        public static ClientManager Manager
        {
            get
            {
                return ClientManager.manager;
            }
            set
            {

            }
        }

        public ObservableCollection<SimpleComponent> SimpleComponents
        {
            get;
            set;
        }

        public ObservableCollection<ComplexComponent> ComplexComponents
        {
            get;
            set;
        }

        public ObservableCollection<Components> OtherComponents
        {
            get;
            set;
        }

        public ObservableCollection<Components> CanvasComponents
        {
            get;
            set;
        }

        public ClientManager()
        {
            logic = default(ILogic);
            SimpleComponents = new ObservableCollection<SimpleComponent>();
            ComplexComponents = new ObservableCollection<ComplexComponent>();
            OtherComponents = new ObservableCollection<Components>();
            CanvasComponents = new ObservableCollection<Components>();
        }

        internal Task Disconnect()
        {
            var disconnectionTask = new Task(() => {
             logic.DisconnectFromServer();
            });

            disconnectionTask.Start();
            return disconnectionTask;
        }

        internal Task ConnectAction()
        {
            var connectionTask = new Task(() =>
            {
                logic.ConnenctToServer();
            });

            connectionTask.Start();
            return connectionTask;
        }

        internal Task SaveComponent(ICollection<Tuple<Rectangle, Rectangle>> componentList)
        {
            var saveTask = new Task(async () => {
                await logic.SaveComponent(this.GenerateComponent(componentList));
            });

            saveTask.Start();
            return saveTask;
       }

        internal Task LoadComponents()
        {
           var loadingComponentTask = new Task(async ()=>{
               var components = await logic.LoadComponents();
               foreach (var entry in components)
               {
                   if (entry.Item2.GetType().Equals(typeof(SimpleComponent)))
                   {
                       this.SimpleComponents.Add(entry.Item2 as SimpleComponent);
                   }
                   else if (entry.Item2.GetType().Equals(typeof(ComplexComponent)))
                   {
                       this.ComplexComponents.Add(entry.Item2 as ComplexComponent);
                   }
                   else if(entry.Item2.GetType().Equals(typeof(Components))){
                       this.OtherComponents.Add(entry.Item2 as Components);
                   }
               }
           });

           loadingComponentTask.Start();
           return loadingComponentTask;
        }

        private IComponent GenerateComponent(ICollection<Tuple<Rectangle, Rectangle>> componentList)
        {
            return default(IComponent);
        }

        internal void FillTestDataAsync()
        {
            this.SimpleComponents.Add(new SimpleComponent("Addition"));
            this.SimpleComponents.Add(new SimpleComponent("Substraction"));
            this.SimpleComponents.Add(new SimpleComponent("Division"));
            this.SimpleComponents.Add(new SimpleComponent("Multiplication"));

            this.ComplexComponents.Add(new ComplexComponent("Complex Addition"));
            this.ComplexComponents.Add(new ComplexComponent("Complex Substraction"));
            this.ComplexComponents.Add(new ComplexComponent("Complex Division"));
            this.ComplexComponents.Add(new ComplexComponent("Complex Multiplication"));

            this.OtherComponents.Add(new SimpleComponent("Simple other Addition"));
            this.OtherComponents.Add(new ComplexComponent("Complex other Substraction"));
            this.OtherComponents.Add(new ComplexComponent("Complex other Division"));
            this.OtherComponents.Add(new SimpleComponent("Simple otherMultiplication"));
        }
    }
}

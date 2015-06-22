using CommonRessources;
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
        private IClientLogic logic;
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

        public ObservableCollection<Category> CathegoryCollection
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
            logic = default(IClientLogic);
            CathegoryCollection = new ObservableCollection<Category>();
            CanvasComponents = new ObservableCollection<Components>();
            CathegoryCollection.Add(new Category() { Name = "Simple" });
            CathegoryCollection.Add(new Category() { Name = "Complex" });
            CathegoryCollection.Add(new Category() { Name = "Other" });
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
            var connectionTask = new Task(() => { logic.ConnenctToServer("test"); });

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
                       this.CathegoryCollection[0].Components.Add(entry.Item2 as SimpleComponent);
                   }
                   else if (entry.Item2.GetType().Equals(typeof(ComplexComponent)))
                   {
                       this.CathegoryCollection[1].Components.Add(entry.Item2 as ComplexComponent);
                   }
                   else if(entry.Item2.GetType().Equals(typeof(Components))){
                       this.CathegoryCollection[2].Components.Add(entry.Item2 as Components);
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
            this.CathegoryCollection[0].Components.Add(new SimpleComponent("Addition"));
            this.CathegoryCollection[0].Components.Add(new SimpleComponent("Substraction"));
            this.CathegoryCollection[0].Components.Add(new SimpleComponent("Division"));
            this.CathegoryCollection[0].Components.Add(new SimpleComponent("Multiplication"));

            this.CathegoryCollection[1].Components.Add(new ComplexComponent("Complex Addition"));
            this.CathegoryCollection[1].Components.Add(new ComplexComponent("Complex Substraction"));
            this.CathegoryCollection[1].Components.Add(new ComplexComponent("Complex Division"));
            this.CathegoryCollection[1].Components.Add(new ComplexComponent("Complex Multiplication"));

            this.CathegoryCollection[2].Components.Add(new SimpleComponent("Simple other Addition"));
            this.CathegoryCollection[2].Components.Add(new ComplexComponent("Complex other Substraction"));
            this.CathegoryCollection[2].Components.Add(new ComplexComponent("Complex other Division"));
            this.CathegoryCollection[2].Components.Add(new SimpleComponent("Simple otherMultiplication"));
        }

        internal void AddCanvasComponent(Guid? id)
        {
            if (id == null)
            {
                return;
            }

            var queryRes = CathegoryCollection.SelectMany(category => category.Components).SingleOrDefault(component => component.UniqueID == id);
            if (queryRes != null) 
                this.CanvasComponents.Add(queryRes);
        }

    }
}

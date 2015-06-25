﻿using CommonRessources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using AppLogic.ClientLogic;
using NetworkComponent = Core.Network.Component;
using System.Windows;

namespace GuiClientWPF
{
    public class ClientManager
    {
        private NetworkComponent currentGui;
        private IClientLogic logic;
        private readonly static ClientManager manager = new ClientManager();
        private System.Windows.Threading.Dispatcher disp;
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
            logic = ClientLogic.LogicClient;
            CathegoryCollection = new ObservableCollection<Category>();
            CanvasComponents = new ObservableCollection<Components>();
            CathegoryCollection.Add(new Category() { Name = "Simple" });
            CathegoryCollection.Add(new Category() { Name = "Complex" });
            CathegoryCollection.Add(new Category() { Name = "Other" });
            logic.OnComponentsLoaded += logic_OnComponentsLoaded;
        }

        void logic_OnComponentsLoaded(object sender, LoadedCompoentEventArgs e)
        {
            throw new NotImplementedException();
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

        internal Task SaveComponent(ICollection<Tuple<Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, LineContainer>> componentList, System.Windows.Threading.Dispatcher disp)
        {
            this.disp = disp;
            var saveTask = new Task(() => {
                this.disp.Invoke(async() =>
                {
                    await logic.SaveComponent(await this.GenerateComponent(componentList));
                });
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
                   string name = entry.Item2.FriendlyName;
                   var inputHints = entry.Item2.InputHints;
                   var outputhints = entry.Item2.OutputHints;
                   if (entry.Item1 == ComponentType.Simple)
                   {
                       this.CathegoryCollection[0].Components.Add(new Components() { ComponentGuid = entry.Item2.ComponentGuid, FriendlyName = name, InputHints = inputHints, OutputHints = outputhints });
                   }

               }
           });

           loadingComponentTask.Start();
           return loadingComponentTask;
        }

        private async Task<NetworkComponent> GenerateComponent(ICollection<Tuple<Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, LineContainer>> componentList)
        {
           var list = await ParsingTask(componentList);
           foreach (var entry in list)
           {
               MessageBox.Show(entry.Item1.FriendlyName.Text);
           }
           await NetworkGenerator(await ParsingTask(componentList));
          
           MessageBox.Show(this.currentGui.Edges.ToString());
            return this.currentGui;
        }

        private Task<ICollection<Tuple<GuiComponent, GuiComponent, LineContainer>>> ParsingTask(ICollection<Tuple<Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, Tuple<GuiComponent, InputNodeComponent, Ellipse, Point>, LineContainer>> componentList)
        {
            var parsingTask = new Task<ICollection<Tuple<GuiComponent, GuiComponent, LineContainer>>>(() => 
            {
               return this.disp.Invoke(() =>
                {
                    return componentList.Select(tuple => new Tuple<GuiComponent, GuiComponent, LineContainer>(tuple.Item1.Item1, tuple.Item2.Item1, tuple.Item3)).ToList();
                    });
            });

            parsingTask.Start();
            return parsingTask;
        }

        private Task NetworkGenerator(ICollection<Tuple<GuiComponent, GuiComponent, LineContainer>> componentList)
        {
            var generator = new Task(async () =>
            {
                this.currentGui = new NetworkComponent()
                {
                    ComponentGuid = Guid.NewGuid(),
                    FriendlyName = "Autogenerated",
                    InputHints = await this.FindInputhints(componentList),
                    OutputHints = await this.FindOutPutHints(componentList),
                };

                this.currentGui.Edges = await this.GrenadeEdges(componentList, this.currentGui.ComponentGuid);
            });

            generator.Start();
            return generator;
        }

        private Task<IEnumerable<Core.Network.ComponentEdge>> GrenadeEdges(ICollection<Tuple<GuiComponent, GuiComponent, LineContainer>> componentList, Guid id)
        {
            var edgeCreationTask = new Task<IEnumerable<Core.Network.ComponentEdge>>(() => 
            {
                var componentInputport = (uint)0;
                var componentOutputport = (uint)0;
                var edgeList = new List<Core.Network.ComponentEdge>();
                foreach (var entry in componentList)
                {
                    var Componentedge = new List<Core.Network.ComponentEdge>();
                    
                    if (entry.Item1.FreeInputNodes > 0)
                    {
                        if (!entry.Item1.InputVisit)
                        {
                            foreach (var connection in entry.Item1.FreeInputNodesList)
                            {
                                var edge = new Core.Network.ComponentEdge();
                                edge.OutputComponentGuid = id;
                                edge.InputComponentGuid = entry.Item1.Component.Entry.UniqueID;
                                edge.OutputValueID = ++componentInputport;
                                edge.InputValueID = (uint)entry.Item1.InputNodesList.IndexOf(connection);
                                edge.InternalInputComponentGuid = Guid.Empty;
                                edge.InternalOutputComponentGuid = entry.Item1.Id;
                                edgeList.Add(edge);
                            }
                        }
                    }
                    if(entry.Item2.FreeInputNodes >0)
                    {
                        if (!entry.Item2.InputVisit)
                        {

                            foreach (var connection in entry.Item2.FreeInputNodesList)
                            {
                                var edge = new Core.Network.ComponentEdge();
                                edge.OutputComponentGuid = id;
                                edge.InputComponentGuid = entry.Item2.Component.Entry.UniqueID;
                                edge.OutputValueID = ++componentInputport;
                                edge.InputValueID = (uint)entry.Item2.InputNodesList.IndexOf(connection);
                                edge.InternalInputComponentGuid = Guid.Empty;
                                edge.InternalOutputComponentGuid = entry.Item2.Id;
                                edgeList.Add(edge);
                            }
                        }

                    }
                    if(entry.Item1.FreeOutputNodes > 0)
                    {
                        if (!entry.Item1.OutputVisit)
                        {
                            foreach (var connection in entry.Item1.FreeOutputNodesList)
                            {
                                var edge = new Core.Network.ComponentEdge();
                                edge.OutputComponentGuid = entry.Item1.Component.Entry.UniqueID;
                                edge.InputComponentGuid = id;
                                edge.OutputValueID = (uint)entry.Item1.OutputNodesList.IndexOf(connection);
                                edge.InputValueID = ++componentOutputport;
                                edge.InternalInputComponentGuid = entry.Item1.Id;
                                edge.InternalOutputComponentGuid = Guid.Empty;
                                edgeList.Add(edge);
                            }
                        }
                    }
                    if (entry.Item2.FreeOutputNodes > 0)
                    {
                        if (!entry.Item2.OutputVisit)
                        {
                            foreach (var connection in entry.Item2.FreeOutputNodesList)
                            {
                                var edge = new Core.Network.ComponentEdge();
                                edge.OutputComponentGuid = entry.Item2.Component.Entry.UniqueID;
                                edge.InputComponentGuid = id;
                                edge.OutputValueID = (uint)entry.Item2.OutputNodesList.IndexOf(connection);
                                edge.InputValueID = ++componentOutputport;
                                edge.InternalInputComponentGuid = entry.Item2.Id;
                                edge.InternalOutputComponentGuid = Guid.Empty;
                                edgeList.Add(edge);
                            }
                        }
                    }
                    entry.Item1.InputVisit = true;
                    entry.Item1.OutputVisit = true;
                    entry.Item2.InputVisit = true;
                    entry.Item2.OutputVisit = true;
                }
                this.GenerateInnerNodes(componentList,ref edgeList);
                return edgeList;
            });

            edgeCreationTask.Start();
            return edgeCreationTask;
        }

        private void GenerateInnerNodes(ICollection<Tuple<GuiComponent, GuiComponent, LineContainer>> componentList, ref List<Core.Network.ComponentEdge> edgeList)
        {
            var lines = componentList.Select(compoent => new { From = compoent.Item1, To = compoent.Item2, Line = compoent.Item3 });
            foreach (var entry in lines)
            {
                edgeList.Add(
                    new Core.Network.ComponentEdge()
                    {   InputComponentGuid = entry.To.Entry.UniqueID,
                        OutputComponentGuid = entry.From.Entry.UniqueID,
                        InternalInputComponentGuid = entry.To.Id,
                        InternalOutputComponentGuid = entry.From.Id,
                        OutputValueID = (uint)entry.To.InputNodesList.IndexOf(entry.Line.To),
                        InputValueID = (uint)entry.From.OutputNodesList.IndexOf(entry.Line.From),
                    });
            }
        }



        private Task<IEnumerable<string>> FindOutPutHints(ICollection<Tuple<GuiComponent, GuiComponent, LineContainer>> componentList)
        {
            var outputhintsCreationTask = new Task<IEnumerable<string>>(() => 
            {
                return componentList.Where(tupleinfo => tupleinfo.Item1.FreeOutputNodes > 0 || tupleinfo.Item2.FreeOutputNodes > 0)
                .SelectMany(tupleinfo => tupleinfo.Item1.FreeOutputNodesList.Select(node => node.Hint)
                .Concat(tupleinfo.Item2.FreeOutputNodesList.Select(node => node.Hint)))
                .Distinct();
               
            });

            outputhintsCreationTask.Start();
            return outputhintsCreationTask;

        }

        private Task<IEnumerable<string>> FindInputhints(ICollection<Tuple<GuiComponent, GuiComponent, LineContainer>> componentList)
        {
            var inputhintsCreationTask = new Task<IEnumerable<string>>(() =>
            {
                return componentList.Where(tupleinfo => tupleinfo.Item1.FreeInputNodes > 0 || tupleinfo.Item2.FreeInputNodes > 0)
                .SelectMany(tupleinfo => tupleinfo.Item1.FreeInputNodesList.Select(node => node.Hint)
                .Concat(tupleinfo.Item2.FreeInputNodesList.Select(node => node.Hint)))
                .Distinct();

            });

            inputhintsCreationTask.Start();
            return inputhintsCreationTask;
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

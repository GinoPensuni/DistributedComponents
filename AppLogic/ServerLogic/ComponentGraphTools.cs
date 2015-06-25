using CommonRessources;
using Core.Network;
using DataStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AppLogic.ServerLogic
{
    public static class ComponentGraphTools
    {
        public static void ExtractNodes(IEnumerable<ComponentEdge> edges, Dictionary<Guid, ComponentWorker> workerMap, BinaryFormatter bf, INetworkServer serverReference, ComponentStore store)
        {
            var nodeTuples = edges.SelectMany(
                e => new[] 
                {
                    new Tuple<Guid, Guid>(e.InputComponentGuid, e.InternalInputComponentGuid), 
                    new Tuple<Guid, Guid>(e.OutputComponentGuid, e.InternalOutputComponentGuid)
                });

            foreach (var nodeTuple in nodeTuples)
            {
                var componentKey = nodeTuple.Item1;
                var nodeKey = nodeTuple.Item2;

                if (nodeKey != Guid.Empty && !workerMap.ContainsKey(nodeKey))
                {
                    var componentEntry = store[nodeKey];
                    var componentBinary = componentEntry.Item1;
                    var isAtomic = componentEntry.Item2;
                    ComponentWorker newWorker = null;

                    if (isAtomic)
                    {
                        newWorker = new ComponentWorker(serverReference, componentKey, componentBinary);
                        workerMap[nodeKey] = newWorker;
                    }
                    else
                    {
                        MemoryStream deserializationStream = new MemoryStream(componentBinary);
                        var componentGraph = (IEnumerable<ComponentEdge>)bf.Deserialize(deserializationStream);
                        newWorker = new ComponentWorker(serverReference, store, componentKey, componentGraph);
                        workerMap[nodeKey] = newWorker;
                    }
                }
            }
        }

        public static void ExtractInnerEdges(IEnumerable<ComponentEdge> edges, Dictionary<Guid, ComponentWorker> workerMap)
        {
            BinaryFormatter bf = new BinaryFormatter();

            foreach (var edge in edges.Where(e => e.InternalInputComponentGuid != Guid.Empty && e.InternalOutputComponentGuid != Guid.Empty))
            {
                ComponentWorker destinationWorker;
                ComponentWorker sourceWorker;
                Dictionary<uint, DataGate> inputGates = new Dictionary<uint, DataGate>();
                destinationWorker = workerMap[edge.InternalInputComponentGuid];
                sourceWorker = workerMap[edge.InternalOutputComponentGuid];
                DataGate edgeConnector = new DataGate();
                destinationWorker.InputGates[edge.InputValueID] = edgeConnector;
                sourceWorker.OutputGates[edge.OutputValueID] = edgeConnector;
            }
        }

        public static void ExtractOuterEdges(IEnumerable<ComponentEdge> edges, Dictionary<Guid, ComponentWorker> workerMap, Dictionary<uint, DataGate> inputDataGates, Dictionary<uint, DataGate> outputDataGates)
        {
            foreach (var edge in edges.Where(e => e.InternalOutputComponentGuid == Guid.Empty))
            {
                var destinationWorker = workerMap[edge.InternalInputComponentGuid];
                destinationWorker.InputGates[edge.InputValueID] = inputDataGates[edge.OutputValueID];
            }

            foreach (var edge in edges.Where(e => e.InternalInputComponentGuid == Guid.Empty))
            {
                var sourceWorker = workerMap[edge.InternalOutputComponentGuid];
                sourceWorker.OutputGates[edge.OutputValueID] = outputDataGates[edge.InputValueID];
            }
        }

    }
}

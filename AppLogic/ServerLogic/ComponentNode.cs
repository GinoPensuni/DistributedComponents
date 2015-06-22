using Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLogic.ServerLogic
{
    public class ComponentNode
    {
        public Dictionary<uint, ComponentNode> Outputs { get; set; }

        public Dictionary<uint, ComponentNode> Inputs { get; set; }

        public Guid ComponentId { get; set; }

        public Guid InternalId { get; set; }

        public string FriendlyName { get; set; }

        public List<string> InputTypes { get; set; }

        public List<string> OutputTypes { get; set; }

        public IEnumerable<ComponentEdge> InternalEdges { get; set; }
    }
}

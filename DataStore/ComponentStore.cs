using CommonRessources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataStore
{
    public class ComponentStore : IStore
    {
        public ComponentStore()
        {
            this.DbContext = new ComponentDataContext();
        }

        private ComponentDataContext DbContext
        {
            get;
            set;
        }

        public Tuple<byte[], bool> this[Guid AssemblyID]
        {
            get
            {
                var comp = DbContext.Components.Single(component => component.Id == AssemblyID);
                return new Tuple<byte[], bool>(comp.Assembly, comp.IsAtomic);
            }
        }

        public byte[] this[int hash]
        {
            get
            {
                return DbContext.Components.Single(component => component.HashCode == hash).Assembly;
            }
        }

        public bool Store(byte[] assemblyCode)
        {

             int hash = assemblyCode.GetHashCode();
             if (this.DbContext.Components.Any(component => component.HashCode.CompareTo(hash).Equals(0)))
             {
                 throw new DuplicateNameException("Assemly hash already stored");
             }

             var datacomponent = new DataComponent();
             datacomponent.HashCode = hash;
             datacomponent.Assembly = assemblyCode;
             this.DbContext.SaveChanges();
             return true;
        }


        public List<byte[]> LoadAssemblies()
        {
            return this.DbContext.Components.Select(component => component.Assembly).ToList();
        }
    }
}

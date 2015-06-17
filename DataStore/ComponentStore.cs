using CommonInterfaces;
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

        public byte[] this[Guid AssemblyID]
        {
            get
            {
                return DbContext.Components.Single(component => component.Id == AssemblyID).Assembly;
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
    }
}


using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore
{
    internal class ComponentDataContext : DbContext
    {
        public ComponentDataContext()
        {
            Database.SetInitializer<ComponentDataContext>(new CreateDatabaseIfNotExists<ComponentDataContext>());
            Database.SetInitializer<ComponentDataContext>(new DropCreateDatabaseIfModelChanges<ComponentDataContext>());
        }

        public DbSet<DataComponent> Components
        {
            get;
            set;
        }
    }
}

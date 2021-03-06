﻿using CommonRessources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataStore
{
    public class ComponentStore
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

        //public byte[] this[int hash]
        //{
        //    get
        //    {
        //        return DbContext.Components.Single(component => component.HashCode == hash).Assembly;
        //    }
        //}

        public bool Store(Guid componentGuid, string name, bool isAtomic, byte[] assemblyCode)
        {
            if (this.DbContext.Components.Any(component => component.Id == componentGuid))
            {
                throw new DuplicateNameException("Assemly hash already stored");
            }

            var datacomponent = new DataComponent();
            datacomponent.Assembly = assemblyCode;
            datacomponent.Id = componentGuid;
            datacomponent.IsAtomic = isAtomic;
            datacomponent.Name = name;
            this.DbContext.Components.Add(datacomponent);
            this.DbContext.SaveChanges();
            return true;
        }


        public List<DataComponent> LoadAssemblies()
        {
            return this.DbContext.Components.ToList();
        }

        public void ClearDatabase()
        {
            this.DbContext.Components.RemoveRange(this.DbContext.Components);
            this.DbContext.SaveChanges();
        }
    }
}

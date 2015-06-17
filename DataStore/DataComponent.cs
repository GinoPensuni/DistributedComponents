using CommonInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataStore
{

    public class DataComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id
        {
            get;
            set;
        }

        public byte[] Assembly
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int HashCode
        {
            get;
            set;
        }
    }
}

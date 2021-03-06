﻿using CommonRessources;
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
        //[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public Guid Id
        {
            get;
            set;
        }

        public bool IsAtomic 
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

    }
}

using MapDicer.MtCompat.src;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

namespace MapDicer.Models
{
    [Table("Layer")]
    public class Layer : MapNode
    {
        [Key, Column("LayerId", TypeName = "INT"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short LayerId { get; set; }

        [Required, Column("Order", TypeName = "INT"), Index(IsUnique = true)]
        public short Order { get; set; }

        [Required, Column("Name", TypeName = "TEXT")]
        public string Name { get; set; }
    }
}

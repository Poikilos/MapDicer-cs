using MapDicer.MtCompat.src;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
// using System.Data.SQLite.Linq;

namespace MapDicer.Models
{
    [Table("Layer")]
    public class Layer : MapNode
    {
        [Key, Column("LayerId"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short LayerId { get; set; }

        [Required, Column("Order"), Index(IsUnique = true)]
        public short Order { get; set; }

        [Required, Column("Name"), Index(IsUnique = true)]
        public string Name { get; set; }
    }
}

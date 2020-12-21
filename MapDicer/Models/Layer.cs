using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
// using System.Data.Entity.Core.Mapping;
using System.Data.SQLite;
// using System.Data.SQLite.EF6;
// using System.Data.SQLite.Linq;
// using System.Data.Linq;
using System.Linq; // orderby etc

using MapDicer.MtCompat;

namespace MapDicer.Models
{
    [Table("Layer")]
    public class Layer : MapNode
    {
        [Key, Column("LayerId"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short LayerId { get; set; }
        public short Primary
        {
            get
            {
                return LayerId;
            }
        }

        [Required, Column("Num"), Index(IsUnique = true)]
        public short Num { get; set; }

        [Required, Column("Name"), Index(IsUnique = true)]
        public string Name { get; set; }

        public static bool Insert(Layer newEntry)
        {
            bool ok = false;
            using (var context = new MapDicerContext())
            {
                context.Database.CreateIfNotExists();
                context.Layers.Add(newEntry);
                ok = context.SaveChanges() > 0;
            }
            return ok;
        }// (*Linq to db*, 2020)
    }
}

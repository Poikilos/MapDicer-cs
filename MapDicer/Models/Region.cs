using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using System.Linq;
using System.Text;

namespace MapDicer.Models
{
    [Table("Region")]
    public class Region
    {
        public Region()
        {
            Mapblocks = new List<Mapblock>();
        }
        public virtual ICollection<Mapblock> Mapblocks { get; set; }

        [Key, Column("RegionId", TypeName = "INT"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RegionId { get; set; }
        /// <summary>
        /// The region name, such as the name of the contenent if Lod's name is continent.
        /// </summary>
        [Required, Column("Name", TypeName = "TEXT")]
        public string Name { get; set; }

        /// <summary>
        /// This is the parent.
        /// </summary>
        [Required, Column("LodId", TypeName ="INT")]
        public short LodId { get; set; }
        public virtual Lod Lod { get; set; }
    }
}

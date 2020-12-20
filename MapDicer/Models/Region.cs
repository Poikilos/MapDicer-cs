using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;

namespace MapDicer.Models
{
    public class Region
    {
        public int RegionId { get; set; }
        /// <summary>
        /// The region name, such as the name of the contenent if Lod's name is continent.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This is the parent.
        /// </summary>
        public short LodId { get; set; }
        public virtual Lod Lod { get; set; }
    }
}

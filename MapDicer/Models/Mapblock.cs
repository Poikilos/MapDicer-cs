using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDicer.Models
{
    public class Mapblock
    {
        public int MapblockId { get; set; }
        public string Name { get; set; }
        
        /// <summary>
        /// This is the parent.
        /// </summary>
        public short LodId { get; set; }
        public virtual Lod Lod { get; set; }
    }
}

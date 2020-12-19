using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDicer
{
    class Mapblock
    {
        public long MapblockId { get; set; }
        public string Title { get; set; }

        public short LodId { get; set; }
        public virtual Lod Lod { get; set; }
    }
}

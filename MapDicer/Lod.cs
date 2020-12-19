using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDicer
{
    class Lod
    {
        public Lod()
        {
            Mapblocks = new List<Mapblock>();
        }

        public short LodId { get; set; }
        /// <summary>
        /// The name of this level of detail, such as World or Continent
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The unique parent LOD in the LOD chain
        /// </summary>
        public short Parent { get; set; }
        /// <summary>
        /// The statistic is only for display purposes.
        /// This stored for caching purposes (to prevent having to traverse to the leaf).
        /// </summary>
        public long UnitsPerSample { get; set; }
        /// <summary>
        /// This is how many pixels are in the square image. The region may contain more than
        /// one Mapblock.
        /// </summary>
        public long SamplesPerMapblock { get; set; }
        /// <summary>
        /// Whether this is the leaf. You cannot enter a leaf. A leaf is a Node.
        /// </summary>
        private bool IsLeaf { get; set; }

        public virtual ICollection<Mapblock> Mapblocks { get; set; }

        public bool GetIsLeaf()
        {
            return (GetChild(this.LodId) == null);
        }

        public static Lod GetChild(short parent)
        {
            using (var context = new MapDicerContext())
            {
                /*
                var lods = from l in context.Lods
                              where l.Parent == parent
                              orderby l.Name
                              select l;

                foreach (var lod in lods)
                {
                    return lod;
                }
                */
                // See
                // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/insert-update-and-delete-operations
                var existing =
                    (from v in context.Lods
                     where v.LodId == parent
                     select v).First();
                return existing;
            }
            // return null;
        }

        public static List<Lod> All()
        {
            using (var context = new MapDicerContext()) // using (var db = new DbW4A1())
            {
                var query = from entry in context.Lods
                                // where entry.Id < 25
                            orderby entry.LodId ascending
                            select entry;
                return query.ToList();
            }
        } // (*Linq to db*, 2020)

        public static bool Insert(Lod lod)
        {
            bool ok = false;
            ok = true;

            return ok;
        }// (*Linq to db*, 2020)
        /// <summary>
        /// Update any fields that differ.
        /// </summary>
        /// <param name="lod">The new version of the lod with the matching LodId</param>
        /// <returns></returns>
        public static bool Update(Lod lod)
        {
            bool ok = false;
            using (var context = new MapDicerContext()) // using (var db = new DbW4A1())
            {
                // see https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/insert-update-and-delete-operations
                var existing =
                    (from v in context.Lods
                     where v.LodId == lod.LodId
                     select v).First();
                existing.Name = lod.Name;
                existing.Parent = lod.Parent;
                existing.UnitsPerSample = lod.UnitsPerSample;
                existing.SamplesPerMapblock = lod.SamplesPerMapblock;
                existing.IsLeaf = lod.IsLeaf;

                // db.SubmitChanges();
                ok = true;

            }
            return ok;
        }// (*Linq to db*, 2020)

        /*
        public static bool Delete(Lod lod)
        {
            bool ok = false;
            using (var context = new MapDicerContext()) // var db = new DbW4A1())
            {
                context.Lods
                  .Where(p => p.Deleted)
                  .Delete();
                ok = true;
            }
            return ok;
        } // (*Linq to db*, 2020)
        */
    }
}

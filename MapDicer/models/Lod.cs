using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDicer
{
    public class Lod
    {
        public Lod()
        {
            Mapblocks = new List<Mapblock>();
        }

        public short LodId { get; set; }
        public short Id
        {
            get
            {
                return LodId;
            }
        }
        /// <summary>
        /// The name of this level of detail, such as World or Continent
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The unique parent LOD in the LOD chain
        /// </summary>
        public short Parent { get; set; }
        /// <summary>
        /// This is how many pixels are in the square image. The region may contain more than
        /// one Mapblock.
        /// </summary>
        public long SamplesPerMapblock { get; set; }

        #region computed
        /// <summary>
        /// The statistic is only for display purposes.
        /// This stored for caching purposes (to prevent having to traverse to the leaf).
        /// </summary>
        public long UnitsPerSample { get; set; }
        /// <summary>
        /// Whether this is the leaf. You cannot enter a leaf. A leaf is a Node.
        /// </summary>
        private bool IsLeaf { get; set; }
        #endregion computed

        public virtual ICollection<Mapblock> Mapblocks { get; set; }


        public static short GetNewId()
        {
            return (short)(Lod.LastId() + 1);
        }
        public bool GetIsLeaf()
        {
            return (GetChild(this.LodId) == null);
        }

        public static Lod GetChild(short parent)
        {
            using (var context = new MapDicerContext()) // var db = new ...())
            {
                // See
                // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/insert-update-and-delete-operations
                var existing =
                    (from v in context.Lods
                     where v.LodId == parent
                     select v).FirstOrDefault();
                // FirstOrDefault can handle null without throwing an exception.
                // Only use it when you do not need a record.

                return existing;
            }
            // return null;
        }

        public static List<Lod> All()
        {
            using (var context = new MapDicerContext())
            {
                var query = from entry in context.Lods
                                // where entry.Id < 25
                            orderby entry.LodId ascending // the Last method depends on ascending.
                            select entry;
                return query.ToList();
            }
        } // (*Linq to db*, 2020)
        public static Lod Last()
        {
            using (var context = new MapDicerContext())
            {
                var existing = (from entry in context.Lods
                                // where entry.Id < 25
                            orderby entry.LodId ascending // the Last method depends on ascending.
                            select entry).Last();
                return existing;
            }
        }
        public static short LastId()
        {
            short result = -1;
            Lod existing = Last();
            if (existing != null)
                result = existing.LodId;
            return result;
        }

        public static Lod GetById(short lodId)
        {
            using (var context = new MapDicerContext())
            {
                var existing = (from entry in context.Lods
                                    // where entry.Id < 25
                                orderby entry.LodId ascending
                                select entry).FirstOrDefault();
                return existing;
            }
        }
        public static bool Insert(Lod entry, bool generateId)
        {
            bool ok = false;
            if (generateId)
            {
                entry.LodId = GetNewId();
            }
            using (var context = new MapDicerContext())
            {
                
                context.Lods.Add(entry);
                ok = context.SaveChanges() > 0;
            }
            return ok;
        }// (*Linq to db*, 2020)

        /// <summary>
        /// Update any fields that differ.
        /// </summary>
        /// <param name="entry">The new version of the lod with the matching LodId</param>
        /// <returns></returns>
        public static bool Update(Lod entry)
        {
            bool ok = false;
            using (var context = new MapDicerContext())
            {
                // see https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/insert-update-and-delete-operations
                var existing =
                    (from v in context.Lods
                     where v.LodId == entry.LodId
                     select v).First();
                existing.Name = entry.Name;
                existing.Parent = entry.Parent;
                existing.SamplesPerMapblock = entry.SamplesPerMapblock;
                existing.UnitsPerSample = entry.UnitsPerSample;
                existing.IsLeaf = entry.IsLeaf;
                ok = context.SaveChanges() > 0;
            }
            return ok;
        }// (*Linq to db*, 2020)

        
        public static bool Delete(Lod entry)
        {
            bool ok = false;
            using (var context = new MapDicerContext())
            {
                var existing =
                    (from v in context.Lods
                     where v.LodId == entry.LodId
                     select v).First();
                context.Lods.Remove(existing);
                ok = context.SaveChanges() > 0;
            }
            return ok;
        } // (*Linq to db*, 2020)
    }
}

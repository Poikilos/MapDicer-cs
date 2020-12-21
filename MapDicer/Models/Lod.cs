using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
// using System.Data.Entity.ModelConfiguration.Conventions;
// using System.Data.Entity.Core.Mapping;
// using System.Data.Linq.Mapping;
using System.Data.SQLite;
// using System.Data.SQLite.EF6;
// using System.Data.SQLite.Linq;
using System.Linq; // orderby etc


namespace MapDicer.Models
{
    [Table("Lod")]
    public class Lod
    {
        public virtual ICollection<Mapblock> Mapblocks { get; set; }
        public virtual ICollection<Region> Regions { get; set; }
        public Lod()
        {
            Mapblocks = new List<Mapblock>();
            Regions = new List<Region>();
        }
        /*
        public void populateMapblocks()
        {
            Mapblocks = new List<Mapblock>();
        }
        */

        [Key, Column("LodId"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short LodId { get; set; }

        /// <summary>
        /// The name of this level of detail, such as World or Continent
        /// </summary>
        [Column("Name"), Required, Index(IsUnique = true)]
        public string Name { get; set; }
        /// <summary>
        /// The unique parent LOD in the LOD chain
        /// </summary>
        [Column("ParentLodId"), Index(IsUnique = true)]
        public short? ParentLodId { get; set; }
        [Association("lod_parentlodid_fk", "ParentLodId", "Lod.LodId", IsForeignKey = true)]
        public Lod Parent { get; set; }
        /// <summary>
        /// This is how many pixels are in the square image. The region may contain more than
        /// one Mapblock.
        /// </summary>
        [Column("SamplesPerMapblock"), Required]
        public int SamplesPerMapblock { get; set; }

        public Lod Child
        {
            get
            {
                return Lod.GetChild(this.LodId);
            }
        }

        #region computed
        /// <summary>
        /// The statistic is only for display purposes.
        /// This stored for caching purposes (to prevent having to traverse to the leaf).
        /// </summary>
        public long UnitsPerSample;
        /// <summary>
        /// Whether this is the leaf. You cannot enter a leaf. A leaf is a Node.
        /// </summary>
        public bool IsLeaf;
        #endregion computed



        public static short GetNewId()
        {
            return (short)(Lod.LastId() + 1);
        }
        public static bool GetIsLeaf(Lod entry)
        {
            return (GetChild(entry.LodId) == null);
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

        public static Queue<string> errors = new Queue<string>();

        public static List<Lod> All()
        {
            try
            {
                using (var context = new MapDicerContext())
                {
                    context.Database.CreateIfNotExists();
                    if (!context.Database.Exists())
                    {
                        /*
                        string msg = String.Format("The database is missing {0}", context.Database.Log);
                        if (!errors.Contains(msg))
                        {
                            errors.Enqueue(msg);
                        }
                        */
                        return null;
                    }
                    // MessageBox.Show(String.Format("context: {0}", context.Database.Exists()));
                    var query = from entry in context.Lods
                                    // where entry.Id < 25
                                orderby entry.LodId ascending // the Last method depends on ascending.
                                select entry;
                    return query.ToList();
                }
            }
            catch (System.ArgumentException ex)
            {
                string msg = ex.Message;
                if (!errors.Contains(msg))
                {
                    errors.Enqueue(msg);
                }
                return null;
            }
            /*
            catch (System.Data.SQLite.SQLiteException ex)
            {
                
                // SQLiteException: SQL logic error
                // no such table: Lod
                // ^ but that's just the inner exception. See below.
                
                string msg = ex.Message;
                if (!errors.Contains(msg))
                {
                    errors.Enqueue(msg);
                }
                return null;
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
            {
                string msg = ex.Message;
                if (!errors.Contains(msg))
                {
                    errors.Enqueue(msg);
                }
                return null;
            }
            */
            return null;
        } // (*Linq to db*, 2020)

        public static Lod Last()
        {
            using (var context = new MapDicerContext())
            {
                context.Database.CreateIfNotExists();
                if (!context.Database.Exists())
                {
                    /*
                    string msg = String.Format("The database is missing {0}", context.Database.Log);
                    if (!errors.Contains(msg))
                    {
                        errors.Enqueue(msg);
                    }
                    */
                    return null;
                }
                var existing = (from entry in context.Lods
                                // where entry.Id < 25
                            orderby entry.LodId descending // the Last method depends on ascending.
                            select entry).First();
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

        public static Lod GetById(short id)
        {
            using (var context = new MapDicerContext())
            {
                context.Database.CreateIfNotExists();
                var existing = (from entry in context.Lods
                                where entry.LodId == id
                                orderby entry.LodId ascending
                                select entry).FirstOrDefault();
                return existing;
            }
        }
        public static string Insert(Lod newEntry, bool generateId)
        {
            string error = "";
            Lod last = null;
            short newId = 0;
            if (generateId)
            {
                using (var context = new MapDicerContext())
                {
                    context.Database.CreateIfNotExists();
                    var existing = (from entry in context.Lods
                                        // where entry.Id < 25
                                    orderby entry.LodId descending // the Last method depends on ascending.
                                    select entry).FirstOrDefault();
                    if (existing != null)
                    {
                        newId = (short)(existing.LodId + 1); // last = existing;
                    }
                    // else assume no entries (leave new entry at 0 if generateId)
                }
            }

            using (var context = new MapDicerContext())
            {
                context.Database.CreateIfNotExists();
                if (generateId)
                {
                    newEntry.LodId = newId;
                    /*
                    newEntry.LodId = 0;
                    if (last != null)
                        newEntry.LodId = (short)(last.LodId + 1);
                    else
                        newEntry.LodId = 0; // Assumes the table is empty or not present.
                    */
                }

                context.Lods.Add(newEntry);
                try
                {
                    int count = context.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    // There may be two inner exceptions such as
                    // 1:
                    // UpdateException: An error occurred while updating the entries. See the inner exception for details.
                    // 2:
                    // SQLiteException: constraint failed
                    // UNIQUE constraint failed: Lod.LodId
                    error = ex.Message;
                    Exception ex1 = ex.InnerException;
                    if (ex1 != null)
                    {
                        error += "\n" + ex1.Message;
                        Exception ex2 = ex1.InnerException;
                        if (ex2 != null)
                        {
                            error += "\n" + ex2.Message;
                        }
                    }
                }
            }
            return error;
        }// (*Linq to db*, 2020)

        /// <summary>
        /// Update any fields that differ.
        /// </summary>
        /// <param name="entry">The new version of the lod with the matching LodId</param>
        /// <returns>True if ok</returns>
        public static bool Update(Lod entry)
        {
            bool ok = false;
            using (var context = new MapDicerContext())
            {
                context.Database.CreateIfNotExists();
                // see https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/insert-update-and-delete-operations
                var existing =
                    (from v in context.Lods
                     where v.LodId == entry.LodId
                     select v).First();
                existing.Name = entry.Name;
                existing.ParentLodId = entry.ParentLodId;
                existing.SamplesPerMapblock = entry.SamplesPerMapblock;
                existing.UnitsPerSample = entry.UnitsPerSample;
                existing.IsLeaf = entry.IsLeaf;
                ok = context.SaveChanges() > 0;
            }
            return ok;
        }

        
        public static bool Delete(Lod entry)
        {
            bool ok = false;
            using (var context = new MapDicerContext())
            {
                context.Database.CreateIfNotExists();
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

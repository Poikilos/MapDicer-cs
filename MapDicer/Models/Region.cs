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
// using System.Data.Linq;
using System.Linq; // orderby etc

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

        [Key, Column("RegionId"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RegionId { get; set; }
        /// <summary>
        /// The region name, such as the name of the contenent if Lod's name is continent.
        /// </summary>
        [Required, Column("Name")]
        public string Name { get; set; }

        /// <summary>
        /// This is the parent.
        /// </summary>
        [Required, Column("LodId")]
        public short LodId { get; set; }
        [Association("region_lodid_fk", "LodId", "Lod.LodId", IsForeignKey = true)]
        public virtual Lod Lod { get; set; }

        public static string Insert(Region newEntry, bool generateId)
        {
            string error = "";
            Region last = null;
            short newId = 0;
            if (generateId)
            {
                using (var context = new MapDicerContext())
                {
                    context.Database.CreateIfNotExists();
                    var existing = (from entry in context.Regions
                                        // where entry.Id < 25
                                    orderby entry.RegionId descending // the Last method depends on ascending.
                                    select entry).FirstOrDefault();
                    if (existing != null)
                    {
                        newId = (short)(existing.RegionId + 1); // last = existing;
                    }
                    // else assume no entries (leave new entry at 0 if generateId)
                }
            }

            using (var context = new MapDicerContext())
            {
                context.Database.CreateIfNotExists();
                if (generateId)
                {
                    newEntry.RegionId = newId;
                    /*
                    newEntry.RegionId = 0;
                    if (last != null)
                        newEntry.RegionId = (short)(last.RegionId + 1);
                    else
                        newEntry.RegionId = 0; // Assumes the table is empty or not present.
                    */
                }

                context.Regions.Add(newEntry);
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
                    // UNIQUE constraint failed: Region.RegionId
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
    }
}

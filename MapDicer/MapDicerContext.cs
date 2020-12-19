using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

// See https://damienbod.com/2013/11/14/using-sqlite-with-net/

namespace MapDicer
{
    class MapDicerContext : DbContext
    {
        public DbSet<Lod> Lods { get; set; }
        public DbSet<Mapblock> Mapblocks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Chinook Database does not pluralize table names
            modelBuilder.Conventions
                .Remove<PluralizingTableNameConvention>();
        }
    }
}

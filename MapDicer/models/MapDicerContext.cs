using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

// See https://damienbod.com/2013/11/14/using-sqlite-with-net/

namespace MapDicer.models
{
    class MapDicerContext : DbContext
    {
        public DbSet<Lod> Lods { get; set; }
        public DbSet<Mapblock> Mapblocks { get; set; }
        public DbSet<Region> Regions { get; set; }

        public MapDicerContext()
            : base(SettingModel.SqlConnectionString)
        {
            // MessageBox.Show(String.Format("Connection string: {0}", SettingModel.SqlConnectionString));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Do not pluralize table names.
            modelBuilder.Conventions
                .Remove<PluralizingTableNameConvention>();
        }
    }
}

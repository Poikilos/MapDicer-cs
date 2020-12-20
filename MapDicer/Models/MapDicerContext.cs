using System;
using System.Collections.Generic;
using System.Linq;
// using System.Text;
using System.Data.SQLite.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;

// See https://damienbod.com/2013/11/14/using-sqlite-with-net/

namespace MapDicer.Models
{
    class MapDicerContext : DbContext
    {
        static MapDicerContext() {
            SettingController.Start();
        }
        // can't derive from sealed class System.Data.SQLite.SQLiteContext
        public DbSet<Layer> Layers { get; set; }
        public DbSet<Lod> Lods { get; set; }
        public DbSet<Mapblock> Mapblocks { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Terrain> Terrains { get; set; }

        public MapDicerContext()
            : base(new SQLiteConnection() {
                ConnectionString  = SettingModel.SqlConnectionString
                
            }, true)
        {
            // MessageBox.Show(String.Format("Connection string: {0}", SettingModel.SqlConnectionString));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Do not pluralize table names.
            modelBuilder.Conventions
                .Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}

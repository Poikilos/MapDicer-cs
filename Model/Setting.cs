using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
// See <https://sqliteefcore-wasm.platform.uno/>
// You must import and install EntityFrameworkCore via Console Core not Universal Windows--see
// <https://github.com/dotnet/efcore/issues/9666> cited by
// <https://stackoverflow.com/questions/59444014/entity-framework-tools-not-working-with-uwp-apps-c-sharp>
namespace MapDicer
{
    class Setting
    {
    }

    // public static async Task Run()
    public static AddLOD(string name, string unit, double unitsPerSample)
    {
        using (var db = new SettingContext())
        {
            db.Database.EnsureCreated();

            Console.WriteLine("Database created");

            db.LODs.Add(new LOD { Name = name, Unit = unit, UnitsPerSample = unitsPerSample });
            var count = await db.SaveChangesAsync(CancellationToken.None);

            Console.WriteLine("{0} records saved to database", count);

            Console.WriteLine();
            Console.WriteLine("All blogs in database:");
            foreach (var lod in db.LODs)
            {
                Console.WriteLine(" - {0}", lod.Name);
            }
        }
    }
    public class SettingContext : DbContext
    {
        public DbSet<LOD> LODs { get; set; }
        public DbSet<Region> Regions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Uncomment those to enable logging
            // optionsBuilder.UseLoggerFactory(LogExtensionPoint.AmbientLoggerFactory);
            // optionsBuilder.EnableSensitiveDataLogging(true);

            // When building in app, use Windows.Storage.ApplicationData.Current.LocalFolder.Path
            // instead of /local to get browser persistence.
            optionsBuilder.UseSqlite($"data source=/local/local.db");
        }
    }
    public class LOD
    {
        public int LODId { get; set; }
        // Scale
        /// <summary>
        /// The scale name at this Level of Detail, such as "Planet"
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The unit of measurement, such as "m" for meters
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// The number of units per sample, such as 10 meters per LIDAR pixel.
        /// </summary>
        public double UnitsPerSample { get; set; }
        public List<Region> Regions { get; set; }
    }
    public class Region
    {
        public int RegionId { get; set; }
        public string Name { get; set; }
        public int LODId { get; set; }
        public LOD LOD { get; set; }
    }
}

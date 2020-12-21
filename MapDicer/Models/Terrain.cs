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
using System.Windows;
using System.Windows.Media;

namespace MapDicer.Models
{
    [Table("Terrain")]
    public class Terrain
    {
        public const int SourceWorldmapOverworldTileset = 1;
        /// <summary>
        /// The color on the map image denotes the terrain. Alpha is unused, so only 24 bits of this
        /// int will ever be used.
        /// </summary>
        [Key, Column("TerrainId"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TerrainId { get; set; }
        public int Primary
        {
            get
            {
                return TerrainId;
            }
        }

        [Required, Column("Name"), Index(IsUnique = true)]
        public string Name { get; set; }

        /// <summary>
        /// Ground, water, mountains, trees, etc
        /// </summary>
        [Column("Classification")]
        public string Classification { get; set; }

        /// <summary>
        /// Variant such as oak, pine, etc
        /// </summary>
        [Column("Variant")]
        public string Variant { get; set; }

        /// <summary>
        /// The path of the single tile.
        /// </summary>
        [Column("Path")]
        public string Path { get; set; }

        /// <summary>
        /// (Reserved for future use) The source of the image.
        /// </summary>
        [Column("SourceId")]
        public int? SourceId { get; set; }

        /// <summary>
        /// (Reserved for future use) If this was/is cropped from a tileset (denoted by SourceId in future
        /// versions), this is the ID.
        /// </summary>
        [Column("TileIndex")]
        public int? TileIndex { get; set; }

        /// <summary>
        /// (Reserved for future use) JSON metadata (NodeDef, one per many instances).
        /// </summary>
        [Column("JSON")]
        public string JSON { get; set; }

        /// <summary>
        /// Scale the graphic so this many pixels fits into the sample (this is for representing a terrain
        /// graphically rather than by plain color).
        /// </summary>
        [Column("PixPerSample")]
        public int PixPerSample { get; set; }

        public static Queue<string> errors = new Queue<string>();
        public static List<Terrain> All()
        {
            try
            {
                using (var context = new MapDicerContext())
                {
                    context.Database.CreateIfNotExists();
                    if (!context.Database.Exists())
                    {
                        return null;
                    }
                    var query = from entry in context.Terrains
                                    // where entry.Id < 25
                                orderby entry.Name ascending
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
            return null;
        } // (*Linq to db*, 2020)

        public static string Insert(Terrain newEntry)
        {
            string error = "";
            using (var context = new MapDicerContext())
            {
                context.Database.CreateIfNotExists();
                context.Terrains.Add(newEntry);
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
                    // UNIQUE constraint failed: Mapblock.MapblockId
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

        public Color GetColor()
        {
            return Terrain.ColorFromId(TerrainId);
        }


        public static int IntFromHexPair(string hexPairStr)
        {
            return Convert.ToInt32(hexPairStr, 16);
        }

        public static byte ByteFromHexPair(string hexPairStr)
        {
            return (byte)IntFromHexPair(hexPairStr);
        }
        public static string HexPair(byte v)
        {
            return Convert.ToString(v, 16).PadLeft(2, '0');
        }

        public static bool Test()
        {
            bool ok = true;
            string hexPairStr = "10";

            int value = IntFromHexPair(hexPairStr);
            string testStr = HexPair((byte)value);
            if (testStr != hexPairStr)
            {
                ok = false;
                MessageBox.Show(String.Format("Error: {0} made {1} which is really {2}", hexPairStr, value, testStr));
            }

            byte v = 128;
            string hStr = HexPair(v);
            byte testByte = ByteFromHexPair(hStr);
            if (testByte != v)
            {
                ok = false;
                MessageBox.Show(String.Format("Error: {0} made {1} which is really {2}", v, hStr, testByte));
            }
            return ok;
        }

        internal static Terrain GetById(int id)
        {
            using (var context = new MapDicerContext())
            {
                context.Database.CreateIfNotExists();
                var existing = (from entry in context.Terrains
                                where entry.Primary == id
                                orderby entry.Primary ascending
                                select entry).FirstOrDefault();
                return existing;
            }
        }

        /// <summary>
        /// Convert the string such that blue FIRST (in BBRRGG) is the MOST significant byte.
        /// Get the color as a Terrain primary key integer. The order is BGR so blue is most significant
        /// to immitate life (blue is a higher intensity by frequency).
        /// The ID only reaches the value of a 24-bit number.
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static int IdFromHexColorBgr(string hexColor)
        {
            if (hexColor.Length != 6)
            {
                throw new ApplicationException("The hex color must be 6 characters.");
            }
            return (IntFromHexPair(hexColor.Substring(0, 2)) * 65536
                    + IntFromHexPair(hexColor.Substring(2, 2)) * 256
                    + ByteFromHexPair(hexColor.Substring(4, 2)));
        }
        internal static int IdFromColorRgb(byte r, byte g, byte b)
        {
            return b * 65536 + g * 256 + r;
        }
        /// <summary>
        /// Convert the string such that red FIRST (in RRGGBB) is the LEAST significant byte.
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static int IdFromHexColorRgb(string hexColor)
        {
            if (hexColor.Length != 6)
            {
                throw new ApplicationException("The hex color must be 6 characters.");
            }
            return (IntFromHexPair(hexColor.Substring(4, 2)) * 65536
                    + IntFromHexPair(hexColor.Substring(2, 2)) * 256
                    + ByteFromHexPair(hexColor.Substring(0, 2)));
        }
        public static string HexBgrFromColor(byte r, byte g, byte b)
        {
            return HexPair(b) + HexPair(g) + HexPair(r);
        }
        public static string HexRgbFromColor(byte r, byte g, byte b)
        {
            return HexPair(r) + HexPair(g) + HexPair(b);
        }
        public static Color ColorFromHexColorRgb(string hexColor)
        {
            if (hexColor.Length != 6)
            {
                throw new ApplicationException("The hex color must be 6 characters.");
            }
            byte r = ByteFromHexPair(hexColor.Substring(0, 2));
            byte g = ByteFromHexPair(hexColor.Substring(2, 2));
            byte b = ByteFromHexPair(hexColor.Substring(4, 2));
            return Color.FromArgb(255, r, g, b);
        }
        public static Color ColorFromHexColorBgr(string hexColor)
        {
            if (hexColor.Length != 6)
            {
                throw new ApplicationException("The hex color must be 6 characters.");
            }
            byte b = ByteFromHexPair(hexColor.Substring(0, 2));
            byte g = ByteFromHexPair(hexColor.Substring(2, 2));
            byte r = ByteFromHexPair(hexColor.Substring(4, 2));
            return Color.FromArgb(255, r, g, b);
        }
        public static Color ColorFromId(int id)
        {
            // A byte overflow will occur on blue if the id is out of range.
            return Color.FromArgb(255, (byte)(id & 0xFF), (byte)((id >> 8) & 0xFF), (byte)(id >> 16));
        }

        
    }
}

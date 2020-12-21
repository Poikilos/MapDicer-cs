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
                                orderby entry.TerrainId ascending
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

        public static bool Insert(Terrain newEntry)
        {
            bool ok = false;
            using (var context = new MapDicerContext())
            {
                context.Database.CreateIfNotExists();
                context.Terrains.Add(newEntry);
                ok = context.SaveChanges() > 0;
            }
            return ok;
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
            return Convert.ToString(v, 16);
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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
// using System.Data.SQLite.Linq;
using System.Windows.Media;

namespace MapDicer.Models
{
    [Table("Terrain")]
    public class Terrain
    {
        /// <summary>
        /// The color on the map image denotes the terrain. Alpha is unused, so only 24 bits of this
        /// int will ever be used.
        /// </summary>
        [Key, Column("TerrainId", TypeName = "INT"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TerrainId { get; set; }

        [Required, Column("Name")]
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
        [Column("SourceId", TypeName = "INT")]
        public long SourceId { get; set; }

        /// <summary>
        /// (Reserved for future use) If this was/is cropped from a tileset (denoted by SourceId in future
        /// versions), this is the ID.
        /// </summary>
        [Column("TileIndex", TypeName = "INT")]
        public int TileIndex { get; set; }

        /// <summary>
        /// (Reserved for future use) JSON metadata (NodeDef, one per many instances).
        /// </summary>
        [Column("JSON")]
        public string JSON { get; set; }

        /// <summary>
        /// Scale the graphic so this many pixels fits into the sample (this is for representing a terrain
        /// graphically rather than by plain color).
        /// </summary>
        [Column("PixPerSample", TypeName = "INT")]
        public int PixPerSample { get; set; }

        public static byte IntFromHexPair(string hexPairStr)
        {
            return (byte)Convert.ToInt32(hexPairStr, 16);
        }
        public static byte ByteFromHexPair(string hexPairStr)
        {
            return (byte)IntFromHexPair(hexPairStr);
        }
        public static string HexPair(byte v)
        {
            return Convert.ToString(v, 16);
        }
        public static int IdFromHexColor(string hexColor)
        {
            if (hexColor.Length != 6)
            {
                throw new ApplicationException("The hex color must be 6 characters.");
            }
            // The ID only reaches the value of a 24-bit number.
            // Blue is most significant byte (to immitate life--blue is more intense).
            return (IntFromHexPair(hexColor.Substring(0, 2)) * 65536
                    + ByteFromHexPair(hexColor.Substring(2, 2)) * 256
                    + ByteFromHexPair(hexColor.Substring(4, 2)));
        }
        public static Color ColorFromHexColor(string hexColor)
        {
            if (hexColor.Length != 6)
            {
                throw new ApplicationException("The hex color must be 6 characters.");
            }
            byte r = ByteFromHexPair(hexColor.Substring(4, 2));
            byte g = ByteFromHexPair(hexColor.Substring(2, 2));
            byte b = ByteFromHexPair(hexColor.Substring(0, 2));
            return Color.FromArgb(255, r, g, b);
        }
        public static Color ColorFromId(int id)
        {
            // A byte overflow will occur on blue if the id is out of range.
            return Color.FromArgb(255, (byte)(id & 0xFF), (byte)((id >> 8) & 0xFF), (byte)(id >> 16));
        }
    }
}

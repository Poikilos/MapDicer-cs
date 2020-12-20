using MapDicer.MtCompat.src;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
// using System.Data.SQLite.Linq;

namespace MapDicer.Models
{
    [Table("Mapblock")]
    public class Mapblock : MapNode
    {
        /// <summary>
        /// The primary key must be long (INT in SQLite is same as long) because it contains the hash
        /// of the coordinates as the primary key. It is the key because the hash denotes a unique
        /// combination of location, Layer, and Lod (See MtCompat.MapDatabase.getBlockAsInteger).
        /// </summary>
        [Key, Column("MapblockId", TypeName = "INT"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MapblockId { get; set; }
        // public string Name { get; set; }
        public Mapblock()
        {

        }

        public MapDicerPos MapDicerPos
        {
            get
            {
                return new MapDicerPos(this.MapblockId);
            }
        }

        public short GetLayer()
        {
            return (short)(MapDatabase.getIntegerAsBlock(this.MapblockId).Y % SettingModel.MaxLayerCount);
        }
        public short GetLodId()
        {
            return (short)(MapDatabase.getIntegerAsBlock(this.MapblockId).Y / SettingModel.MaxLayerCount);
        }

        /// <summary>
        /// This is for getting infomation on how to place it, not for structure per se.
        /// </summary>
        [Required, Column("LodId", TypeName = "INT"), ForeignKey("Lod")]
        public short LodId { get; set; }
        public virtual Lod Lod { get; set; }


        /// <summary>
        /// This is for organizing by stat, not for structure per se.
        /// </summary>
        [Required, Column("LayerId", TypeName = "INT"), ForeignKey("Layer")]
        public short LayerId { get; set; }
        public virtual Layer Layer { get; set; }


        /// <summary>
        /// This is the structural (real) parent.
        /// </summary>
        [Required, Column("RegionId", TypeName = "INT"), ForeignKey("Region")]
        public long RegionId { get; set; }
        public virtual Region Region { get; set; }

        /// <summary>
        /// This is the default terrain for non-generated parts. It is visible through the transparent
        /// parts of the map data, so only 24-bits of it is used.
        /// </summary>
        [Column("TerrainId", TypeName = "INT")]
        public int TerrainId { get; set; }

        /// <summary>
        /// This is the map data for this section of the map (this map block). Each color in the image
        /// represents a terrain type.
        /// </summary>
        [Column("Path", TypeName = "TEXT")]
        public string Path { get; set; }

        // TODO: (future) -- this code is not validated
        /*
        [Column("Data", TypeName = "BLOB")]
        public byte[] Data { get; set; }
        */
    }
}

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

using MapDicer.MtCompat;

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
        [Key, Column("MapblockId"), DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        //, ForeignKey("LodId") // System.InvalidOperationException: 'The property 'LodId' cannot be configured as a navigation property. The property must be a valid entity type and the property should have a non-abstract getter and setter. For collection properties the type must implement ICollection<T> where T is a valid entity type.'
        [Required, Column("LodId")]
        public short LodId { get; set; }
        // private EntityRef<Lod> _lod = new EntityRef<Lod>();
        [Association("mapblock_lodid_fk", "LodId", "Lod.LodId", IsForeignKey = true)]
        public virtual Lod Lod { get; set; }
        // ^ See Bruniasty's answer on
        // https://stackoverflow.com/questions/29120917/how-to-add-a-relationship-between-tables-in-linq-to-db-model-class


        /// <summary>
        /// This is for organizing by stat, not for structure per se.
        /// </summary>
        [Required, Column("LayerId")]
        public short LayerId { get; set; }
        [Association("mapblock_lodid_fk", "LodId", "Lod.LodId", IsForeignKey = true)]
        public virtual Layer Layer { get; set; }


        /// <summary>
        /// This is the structural (real) parent.
        /// </summary>
        [Required, Column("RegionId")]
        public long RegionId { get; set; }
        [Association("mapblock_regionid_fk", "RegionId", "Region.RegionId", IsForeignKey = true)]
        public virtual Region Region { get; set; }

        /// <summary>
        /// This is the default terrain for non-generated parts. It is visible through the transparent
        /// parts of the map data, so only 24-bits of it is used.
        /// </summary>
        [Required, Column("TerrainId")]
        public int TerrainId { get; set; }
        [Association("mapblock_terrainid_fk", "TerrainId", "Terrain.TerrainId", IsForeignKey = true)]
        public virtual Terrain Terrain { get; set; }

        /// <summary>
        /// This is the map data for this section of the map (this map block). Each color in the image
        /// represents a terrain type.
        /// </summary>
        [Column("Path")]
        public string Path { get; set; }

        // TODO: (future) -- this code is not validated
        /*
        [Column("Data", TypeName = "BLOB")]
        public byte[] Data { get; set; }
        */
    }
}

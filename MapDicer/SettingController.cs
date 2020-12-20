using MapDicer.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace MapDicer
{
    class SettingController
    {
        private static bool started = false;
        public static void Start()
        {
            if (started)
                return;
            // The static constructor runs when the first instance is created or the first static member
            // is accessed (even if this method is empty).
            LoadSettings();
            EnsureTables();
        }
        static SettingController() {
            Start();
            // Properties.Settings.Default.PropertyChanged += SettingModel.PropertyChangedCallback_Event;
        }
        public static void LoadSettings()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sc"></param>
        /// <returns>Exception or null</returns>
        private static Exception TrySql(string sql, SQLiteConnection sc)
        {
            try
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, sc))
                    command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                // probably already exists
                return ex;
            }
            return null;
        }
        public static int IntParseOrDefault(string text, int defaultValue)
        {
            int result;
            bool ok = int.TryParse(text, out result);
            return ok ? result : defaultValue;
        }
        public static void EnsureTables()
        {
            // SettingModel.SqlConnectionString = Properties.Settings.Default.DbConnectionString;
            if (!File.Exists(SettingModel.DbFullPath))
            {
                SQLiteConnection.CreateFile(SettingModel.DbFullPath);
            }
            using (SQLiteConnection sc = new SQLiteConnection(SettingModel.SqlConnectionString))
            {
                // For a description of the fields, see Models.
                sc.Open();
                string sql;
                sql = "PRAGMA foreign_keys = ON;";
                TrySql(sql, sc);
                sql = ("create table Lod ("
                       + "LodId INT NOT NULL PRIMARY KEY"
                       + ", Name TEXT NOT NULL UNIQUE"
                       + ", ParentLodId INT UNIQUE" // top should be null
                       + ", SamplesPerMapblock INT NOT NULL"
                       + ");");
                TrySql(sql, sc);
                sql = ("create table Layer ("
                       + "LayerId INT NOT NULL PRIMARY KEY"
                       + ", Num INT NOT NULL UNIQUE"
                       + ", Name TEXT NOT NULL UNIQUE"
                       + ");");
                TrySql(sql, sc);
                sql = ("create table Region ("
                       + "RegionId INT NOT NULL PRIMARY KEY"
                       + ", Name TEXT NOT NULL"
                       + ", LodId INT NOT NULL"
                       + ");");
                TrySql(sql, sc);
                sql = ("create table Terrain ("
                       + "TerrainId INT NOT NULL PRIMARY KEY"
                       + ", Name TEXT NOT NULL UNIQUE"
                       + ", Classification TEXT"
                       + ", Variant TEXT"
                       + ", Path TEXT"
                       + ", SourceId INT"
                       + ", TileIndex INT"
                       + ", JSON TEXT"
                       + ", PixPerSample INT NOT NULL"
                       + ");");
                TrySql(sql, sc);
                sql = ("create table Mapblock ("
                       + "MapblockId INT NOT NULL PRIMARY KEY"
                       + ", LodId INT NOT NULL UNIQUE"
                       + ", LayerId INT NOT NULL UNIQUE"
                       + ", RegionId INT NOT NULL UNIQUE"
                       + ", TerrainId INT NOT NULL UNIQUE"
                       + ", Path TEXT"
                       // + ", Data BLOB" // TODO: (future) Data
                       + ", FOREIGN KEY (LodId)"
                       + "    REFERENCES Lod (LodId)"
                       + ", FOREIGN KEY (LayerId)"
                       + "    REFERENCES Layer (LayerId)"
                       + ", FOREIGN KEY (RegionId)"
                       + "    REFERENCES Region (RegionId)"
                       + ", FOREIGN KEY (TerrainId)"
                       + "    REFERENCES Terrain (TerrainId)"
                       + ");");
                TrySql(sql, sc);
                sc.Close();
            }

            using (MapDicerContext context = new MapDicerContext())
            {
            }
            
        }

    }
}

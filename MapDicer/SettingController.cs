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
        public const short LayerWhenOnly1 = 0;
        public static bool NewDB = true;
        private static string loadedCS = null;
        public static void Start()
        {
            if (started)
                return;
            started = true;
            // The static constructor runs when the first instance is created or the first static member
            // is accessed (even if this method is empty).
            InitializeDB();
        }
        public static void InitializeDB()
        {
            if (loadedCS != SettingModel.SqlConnectionString)
            {
                loadedCS = SettingModel.SqlConnectionString;
                NewDB = true;
                EnsureTables();
                EnsureSampleData();
            }
        }
        public static string DataPath {
            get
            {
                string DbPath = SettingModel.DbFullPath;
                FileInfo fi = new FileInfo(DbPath);
                string DbNoExt = Path.GetFileNameWithoutExtension(DbPath);
                string DbDirPath = fi.Directory.FullName;
                string DataDirName = DbNoExt + "_files";
                return Path.Combine(DbDirPath, DataDirName);
            }
        }


        /// <summary>
        /// Import the file to DataPath in the given category as a subfolder.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="category"></param>
        /// <param name="id">The new filename, or null to retain filename (extension from path is
        /// <param name="overwrite">Overwrite any existing file (at return path--see returns).</param>
        /// <returns>The new path relative to DataPath, without a leading slash.</returns>
        public static string Import(string path, string category, string id, bool overwrite)
        {
            category = category.Replace('\\', '/');
            string fileName = Path.GetFileName(path);
            if (id != null)
            {
                string dotExt = Path.GetExtension(path);
                fileName = id + dotExt;
            }
            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }
            string catPath = Path.Combine(DataPath, category);
            if (!Directory.Exists(catPath))
            {
                Directory.CreateDirectory(catPath);
            }
            // TODO: allow category to contain a slash
            string relPath = Path.Combine(category, fileName);
            string newPath = Path.Combine(DataPath, relPath);
            File.Copy(path, newPath, overwrite);
            return relPath;
        }
        static SettingController() {
            Start();
            // Properties.Settings.Default.PropertyChanged += SettingModel.PropertyChangedCallback_Event;
        }
        /// <summary>
        /// Try a sql command, otherwise return an exception.
        /// </summary>
        /// <param name="sql">SQLite-flavored SQL code</param>
        /// <param name="sc">The SQLiteConnection</param>
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
        private static void EnsureSampleData()
        {
            Layer layer = new Layer();
            layer.LayerId = SettingController.LayerWhenOnly1;
            layer.Name = "terrain";
            layer.Num = 0;
            try
            {
                Layer.Insert(layer);
            }
            catch
            {
                // TODO: this assumes it was already added and the problem was a primary key constraint violation
            }
        }
        private static void EnsureTables()
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
        }

    }
}

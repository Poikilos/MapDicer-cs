﻿using MapDicer.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        public static string MapblockImageDotExt
        {
            get
            {
                return ".png";
            }
        }
        /// <summary>
        /// This must be 96 for WPF or WPF will scale the WriteableBitmap (See
        /// <https://www.hanselman.com/blog/be-aware-of-dpi-with-image-pngs-in-wpf-images-scale-weird-or-are-blurry>)
        /// </summary>
        public static double DpiForNonViewableData
        {
            get
            {
                return 96;
            }
        }


        /// <summary>
        /// Import the file to DataPath in the given category as a subfolder.
        /// </summary>
        /// <param name="path">The original file to move</param>
        /// <param name="category">Use this subdirectory under DataPath.</param>
        /// <param name="id">The new filename, or null to retain filename (extension from path is
        /// <param name="overwrite">Overwrite any existing file (at return path--see returns).</param>
        /// <returns>The new path relative to DataPath, without a leading slash.</returns>
        public static string Import(string path, string category, string id, bool overwrite)
        {
            return Import(path, category, id, overwrite, false, true);
        }
        private static string Import(string path, string category, string id, bool overwrite, bool dryRun, bool makeDirs)
        {
            category = category.Replace('\\', '/');
            string fileName = null;
            if (id != null)
            {
                string dotExt = Path.GetExtension(path);
                // Remove invalid file name characters:
                string regex = String.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars())));
                Regex removeInvalidChars = new Regex(regex, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
                id = removeInvalidChars.Replace(id, "_");
                // ^ See Jan's answer on
                // <https://stackoverflow.com/questions/146134/how-to-remove-illegal-characters-from-path-and-filenames>
                fileName = id + dotExt;
            }
            else
            {
                if (path != null)
                {
                    fileName = Path.GetFileName(path);
                }
                else if (!dryRun)
                {
                    throw new ApplicationException("You cannot use a null path unless there is an id.");
                }
            }
            string catPath = Path.Combine(DataPath, category);
            if (makeDirs)
            {
                if (!Directory.Exists(DataPath))
                {
                    Directory.CreateDirectory(DataPath);
                }
                if (!Directory.Exists(catPath))
                {
                    Directory.CreateDirectory(catPath);
                }
            }
            // TODO: allow category to contain a slash
            string relPath = Path.Combine(category, fileName);
            string newPath = Path.Combine(DataPath, relPath);
            if (!dryRun) 
                File.Copy(path, newPath, overwrite);
            return relPath;
        }
        /// <summary>
        /// Get the path to which the file would be imported including the filename. See
        /// public static Import for documentation.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="category"></param>
        /// <param name="id"></param>
        /// <param name="makeDirs"></param>
        /// <returns></returns>
        public static string GetImportRelPath(string path, string category, string id, bool makeDirs)
        {
            return Import(path, category, id, false, true, makeDirs);
        }
        /// <summary>
        /// Get the path to which the file would be imported including the filename. See
        /// public static Import for documentation.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="category"></param>
        /// <param name="id"></param>
        /// <param name="makeDirs"></param>
        /// <returns></returns>
        public static string GetImportFullPath(string path, string category, string id, bool makeDirs)
        {
            return Path.Combine(DataPath, Import(path, category, id, false, true, makeDirs));
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
                       + ", LodId INT NOT NULL"
                       + ", LayerId INT NOT NULL"
                       + ", RegionId INT NOT NULL"
                       + ", TerrainId INT NOT NULL"
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

        /// <summary>
        /// Generate the block, but do not save an image nor write the entry to the database.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="regionId"></param>
        /// <param name="terrainId"></param>
        /// <returns></returns>
        internal static Mapblock GenerateBlock(MapDicerPos pos, long regionId, int terrainId)
        {
            string path = null;
            Mapblock mapblock = new Mapblock
            {
                MapblockId = pos.getSliceAsInteger(),
                LodId = pos.LodId,
                LayerId = pos.LayerId,
                RegionId = regionId,
                TerrainId = terrainId,
                Path = Mapblock.GetImagePath(pos, true),
            };
            return mapblock;
        }
    }
}

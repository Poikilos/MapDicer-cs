using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace MapDicer.Models
{
    class SettingModel
    {
        public const string NewIdStr = "(New)";
        public static string DbFullPath
        {
            get
            {
                return System.IO.Path.GetFullPath(Properties.Settings.Default.DbFile);
            }
        }
        public static string SqlConnectionString
        {
            get
            {
                string oldConnectionString = Properties.Settings.Default.DbConnectionString;
                if (oldConnectionString.Trim().Length == 0)
                {
                    SQLiteConnectionStringBuilder conString = new SQLiteConnectionStringBuilder();
                    string fullPath = DbFullPath;
                    conString.DataSource = fullPath;
                    // conString.ToFullPath = true; // not available
                    // conString.FullUri = (new System.Uri(fullPath)).AbsoluteUri;
                    // conString.Version = 3;
                    conString.ForeignKeys = true; // not available
                    conString.Flags |= SQLiteConnectionFlags.HidePassword; // Flags not available
                    // conString.Flags |= SQLiteConnectionFlags.LogAll;
                    // conString.Flags |= SQLiteConnectionFlags.UseConnectionPool;
                    // conString.Flags |= SQLiteConnectionFlags.UseConnectionTypes;
                    // conString.Pooling = true;
                    // conString.Source = ":memory:";
                    // conString.MaxPageCount = ;
                    // maxI = conString.ProgressOps;
                    // conString.PageSize = ;
                    // conString.Password = ;
                    // conString.VfsName = ;
                    conString.Add("ProviderName", "SQLite"); // not supported by EntityFrameworkCore
                    // ^ linq2db.SQLite (<https://github.com/linq2db/examples/blob/master/SQLite/GetStarted/LinqToDB.Templates/CopyMe.SQLite.tt.txt>)
                    conString.Add("Database", "MapDicer");
                    Properties.Settings.Default.DbConnectionString = conString.ConnectionString;
                    Properties.Settings.Default.Save();
                }
                return Properties.Settings.Default.DbConnectionString;
            }
        }
        private static short maxLayerCount = 128;
        /// <summary>
        /// Get the number of layers per level of detail (also determines y skip to next LOD).
        /// Y is the height axis to match Minetest (each realm is MaxLayerCount apart here).
        /// </summary>
        public static short MaxLayerCount
        {
            get
            {
                return maxLayerCount;
            }
        }
        static SettingModel() {
        }
        /*
        private static void PropertyChangedCallback_Event(object sender, PropertyChangedEventArgs e)
        {
            MessageBox.Show(sender.GetType().Name);
        }
        */
    }
}

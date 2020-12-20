using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Data.SQLite;

namespace MapDicer.Models
{
    class SettingModel
    {
        public const string NewIdStr = "(New)";
        public static string SqlConnectionString
        {
            get
            {
                string oldConnectionString = Properties.Settings.Default.DbConnectionString;
                if (oldConnectionString.Trim().Length == 0)
                {
                    SQLiteConnectionStringBuilder conString = new SQLiteConnectionStringBuilder();
                    string fullPath = System.IO.Path.GetFullPath(Properties.Settings.Default.DbFile);
                    conString.DataSource = fullPath;
                    // conString.ToFullPath = true; // not available
                    // conString.FullUri = (new System.Uri(fullPath)).AbsoluteUri;
                    // conString.Version = 3;
                    // conString.ForeignKeys = true; // not available
                    // conString.Flags |= SQLiteConnectionFlags.HidePassword; // Flags not available
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
        public static void LoadSettings()
        {
            // SettingModel.SqlConnectionString = Properties.Settings.Default.DbConnectionString;
        }
        static SettingModel() {
            LoadSettings();
            // Properties.Settings.Default.PropertyChanged += SettingModel.PropertyChangedCallback_Event;
            // ^ never happens
        }
        /*
        private static void PropertyChangedCallback_Event(object sender, PropertyChangedEventArgs e)
        {
            MessageBox.Show(sender.GetType().Name);
        }
        */
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
        public static Color ColorFromId(int id)
        {
            // A byte overflow will occur on blue if the id is out of range.
            return Color.FromArgb(255, (byte)(id & 0xFF), (byte)((id >> 8) & 0xFF), (byte)(id >> 16));
        }
    }
}

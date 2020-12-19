using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace MapDicer
{
    class SettingModel
    {
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

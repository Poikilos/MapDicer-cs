using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDicer
{
    /// <summary>
    /// It must be a button to be a combo box Item.
    /// </summary>
    class Terrain : Windows.UI.Xaml.Controls.Button
    {
        public byte R;
        public byte G;
        public byte B;
        // inherits Text from Button
        public Terrain(string Content, double R, double G, double B)
        {
            this.R = (byte)R;
            this.G = (byte)G;
            this.B = (byte)B;
            this.Content = Content;
        }
    }
}

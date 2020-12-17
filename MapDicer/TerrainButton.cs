using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDicer
{
    /// <summary>
    /// It must be a button to be a combo box Item.
    /// </summary>
    class TerrainButton : System.Windows.Controls.Button
    {
        public const string newItemContent = "(Add New)";

        public byte R;
        public byte G;
        public byte B;
        // inherits Text from Button
        public TerrainButton(string Content, double R, double G, double B)
        {
            this.R = (byte)R;
            this.G = (byte)G;
            this.B = (byte)B;
            this.Content = Content;
            this.Loaded += Terrain_Loaded;
        }

        private void Terrain_Loaded(object sender, EventArgs e)
        {
            if ((string)Content != newItemContent)
            {
                // this.BorderBrush.Opacity = 0;
                // this.Background.Opacity = 0;
                // ^ in WPF (not UWP): "System.InvalidOperationException: 'Cannot set a property
                //   on object...because it is in a read-only state.'" where "..." is an address.
            }
        }
        
    }
}

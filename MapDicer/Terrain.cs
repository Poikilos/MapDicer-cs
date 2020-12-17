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
        public const string newItemContent = "(Add New)";

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
            this.Loaded += Terrain_Loaded;
        }

        private void Terrain_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if ((string)Content != newItemContent)
            {
                this.BorderBrush.Opacity = 0;
                this.Background.Opacity = 0;
            }
        }
        
    }
}

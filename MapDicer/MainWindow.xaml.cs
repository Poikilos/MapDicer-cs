using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapDicer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int touchSize = 16;
        public static MainWindow thisMP = null;

        public static Ellipse brushColorShape = new Ellipse();
        public static double prefillTerrainRed = 128;
        public static double prefillTerrainGreen = 128;
        public static double prefillTerrainBlue = 128;
        private void afterBrushSize()
        {
        }
        private void afterSize(bool InitializeBrush)
        {
            this.touchSize = Math.Max((int)this.Height / 800, 1) * 16;
            this.terrainBrushSizeSlider.Minimum = 1;
            this.terrainBrushSizeSlider.Maximum = touchSize * 8;
            if (InitializeBrush)
            {
                MainWindow.brushColorShape.Width = touchSize * 2;
            }
            // Always set H since W changes elsewhere:
            MainWindow.brushColorShape.Height = MainWindow.brushColorShape.Width;
            if (InitializeBrush)
            {
                this.terrainBrushSizeSlider.Value = MainWindow.brushColorShape.Width;
            }
            this.afterBrushSize();
        }
        public MainWindow()
        {
            InitializeComponent();
            this.afterSize(true);
            MainWindow.thisMP = this;
            Terrain newTerrain = new Terrain(Terrain.newItemContent, 0, 0, 0);
            newTerrain.Click += NewTerrain_Click;
            this.terrainCBx.Items.Add(newTerrain);
            // Button: MessageDialog dialog = new MessageDialog((this.brushTerrainCB.Items[0]).GetType().ToString());
            // var result = dialog.ShowAsync();
            // this.brushTerrainCB.Items.Add(typeof this.brushTerrainCB.Items[0]);
        }
        private void NewTerrain_Click(object sender, RoutedEventArgs e)
        {
            goToAddTerrain();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.afterSize(true);
        }

        private void terrainBrushSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.brushColorShape.Width = this.terrainBrushSizeSlider.Value;
            this.afterSize(false);
        }

        private void AddTerrain(string name, double r, double g, double b)
        {
            MainWindow.brushColorShape.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)r, (byte)g, (byte)b));
            this.terrainCBx.Items.Add(new Terrain(name, r, g, b));
            for (int i = 0; i < this.terrainCBx.Items.Count; i++)
            {
                if ((string)((Terrain)this.terrainCBx.Items[i]).Content == name)
                {
                    this.terrainCBx.SelectedIndex = i;
                    break;
                }
            }

        }

        private void brushTerrainCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((this.terrainCBx.SelectedItem != null) && (((Terrain)this.terrainCBx.SelectedItem).Content.Equals(Terrain.newItemContent)))
            {
                try
                {
                    goToAddTerrain();
                }
                catch (System.ArgumentException ex)
                {
                    // The system gave us the page, not the add terrain page.
                }
            }
        }

        private void goToAddTerrain()
        {
            // this.Frame.Navigate(typeof(NewTerrainPage), null);
            // ^ Don't do that, it generates a new page.
            NewTerrainWindow contentDialog = new NewTerrainWindow();
            contentDialog.prefill(prefillTerrainRed, prefillTerrainGreen, prefillTerrainBlue);
            // ^ widgets are null at this point apparently
            var result = contentDialog.ShowDialog();
            prefillTerrainRed = contentDialog.Red;
            prefillTerrainGreen = contentDialog.Green;
            prefillTerrainBlue = contentDialog.Blue;
            if (result == true)
            {
                string textTrim = contentDialog.TerrainName;
                if (textTrim.Length > 0)
                {
                    AddTerrain(textTrim, contentDialog.Red, contentDialog.Green, contentDialog.Blue);
                }
                else
                {
                    MessageBox.Show("You must give the terrain a name.");
                }
            }
            else if (result == false)
            {
                MessageBox.Show("You cancelled adding the terrain.");
            }
            else
            {
                MessageBox.Show("The result of the dialog was null.");
            }
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

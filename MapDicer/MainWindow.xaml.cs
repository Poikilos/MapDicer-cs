using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MapDicer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int touchSize = 16;
        public static MainPage thisMP = null;
        
        public static Ellipse brushColorShape = new Ellipse();
        public static double prefillTerrainRed = 128;
        public static double prefillTerrainGreen = 128;
        public static double prefillTerrainBlue = 128;
        private void afterBrushSize()
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        private void afterSize(bool InitializeBrush)
        {
            this.touchSize = Math.Max((int)this.Height / 800, 1) * 16;
            this.terrainBrushSizeSlider.Minimum = 1;
            this.terrainBrushSizeSlider.Maximum = touchSize * 8;
            if (InitializeBrush)
            {
                MainPage.brushColorShape.Width = touchSize * 2;
            }
            // Always set H since W changes elsewhere:
            MainPage.brushColorShape.Height = MainPage.brushColorShape.Width;
            if (InitializeBrush) { 
                this.terrainBrushSizeSlider.Value = MainPage.brushColorShape.Width;
            }
            this.afterBrushSize();
        }
        public MainPage()
        {
            this.InitializeComponent();
            this.afterSize(true);
            MainPage.thisMP = this;
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

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.afterSize(true);
        }

        private void brushSizeSlider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
        }

        private void brushSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            MainPage.brushColorShape.Width = this.terrainBrushSizeSlider.Value;
            this.afterSize(false);

        }

        private void AddTerrain(string name, double r, double g, double b)
        {
            MainPage.brushColorShape.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, (byte)r, (byte)g, (byte)b));
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

        private async void goToAddTerrain()
        {
            // this.Frame.Navigate(typeof(NewTerrainPage), null);
            // ^ Don't do that, it generates a new page.
            NewTerrainContentDialog contentDialog = new NewTerrainContentDialog();
            contentDialog.prefill(prefillTerrainRed, prefillTerrainGreen, prefillTerrainBlue);
            // ^ widgets are null at this point apparently
            var result = await contentDialog.ShowAsync();
            prefillTerrainRed = contentDialog.Red;
            prefillTerrainGreen = contentDialog.Green;
            prefillTerrainBlue = contentDialog.Blue;
            if (result == ContentDialogResult.Primary)
            {
                string textTrim = contentDialog.TerrainName;
                if (textTrim.Length > 0)
                {
                    AddTerrain(textTrim, contentDialog.Red, contentDialog.Green, contentDialog.Blue);
                }
                else
                {
                    MessageDialog dialog = new MessageDialog("You must give the terrain a name.");
                    var result2 = dialog.ShowAsync();
                }
            }
            else if (result == ContentDialogResult.Secondary)
            {
            }
            else
            {
            }
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

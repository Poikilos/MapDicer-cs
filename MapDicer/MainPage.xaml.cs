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
        private const string newItemContent = "(Add New)";
        public static Ellipse brushColorShape = new Ellipse();
        private void afterBrushSize()
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // See https://stackoverflow.com/a/34832454 
            // CC BY-SA 3.0 Vadim Martynov
            try
            {
                this.terrainCBx.Items.Add(e.Parameter as Terrain);
            }
            catch (System.ArgumentException ex)
            {
                // The system gave us the page, not the add terrain page.
            }
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
            /*

            this.brushSizeImage.Width = this.touchSize * 2;
            this.brushSizeImage.Height = this.brushSizeImage.Width;
            */
            /*
            this.mainSP.Width = this.Width;
            this.menusBarSP.Width = this.mainSP.Width;
            this.menusBarSP.Height = touchSize * 4;
            this.brushParamSlider.Minimum = 0; //
            this.brushParamSlider.Maximum = 65535;
            double slackW = this.menusBarSP.Width - this.brushColorShape.Width;
            this.navSP.Width = slackW;
            this.navSP.Height = this.touchSize * 2;
            this.selectSP.Width = slackW;
            this.selectSP.Height = this.touchSize * 2; // detail, region, block, settings
            this.settingsButton.Width = this.touchSize * 6;
            double selectW = this.selectSP.Width;
            double selectSlackW = selectW - this.settingsButton.Width;
            this.detailCB.Width = selectSlackW * .33;
            this.detailCB.Height = this.settingsButton.Height;
            this.regionCB.Width = selectSlackW * .34;
            this.regionCB.Height = this.detailCB.Height;
            this.blockCB.Width = selectSlackW * .33;
            this.blockCB.Height = this.detailCB.Height;
            this.settingsButton.Height = this.touchSize * 2;
            this.settingsButton.FontSize = touchSize;
            this.brushParamImage.Width = this.brushSizeImage.Width;
            this.brushParamImage.Height = this.brushParamImage.Width;
            double slackWithoutTwoW = slackW - this.brushSizeImage.Width - this.brushParamImage.Width;
            this.brushTerrainCB.Width = Math.Round((double)slackWithoutTwoW * .34);
            this.brushTerrainCB.Height = touchSize * 2;
            this.brushParamSlider.Width = Math.Round((double)slackWithoutTwoW * .33);
            this.brushParamSlider.Height = this.brushTerrainCB.Height;
            this.brushSizeSlider.Width = Math.Round((double)slackWithoutTwoW * .33);
            this.brushSizeSlider.Height = this.brushTerrainCB.Height;
            */

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
            Terrain newTerrain = new Terrain(newItemContent, 0, 0, 0);
            newTerrain.Click += NewTerrain_Click;
            this.terrainCBx.Items.Add(newTerrain);
            // Button: MessageDialog dialog = new MessageDialog((this.brushTerrainCB.Items[0]).GetType().ToString());
            // var result = dialog.ShowAsync();

            //this.brushTerrainCB.Items.Add(typeof this.brushTerrainCB.Items[0]);

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

        internal static void AddTerrain(string name, double r, double g, double b)
        {
            MainPage.brushColorShape.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, (byte)r, (byte)g, (byte)b));
            thisMP.terrainCBx.Items.Add(new Terrain("name", r, g, b));
        }

        private void settingsImage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage), null);
        }

        private void brushTerrainCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((this.terrainCBx.SelectedItem != null) && (((Terrain)this.terrainCBx.SelectedItem).Content.Equals(newItemContent)))
            {
                goToAddTerrain();
            }
        }

        private void goToAddTerrain()
        {
            // this.Frame.Navigate(typeof(NewTerrainPage), null);
            // ^ Don't do that, it generates a new page.
            // this.mainPage.NavigationService.Navigate(new Uri("/Views/Page.xaml?parameter=test", UriKind.Relative));
            // ^ CC BY-SA 3.0 https://creativecommons.org/licenses/by-sa/3.0/ Daniel Little on https://stackoverflow.com/a/12444817/4541104
        }
    }
}

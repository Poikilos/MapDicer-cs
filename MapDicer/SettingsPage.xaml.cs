using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MapDicer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private int touchSize = 16;
        
        private void afterBrushSize()
        {
            // this.menusSP.Width = this.menusBarSP.Width - this.brushColorShape.Width;
        }
        private void afterSize(bool InitializeBrush)
        {
            
            this.touchSize = Math.Max((int)this.Height / 800, 1) * 16;
            /*
            this.mainSP.Width = this.Width;
            this.menusBarSP.Width = this.mainSP.Width;
            this.menusBarSP.Height = touchSize * 4;
            this.brushParamSlider.Minimum = 0;
            this.brushParamSlider.Maximum = 65535;
            this.brushSizeSlider.Minimum = 1;
            this.brushSizeSlider.Maximum = touchSize * 4;
            if (InitializeBrush) {
                this.brushColorShape.Width = touchSize * 2;
            }
            // Always set H since W changes elsewhere:
            this.brushColorShape.Height = this.brushColorShape.Width;
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
            this.brushSizeImage.Width = this.touchSize * 2;
            this.brushSizeImage.Height = this.brushSizeImage.Width;
            this.brushParamImage.Width = this.brushSizeImage.Width;
            this.brushParamImage.Height = this.brushParamImage.Width;
            double slackWithoutTwoW = slackW - this.brushSizeImage.Width - this.brushParamImage.Width;
            this.brushTerrainCB.Width = Math.Round((double)slackWithoutTwoW * .34);
            this.brushTerrainCB.Height = touchSize * 2;
            this.brushParamSlider.Width = Math.Round((double)slackWithoutTwoW * .33);
            this.brushParamSlider.Height = this.brushTerrainCB.Height;
            this.brushSizeSlider.Width = Math.Round((double)slackWithoutTwoW * .33);
            this.brushSizeSlider.Height = this.brushTerrainCB.Height;
            if (InitializeBrush) { 
                this.brushSizeSlider.Value = this.brushColorShape.Width;
            }
            this.afterBrushSize();
            */
        }
        public SettingsPage()
        {
            this.InitializeComponent();
            this.afterSize(true);
        }

        private void SettingsPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.afterSize(true);
        }

        private void settingsMainPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }
        /*
       private void brushSizeSlider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
       {
       }

       private void brushSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
       {
           this.brushColorShape.Width = this.brushSizeSlider.Value;
           this.afterSize(false);

       }
       */
    }
}

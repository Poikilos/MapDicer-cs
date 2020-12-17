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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MapDicer
{
    public sealed partial class NewTerrainContentDialog : ContentDialog
    {
        public static double prefillTerrainRed = 128;
        public static double prefillTerrainGreen = 128;
        public static double prefillTerrainBlue = 128;
        private static TextBox[] colorTBs = null;
        private static Slider[] colorSliders = null;
        public int Red {
            get
            {
                return (int)Math.Round(this.redSlider.Value);
            }
            set
            {
                this.redSlider.Value = (double)value;
            }
        }
        public int Green {
            get
            {
                return (int)Math.Round(this.greenSlider.Value);
            }
            set
            {
                this.greenSlider.Value = (double)value;
            }
        }
        public int Blue {
            get
            {
                return (int)Math.Round(this.blueSlider.Value);
            }
            set
            {
                this.blueSlider.Value = (double)value;
            }
        }
        public string TerrainName { get; private set; }
        private bool suppressToText = false; // suppress transfer of slider to text while doing the opposite
        private bool suppressToSlider = false; // suppress transfer of text to slider while doing the opposite
        // ^ See <https://docs.microsoft.com/en-us/windows/winui/api/microsoft.ui.xaml.controls.
        //   contentdialog?view=winui-3.0-preview>
        public NewTerrainContentDialog()
        {
            this.InitializeComponent();
            colorTBs = new TextBox[3];
            colorSliders = new Slider[3];
            colorTBs[0] = this.redTB;
            colorTBs[1] = this.greenTB;
            colorTBs[2] = this.blueTB;
            colorSliders[0] = this.redSlider;
            colorSliders[1] = this.greenSlider;
            colorSliders[2] = this.blueSlider;
        }
        
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Red = (int)Math.Round(this.redSlider.Value);
            Green = (int)Math.Round(this.greenSlider.Value);
            Blue = (int)Math.Round(this.blueSlider.Value);
            TerrainName = this.nameTB.Text;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Red = (int)Math.Round(this.redSlider.Value);
            Green = (int)Math.Round(this.greenSlider.Value);
            Blue = (int)Math.Round(this.blueSlider.Value);
            TerrainName = this.nameTB.Text;
        }

        private void ContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            this.redSlider.Value = prefillTerrainRed;
            this.greenSlider.Value = prefillTerrainGreen;
            this.blueSlider.Value = prefillTerrainBlue;
        }
        public void prefill(double r, double g, double b)
        {
            // The sliders are null before Opened, apparently, so set properties instead:
            prefillTerrainRed = r;
            prefillTerrainGreen = g;
            prefillTerrainBlue = b;
        }

        private void afterSliderChanged(int index)
        {
            // Copy the slider's value to the text box.
            if (suppressToText) return;
            // ^ doesn't seem to be necessary in UWP somehow, but try to prevent a feedback look anyway.
            if (colorTBs == null) return;
            if (colorTBs[index] == null) return; // It is null on open, but this event runs.
            suppressToSlider = true;
            colorTBs[index].Text = ((int)Math.Round(colorSliders[index].Value)).ToString();
            suppressToSlider = false;
            updateColor();
        }

        private void redSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            afterSliderChanged(0);
        }

        private void greenSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            afterSliderChanged(1);
        }

        private void blueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            afterSliderChanged(2);
        }

        private void afterTextBoxChanged(int index)
        {
            // Update the slider to match the Text Box, given the format and range is correct,
            // otherwise revert the typed text.
            if (suppressToSlider) return;
            // ^ doesn't seem to be necessary in UWP somehow, but try to prevent a feedback look anyway.
            suppressToText = true;
            try
            {
                int value = int.Parse(colorTBs[index].Text);
                if (value > 255)
                {
                    value = 255;
                    suppressToSlider = true;
                    colorTBs[index].Text = value.ToString();
                    suppressToSlider = false;
                }
                colorSliders[index].Value = (double)value;
            }
            catch (Exception ex)
            {
                // bad input, remove it
                if (colorTBs[index].Text.Length > 0)
                {
                    suppressToSlider = true;
                    colorTBs[index].Text = redTB.Text.Substring(0, colorTBs[index].Text.Length - 1);
                    suppressToSlider = false;
                }
                else
                {
                    colorTBs[index].Text = "";
                }
            }
            suppressToText = false;
            updateColor();
        }

        private void redTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            afterTextBoxChanged(0);
        }

        private void greenTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            afterTextBoxChanged(1);
        }
        private void blueTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            afterTextBoxChanged(2);
        }
        void updateColor()
        {
            if (this.redSlider == null) return;
            if (this.greenSlider == null) return;
            if (this.blueSlider == null) return;
            // ^ null before initialized (Red, Green, and Blue ty to use the values).
            if (this.terrainColorEllipse == null) return;
            // ^ null before initialized
            this.terrainColorEllipse.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, (byte)Red, (byte)Green, (byte)Blue));
        }
    }
}

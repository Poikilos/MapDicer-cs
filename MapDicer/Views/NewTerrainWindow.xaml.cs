using MapDicer.Models;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace MapDicer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class NewTerrainWindow : Window
    {
        /*
        public enum Result
        {
            Primary,
            Secondary
        }
        */
        // public Result DialogResult = Result.Primary;
        private double prefillTerrainRed = 128;
        private double prefillTerrainGreen = 128;
        private double prefillTerrainBlue = 128;
        private int prefillSourceId = 1;
        private int prefillPPS = 1;
        private static TextBox[] colorTBs = null;
        private static Slider[] colorSliders = null;
        private bool suppressToText = false; // suppress transfer of slider to text while doing the opposite
        private bool suppressToSlider = false; // suppress transfer of text to slider while doing the opposite

        public BitmapImage Image = null;
        private Terrain terrain = null;
        public Terrain Terrain
        {
            get
            {
                return terrain;
            }
            set
            {
                terrain = value;
            }
        }
        public int Red
        {
            get
            {
                return (int)Math.Round(this.redSlider.Value);
            }
            set
            {
                this.redSlider.Value = (double)value;
            }
        }
        public int Green
        {
            get
            {
                return (int)Math.Round(this.greenSlider.Value);
            }
            set
            {
                this.greenSlider.Value = (double)value;
            }
        }
        public int Blue
        {
            get
            {
                return (int)Math.Round(this.blueSlider.Value);
            }
            set
            {
                this.blueSlider.Value = (double)value;
            }
        }

        public NewTerrainWindow()
        {
            InitializeComponent();
            colorTBs = new TextBox[3];
            colorSliders = new Slider[3];
            colorTBs[0] = this.redTB;
            colorTBs[1] = this.greenTB;
            colorTBs[2] = this.blueTB;
            colorSliders[0] = this.redSlider;
            colorSliders[1] = this.greenSlider;
            colorSliders[2] = this.blueSlider;
            this.Terrain = null;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            Red = (int)Math.Round(this.redSlider.Value);
            Green = (int)Math.Round(this.greenSlider.Value);
            Blue = (int)Math.Round(this.blueSlider.Value);
            if (this.nameTB.Text.Trim().Length < 1)
            {
                MessageBox.Show("You must enter a name.");
                return;
            }
            int pps;
            bool ok = int.TryParse(this.ppsTB.Text.Trim(), out pps);
            if (!ok)
            {
                MessageBox.Show("PPS must be a whole number.");
                return;
            }
            int? sourceId = null;
            if (this.sourceTB.Text.Trim().Length > 0)
            {
                int tmpId;
                ok = int.TryParse(this.sourceTB.Text.Trim(), out tmpId);
                if (!ok)
                {
                    MessageBox.Show("The SourceId must be a whole number.");
                    return;
                }
                else
                {
                    sourceId = tmpId;
                }
            }
            try
            {
                this.Image = new BitmapImage(new System.Uri(this.pathTB.Text));
            }
            catch (System.UriFormatException ex)
            {
                MessageBox.Show("The path is not valid.");
                return;
            }
            catch (System.NotSupportedException ex)
            {
                MessageBox.Show("You must choose a supported image format such as jpg or png.");
                return;
            }
            this.Terrain = new Terrain
            {
                TerrainId = Terrain.IdFromHexColorRgb(this.hexTB.Text),
                Name = this.nameTB.Text,
                Path = SettingController.Import(this.pathTB.Text, "terrain", null, true),
                SourceId = sourceId,
                PixPerSample = pps,
            };
            this.DialogResult = true;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Red = (int)Math.Round(this.redSlider.Value);
            Green = (int)Math.Round(this.greenSlider.Value);
            Blue = (int)Math.Round(this.blueSlider.Value);
            this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.redSlider.Value = prefillTerrainRed;
            this.greenSlider.Value = prefillTerrainGreen;
            this.blueSlider.Value = prefillTerrainBlue;
            this.ppsTB.Text = prefillPPS.ToString();
            this.sourceTB.Text = prefillSourceId.ToString();
        }

        /// <summary>
        /// Set prefill values before showing the form.
        /// </summary>
        /// <param name="r">Red (0-255)</param>
        /// <param name="g">Green (0-255)</param>
        /// <param name="b">Blue (0-255)</param>
        /// <param name="pps">Pixels Per Sample (32 means a 42x42 image will be larger than one
        /// tile)</param>
        /// <param name="sourceId">This index represents a source for credit (citation) purposes.</param>
        public void prefill(double r, double g, double b, int pps, int sourceId)
        {
            // The sliders are null before Opened, apparently, so set properties instead:
            prefillTerrainRed = r;
            prefillTerrainGreen = g;
            prefillTerrainBlue = b;
            prefillPPS = pps;
            prefillSourceId = sourceId;
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

        private void redSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            afterSliderChanged(0);
        }
        private void greenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            afterSliderChanged(1);
        }
        private void blueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            afterSliderChanged(2);
        }
        private void afterTextBoxChanged(int index)
        {
            // Update the slider to match the Text Box, given the format and range is correct,
            // otherwise revert the typed text.
            if (suppressToSlider) return;
            // ^ doesn't seem to be necessary in UWP somehow, but try to prevent a feedback look anyway.
            if (colorTBs == null) return;
            // ^ null on load.
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
            this.terrainColorEllipse.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)Red, (byte)Green, (byte)Blue));
            this.hexTB.Text = Terrain.HexRgbFromColor((byte)Red, (byte)Green, (byte)Blue);
        }

        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            var result = dlg.ShowDialog();
            if (result == true)
            {
                this.pathTB.Text = dlg.FileName;
            }
        }

        private void nameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            int prevCaretIndex = this.nameTB.CaretIndex;
            this.nameTB.Text = this.nameTB.Text.ToLower();
            this.nameTB.CaretIndex = prevCaretIndex;
        }

        /*
         * doesn't work (neither does terrainColorGrid_SizeChanged or anything: See Window SizeToContent
         * in XAML)
        private void mainSP_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double calcW = mainSP.ActualWidth;
            double calcH = mainSP.ActualHeight;
            if (this.Width < calcW) this.Width = calcW;
            if (this.Height < calcH) this.Height = calcH;
        }
        */

    }
}

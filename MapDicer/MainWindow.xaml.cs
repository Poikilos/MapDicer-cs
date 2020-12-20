using MapDicer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool suppressNewWindow = false;
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

            // TerrainButton newTerrain = new TerrainButton(Terrain.newItemContent, 0, 0, 0);
            // newTerrain.Click += terrainBtn_Click;
            // this.terrainCBx.Items.Add(newTerrain);
            // this.suppressNewWindow = true;
            // this.terrainCBx.SelectedIndex = 0;
            // this.suppressNewWindow = false;

            // Button: MessageDialog dialog = new MessageDialog((this.brushTerrainCB.Items[0]).GetType().ToString());
            // var result = dialog.ShowAsync();
            // this.brushTerrainCB.Items.Add(typeof this.brushTerrainCB.Items[0]);
            try
            {
                this.terrainImage.Source = new BitmapImage(
                    new Uri(System.IO.Path.Combine(System.Environment.CurrentDirectory, "Assets", "terrain.png"))
                );
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }
        private void terrainColorBtn_Click(object sender, RoutedEventArgs e)
        {
            // goToAddTerrain();
        }
        private void terrainBtn_Click(object sender, RoutedEventArgs e)
        {

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
            this.terrainCBx.Items.Add(new TerrainButton(name, r, g, b));
            for (int i = 0; i < this.terrainCBx.Items.Count; i++)
            {
                if ((string)((TerrainButton)this.terrainCBx.Items[i]).Content == name)
                {
                    this.terrainCBx.SelectedIndex = i;
                    break;
                }
            }

        }

        private void brushTerrainCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((this.terrainCBx.SelectedItem != null) && (((TerrainButton)this.terrainCBx.SelectedItem).Content.Equals(SettingModel.NewIdStr)))
            {
                try
                {
                    if (!suppressNewWindow)
                    {
                        goToAddTerrain();
                    }
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
                // MessageBox.Show("You cancelled adding the terrain.");
            }
            else
            {
                MessageBox.Show("The result of the dialog was null.");
            }
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.settingsCM.IsOpen = true;
        }

        private void settingsEditorSettingsMI_Click(object sender, RoutedEventArgs e)
        {
            EditorSettingsWindow dlg = new EditorSettingsWindow();
            var result = dlg.ShowDialog();
            if (result == true)
            {
                // Do nothing, it saves on its own.
            }
        }

        private void settingsLayersMI_Click(object sender, RoutedEventArgs e)
        {

        }

        private void terrainPreviewCnv_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void detailBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingController.Start();
            LodWindow dlg = new LodWindow();
            var result = dlg.ShowDialog();
            if (result == true)
            {
                // Do nothing, it saves on its own.
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // TimerCallback tc = new TimerCallback()
            // Timer t = new Timer();
        }
    }
}

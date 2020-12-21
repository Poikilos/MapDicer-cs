using MapDicer.Models;
using MapDicer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

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
        private const int InitStateNone = 0;
        private const int InitStateDb = 1;
        private const int InitStateFull = 2;
        private int initState = 0;
        private ViewModel viewModel = null;
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
        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            this.viewModel = new ViewModel();
            this.viewModel.Parent = this;
            this.DataContext = this.viewModel;
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
            try
            {
                this.image.Source = new BitmapImage(
                    new Uri(System.IO.Path.Combine(System.Environment.CurrentDirectory, "Assets", "splash.png"))
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
            if (!suppressNewWindow)
            {
                goToAddTerrain();
            }
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
        /*
        private void AddTerrain(Terrain terrain, double r, double g, double b)
        {
        }
        */
        /*
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
        */
        public void ShowTerrain(Terrain terrain)
        {
            if ((terrain != null))
            {

                MainWindow.brushColorShape.Fill = new SolidColorBrush(terrain.GetColor());
                this.terrainColorEllipse.Fill = MainWindow.brushColorShape.Fill;
                try
                {
                    this.terrainImage.Source = new BitmapImage(
                        new Uri(System.IO.Path.Combine(SettingController.DataPath, terrain.Path))
                    );
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    MessageBox.Show(
                        String.Format("{0} is missing or inaccessible.", terrain.Path),
                        "File Error"
                    );
                    try
                    {
                        this.terrainImage.Source = new BitmapImage(
                            new Uri(System.IO.Path.Combine(System.Environment.CurrentDirectory, "Assets", "terrain.png"))
                        );
                    }
                    catch (System.IO.FileNotFoundException innerEx)
                    {
                        Console.Error.WriteLine(innerEx.ToString());
                    }
                }
                /*
                if (((TerrainButton)this.terrainCBx.SelectedItem).Content.Equals(SettingModel.NewIdStr)) {
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
                */
            }
            else
            {
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
        }
        private void brushTerrainCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowTerrain((Terrain)this.terrainCBx.SelectedItem);
        }

        private void goToAddTerrain()
        {
            // this.Frame.Navigate(typeof(NewTerrainPage), null);
            // ^ Don't do that, it generates a new page.
            NewTerrainWindow dlg = new NewTerrainWindow();
            dlg.prefill(prefillTerrainRed, prefillTerrainGreen, prefillTerrainBlue, 32, 1);
            // ^ widgets are null at this point apparently
            var result = dlg.ShowDialog();
            prefillTerrainRed = dlg.Red;
            prefillTerrainGreen = dlg.Green;
            prefillTerrainBlue = dlg.Blue;
            if (result == true)
            {
                // string textTrim = contentDialog.TerrainName;

                // string textTrim = contentDialog.Terrain.Name;
                // if (textTrim.Length > 0)
                if (dlg.NewEntry != null)
                {
                    this.viewModel.Terrains.Add(dlg.NewEntry);
                    this.terrainCBx.SelectedIndex = this.viewModel.Terrains.Count - 1;

                    // AddTerrain(dlg.Terrain, dlg.Red, dlg.Green, dlg.Blue);
                    dlg.NewEntry = null;
                }
                else
                {
                    MessageBox.Show("The New Terrain was not valid.");
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
                // saved to database already in this case, so:
                if (dlg.NewEntry != null)
                {
                    this.viewModel.Lods.Add(dlg.NewEntry);
                    dlg.NewEntry = null;
                }
                if (dlg.ChangedEntry != null)
                {
                    int found = -1;
                    // foreach (Lod lod in this.viewModel.Lods)
                    for (int i = 0; i < this.viewModel.Lods.Count; i++)
                    {
                        if (this.viewModel.Lods[i].Id == dlg.ChangedEntry.Id)
                        {
                            // this.viewModel.Lods.Add(dlg.ChangedEntry);
                            // lod.Name = dlg.ChangedEntry.Name;
                            // dlg.ChangedEntry = null;
                            found = i;
                            this.viewModel.Lods.RemoveAt(i);
                            break;
                        }
                    }
                    if (found > -1)
                    {
                        this.viewModel.Lods.Insert(found, dlg.ChangedEntry);
                        dlg.ChangedEntry = null;
                    }
                    if (dlg.ChangedEntry != null) {
                        MessageBox.Show("The changed entry wasn't in the list.");
                        dlg.ChangedEntry = null;
                    }
                }
            }
        }

        public void Enable(bool enable)
        {
            //Here update your label, button or any string related object.

            //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));    
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate {
                // skeletonImage.Visibility = enable ? Visibility.Hidden : Visibility.Visible; // hide skeleton when enabled
                this.menuGrid.Visibility = enable ? Visibility.Visible : Visibility.Hidden;
                this.detailCBx.IsEnabled = enable;
                this.regionCBx.IsEnabled = enable;
                this.blockCBx.IsEnabled = enable;
                this.terrainCBx.IsEnabled = enable;

                this.detailBtn.IsEnabled = enable;
                this.regionBtn.IsEnabled = enable;
                this.mapblockBtn.IsEnabled = enable;
                this.terrainBtn.IsEnabled = enable;
            }));
        }

        private void LoadLodsSafe(bool reloadIds)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate {
                if (reloadIds)
                {
                    this.viewModel.Lods.Clear();
                    this.viewModel.SelectedLod = null;
                    foreach (Lod entry in Lod.All())
                    {
                        this.viewModel.Lods.Add(entry);
                    }

                    this.viewModel.Terrains.Clear();
                    this.viewModel.SelectedTerrain = null;
                    foreach (Terrain entry in Terrain.All())
                    {
                        this.viewModel.Terrains.Add(entry);
                    }

                    // The timer will fill in everything dependent upon selected Lod.

                    if (initState < InitStateDb)
                    {
                        initState = InitStateDb;
                        dispatcherTimer.Start();
                    }
                }
            }));
            // rhatwar007 https://stackoverflow.com/a/24624095/4541104
        }

        private void LoadState(object o)
        {
            ReloadState((bool)o);
        }
        private void ReloadState(bool reloadIds)
        {
            Enable(false);

            //Enable(false);
            // LodId // short
            // Name // string
            // ParentLodId // short
            // UnitsPerSample // long; calculated
            // SamplesPerMapblock // long
            // IsLeaf // bool; calculated&saved
            LoadLodsSafe(reloadIds);

            // Enable(true); // don't enable until InitStateFull
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool ok = true;
            if (!Terrain.Test())
            {
                ok = false;
            }
            if (!ok)
            {
                System.Windows.Application.Current.Shutdown();
            }
            Thread th = new Thread(LoadState);
            th.Start(true);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            string msg = "";
            if (this.viewModel.Terrains.Count > 0)
                this.terrainCBx.SelectedIndex = this.viewModel.Terrains.Count - 1;
            else
                msg += "There are no terrains yet. ";

            if (this.viewModel.Lods.Count > 0)
            {
                this.detailCBx.SelectedIndex = this.viewModel.Lods.Count - 1;
            }
            else
                msg += "There are no Levels of Detail yet. ";
            /*
            if (this.viewModel.Regions.Count > 0)
                this.regionCBx.SelectedIndex = this.viewModel.Regions.Count - 1;
            else
                msg += "There are no Regions in this level of detail yet. ";

            if (this.viewModel.Mapblocks.Count > 0)
                this.blockCBx.SelectedIndex = this.viewModel.Mapblocks.Count - 1;
            else
                msg += "There are no Mapblocks in this level of detail yet. "; // region doesn't matter
            */
            if (msg.Length > 0)
            {
                MessageBox.Show(msg + " Try adding some using the corresponding picture button by the empty selection box.");
            }

            dispatcherTimer.Stop();
            dispatcherTimer = null;
            initState = InitStateFull;
            this.image.Source = null;
            Enable(true);
        }

        private void regionBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingController.Start();
            if (viewModel.SelectedLod == null)
            {
                MessageBox.Show("You must select a Level of Detail before adding or changing regions.");
                return;
            }
            RegionWindow dlg = new RegionWindow(viewModel.SelectedLod.LodId);
            var result = dlg.ShowDialog();
            if (result == true)
            {
                // saved to database already in this case, so:
                if (dlg.NewEntry != null)
                {
                    this.viewModel.Regions.Add(dlg.NewEntry);
                    dlg.NewEntry = null;
                }
            }
        }

        private void mapblockBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingController.Start();
            if (viewModel.SelectedLod == null)
            {
                MessageBox.Show("You must select a Level of Detail, Region and Terrain before adding or changing mapblocks.");
                return;
            }
            if (viewModel.SelectedRegion == null)
            {
                MessageBox.Show("You must select a Region and Terrain before adding or changing mapblocks.");
                return;
            }
            if (viewModel.SelectedTerrain == null)
            {
                MessageBox.Show("You must select a Terrain before adding or changing mapblocks.");
                return;
            }
            MapblockWindow dlg = new MapblockWindow(viewModel.SelectedLod.LodId, viewModel.SelectedRegion.RegionId, viewModel.SelectedTerrain.TerrainId);
            var result = dlg.ShowDialog();
            if (result == true)
            {
                // saved to database already in this case, so:
                if (dlg.NewEntry != null)
                {
                    this.viewModel.Mapblocks.Add(dlg.NewEntry);
                    dlg.NewEntry = null;
                }
            }
        }

        // private Dictionary<long, BitmapImage> bitmapCache = new Dictionary<long, BitmapImage>();

        private void ShowMapblock(Mapblock mapblock)
        {
            // TODO: finish this
        }

        private void ShowMapblock()
        {
            /*
            if (viewModel.SelectedMapblock != null)
            {
                try
                {
                    this.image.Source = new BitmapImage(
                        new Uri(System.IO.Path.Combine(SettingController.DataPath, viewModel.SelectedMapblock.Path))
                    );
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                }
            }
            */
        }

        private void blockCBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowMapblock();
        }

        private void detailCBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.Regions.Clear();
            this.viewModel.Mapblocks.Clear(); 

            this.viewModel.SelectedRegion = null;

            if (this.viewModel.SelectedLod != null)
            {
                foreach (Region entry in Region.WhereLodIdEquals(this.viewModel.SelectedLod.Id))
                {
                    this.viewModel.Regions.Add(entry);
                }
                if (this.viewModel.Regions.Count > 0)
                {
                    this.regionCBx.SelectedIndex = this.viewModel.Regions.Count - 1;
                }

                foreach (Mapblock entry in Mapblock.WhereLodIdEquals(this.viewModel.SelectedLod.Id))
                {
                    this.viewModel.Mapblocks.Add(entry);
                    ShowMapblock(entry);
                }
                if (this.viewModel.Mapblocks.Count > 0)
                {
                    this.regionCBx.SelectedIndex = this.viewModel.Mapblocks.Count - 1;
                }

            }

        }

        private void terrainCBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel.SelectedTerrain != null)
            {
                ShowTerrain(viewModel.SelectedTerrain);
                Color color = Terrain.ColorFromId(viewModel.SelectedTerrain.TerrainId);
                prefillTerrainRed = color.R;
                prefillTerrainGreen = color.G;
                prefillTerrainBlue = color.B;

                // TODO: (optional) Change the prefill values for new colors to avoid primary key
                // constraint violations in case the user doesn't change the color:
                /*
                if (prefillTerrainRed < 255)
                    prefillTerrainRed += 1;
                else
                    prefillTerrainRed -= 1;
                if (prefillTerrainGreen < 255)
                    prefillTerrainGreen += 1;
                else
                    prefillTerrainGreen -= 1;
                if (prefillTerrainBlue < 255)
                    prefillTerrainBlue += 1;
                else
                    prefillTerrainBlue -= 1;
                */
            }

        }
    }
    class ViewModel
    {
        public MainWindow Parent = null;
        public ObservableCollection<Lod> Lods { get; set; }
        public ObservableCollection<Region> Regions { get; set; }
        public ObservableCollection<Mapblock> Mapblocks { get; set; }
        public ObservableCollection<Terrain> Terrains { get; set; }
        public ViewModel()
        {
            Lods = new ObservableCollection<Lod>();
            Regions = new ObservableCollection<Region>();
            Mapblocks = new ObservableCollection<Mapblock>();
            Terrains = new ObservableCollection<Terrain>();
        }

        private int selectedLodId;
        public int SelectedLodId
        {
            get { return selectedLodId; }
            set
            {
                selectedLodId = value;
            }
        }
        private Lod selectedLod;
        public Lod SelectedLod
        {
            get { return selectedLod; }
            set
            {
                selectedLod = value;
            }
        }

        private int selectedRegionId;
        public int SelectedRegionId
        {
            get { return selectedRegionId; }
            set
            {
                selectedRegionId = value;
            }
        }
        private Region selectedRegion;
        public Region SelectedRegion
        {
            get { return selectedRegion; }
            set
            {
                selectedRegion = value;
            }
        }

        private int selectedMapblockId;
        public int SelectedMapblockId
        {
            get { return selectedMapblockId; }
            set
            {
                selectedMapblockId = value;
            }
        }
        private Mapblock selectedMapblock;
        public Mapblock SelectedMapblock
        {
            get { return selectedMapblock; }
            set
            {
                selectedMapblock = value;
            }
        }

        private int selectedTerrainId;
        public int SelectedTerrainId
        {
            get { return selectedTerrainId; }
            set
            {
                selectedTerrainId = value;
            }
        }
        private Terrain selectedTerrain;
        public Terrain SelectedTerrain
        {
            get { return selectedTerrain; }
            set
            {
                selectedTerrain = value;
                
            }
        }
    }
}

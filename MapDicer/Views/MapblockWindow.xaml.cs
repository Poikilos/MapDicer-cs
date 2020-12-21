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

namespace MapDicer.Views
{
    /// <summary>
    /// Interaction logic for MapblockWindow.xaml
    /// </summary>
    public partial class MapblockWindow : Window
    {
        public Mapblock NewEntry = null;
        private short lodId = 0;
        private short layerId = SettingController.LayerWhenOnly1;
        private long regionId = 0;
        private int terrainId = 0;
        public MapblockWindow()
        {
            InitializeComponent();
            MessageBox.Show("You must select a Lod and Region first.");
            this.DialogResult = false;
        }
        public MapblockWindow(short lodId, long regionId, int terrainId)
            : base()
        {
            InitializeComponent();
            this.lodId = lodId;
            this.regionId = regionId;
            this.terrainId = terrainId;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string string1;
            TextBox tb1;
            tb1 = this.pathTB;
            string1 = tb1.Text.Trim();
            if (string1.Length < 1)
            {
                MessageBox.Show("You must enter a path.");
                return;
            }
            Mapblock mapblock = new Mapblock();
            mapblock.LodId = this.lodId;
            mapblock.LayerId = this.layerId;
            mapblock.RegionId = this.regionId;
            mapblock.TerrainId = this.terrainId;
            short x = short.Parse(this.longitudeTB.Text);
            short z = short.Parse(this.latitudeTB.Text);
            MapDicerPos mpos = new MapDicerPos
            {
                Layer = layerId,
                LodId = lodId,
                X = x,
                Z = z,
            };
            mapblock.MapblockId = mpos.getSliceAsInteger();
            mapblock.Path = SettingController.Import(string1, "mapblocks", mapblock.MapblockId.ToString(), false);
            string error = Mapblock.Insert(mapblock, x, z, false);
            if ((error != null) && (error.Length > 0))
            {
                MessageBox.Show(String.Format("The database does not accept the entry.\n {0}", error));
                this.NewEntry = null;
                return;
            }
            this.NewEntry = mapblock;
            this.DialogResult = true;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NewEntry = null;
            this.DialogResult = false;
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
    }
}

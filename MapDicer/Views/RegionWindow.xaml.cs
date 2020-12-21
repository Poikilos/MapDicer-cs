using MapDicer.Models;
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
    /// Interaction logic for RegionWindow.xaml
    /// </summary>
    public partial class RegionWindow : Window
    {
        public Region NewEntry = null;
        private short parentId = 0;
        public RegionWindow()
        {
            InitializeComponent();
            MessageBox.Show("You must select a Lod first.");
            this.DialogResult = false;
        }
        public RegionWindow(short parentId)
            : base()
        {
            InitializeComponent();
            this.parentId = parentId;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string string1;
            TextBox tb1;
            tb1 = this.NameTB;
            string1 = tb1.Text.Trim();
            if (string1.Length < 1)
            {
                MessageBox.Show("You must enter a name.");
                return;
            }
            Region region = new Region();
            region.LodId = this.parentId;
            region.Name = string1;
            Region.Insert(region, true);
            this.NewEntry = region;
            this.DialogResult = true;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NewEntry = null;
            this.DialogResult = false;
        }
    }
}

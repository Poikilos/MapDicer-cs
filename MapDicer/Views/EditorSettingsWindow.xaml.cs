using MapDicer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class EditorSettingsWindow : Window
    {
        public EditorSettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.readSettings();
        }

        private void readSettings()
        {
            this.DbConnectionStringTB.Text = Properties.Settings.Default.DbConnectionString;
            this.DbGeneratedTB.Text = SettingModel.SqlConnectionString;
            this.DbFileTB.Text = Properties.Settings.Default.DbFile;
        }

        private void writeSettings()
        {
            Properties.Settings.Default.DbConnectionString = this.DbConnectionStringTB.Text.Trim();
            Properties.Settings.Default.DbFile = this.DbFileTB.Text.Trim();
            Properties.Settings.Default.Save();
            // MessageBox.Show(String.Format("Saved {0}", Properties.Settings.Default.LastConnectionString));
            // Properties.Settings.Default.Reload();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.writeSettings();
            SettingController.InitializeDB(); // Reloads if connection string changed.
            this.DialogResult = true;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.readSettings();
            this.DialogResult = false;
        }

        private void DbGenerateCSBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DbConnectionStringTB.Text = "";
            this.writeSettings();
            this.readSettings();
            this.readSettings();
        }
    }
}

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
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.readSettings();
        }

        private void readSettings()
        {
            this.LastConnectionStringTB.Text = Properties.Settings.Default.LastConnectionString;
        }

        private void writeSettings()
        {
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.readSettings();
            this.Close();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.writeSettings();
            this.Close();
        }
    }
}

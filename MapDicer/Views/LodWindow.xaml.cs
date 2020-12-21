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
// using System.Windows.Threading;
using System.Threading;
using System.Windows.Threading;

namespace MapDicer.Views
{
    /// <summary>
    /// Interaction logic for LodWindow.xaml
    /// </summary>
    public partial class LodWindow : Window
    {
        public LodWindow()
        {
            InitializeComponent();
        }
        public static short PrefillId = -1;
        public static short prefillParent;
        public Lod NewEntry = null;
        public Lod ChangedEntry = null;
        private bool suppressLoad = false;

        private void ClearFields(bool reloadIds, bool setSelectedIndexToNew)
        {
            this.suppressLoad = true;
            if (reloadIds)
            {
                Lod.errors.Clear();
                this.IdCbx.Items.Clear();
                // this.IdCbx.Items.Add(SettingModel.NewIdStr);
                List<Lod> items = null;
                try
                {
                    items = Lod.All();
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
                {
                    // doesn't matter, table must be empty (or not present yet)
                }
                if (Lod.errors.Count > 0)
                {
                    string msg = Lod.errors.Dequeue();
                    MessageBox.Show(String.Format("There is a problem with the database or database"
                                                    + " connection string in settings (Gear button,"
                                                    + " Editor Settings): {0}", msg));
                    Lod.errors.Clear();
                    this.DialogResult = false;
                    this.Close();
                }
                if (items != null)
                {
                    foreach (var item in items)
                        this.IdCbx.Items.Add(item.Id);
                    Enable(true); // enable drop-down if disabled by having 0
                }
            }
            if (setSelectedIndexToNew)
            {
                this.IdCbx.SelectedIndex = 0;
                this.IdCbx.SelectedItem = null;
                this.IdCbx.Text = "";
            }
            this.suppressLoad = false;

            this.NameTB.Text = "";
            this.UnitsPerSample.Text = "";
            this.SamplesPerMapblockTB.Text = "";
            this.IsLeafCB.IsChecked = false;
        }
        void SetFrom(Lod entity, bool selectIndexFromIdText)
        {
            // MessageBox.Show(String.Format("Setting from {0} and {1}selecting index.", entity.LodId, (selectIndexFromIdText?"":"not ")));
            suppressLoad = true;
            // this.IdCbx.Text = entity.LodId.ToString();
            for (int i = 0; i < this.IdCbx.Items.Count; i++)
            {
                if (selectIndexFromIdText)
                {
                    if (entity.LodId.ToString() == this.IdCbx.Items[i].ToString())
                    {
                        this.IdCbx.SelectedIndex = i;
                    }
                }
            }
            suppressLoad = false;
            this.NameTB.Text = entity.Name;
            this.ParentTB.Text = entity.ParentLodId.ToString();
            this.SamplesPerMapblockTB.Text = entity.SamplesPerMapblock.ToString();
            this.UnitsPerSample.Text = entity.UnitsPerSample.ToString();
            this.IsLeafCB.IsChecked = Lod.GetIsLeaf(entity);
        }

        /// <summary>
        /// Convert the form to an entity.
        /// The IsLeaf field is ignored.
        /// </summary>
        /// <param name="allowDup">Allow it to be an existing entity.</param>
        /// <param name="err">If this is not blank, the form is filled incorrectly</param>
        /// <returns></returns>
        public Lod AsEntity(bool allowDup, out string err)
        {
            Lod entity = new Lod();
            err = "";
            TextBox tb1 = null;
            short short1 = -1;
            long long1 = -1;
            string string1 = null;
            if (this.newCB.IsChecked == true || (this.IdCbx.Text.Trim() == "")) // this.IdCbx.Text == SettingModel.NewIdStr)
            {
                entity.LodId = -1;
            }
            else if (short.TryParse(this.IdCbx.Text, out short1))
            {
                entity.LodId = short1;
                if (!allowDup)
                {
                    Lod existing = Lod.GetById(short1);
                    if (existing != null)
                    {
                        err += " An entry with that Id already exists.";
                        return null;
                    }
                }
            }
            else
            {
                err += " The Id is not a valid short.";
                return null;
            }
            tb1 = this.NameTB;
            string1 = tb1.Text.Trim();
            if (string1.Length > 0)
                entity.Name = string1;
            else
            {
                err += String.Format(" The {0} must not be blank.", tb1.Name);
                return null;
            }
            tb1 = this.ParentTB;
            if (this.IdCbx.Items.Count < 1)
            {
                if (tb1.Text.Trim().Length > 0)
                {
                    err += "The top Level of Detail must have no parent.";
                    return null;
                }
                entity.ParentLodId = null; // Assume this is the top.
            }
            else if (short.TryParse(tb1.Text, out short1))
            {
                entity.ParentLodId = short1;
            }
            else
            {
                if (entity.LodId != 0)
                {
                    // Only allow a blank parent if the entry is the top level.
                    err += String.Format(" The {0} is not a valid short.", tb1.Name);
                    return null;
                }
                else
                {
                    entity.ParentLodId = null;
                }
            }

            

            tb1 = this.SamplesPerMapblockTB;
            string1 = tb1.Text.Trim();
            if (string1.Length > 0)
            {
                if (long.TryParse(string1, out long1))
                {
                    entity.SamplesPerMapblock = long1;
                }
                else
                {
                    err += String.Format(" The {0} is not a valid long.", tb1.Name);
                    return null;
                }
            }
            else
            {
                err += String.Format(" You must enter the SPM.", tb1.Name);
            }

            // TODO: OPTIONAL (calculated) UnitsPerSample:
            /*
            tb1 = this.UnitsPerSample;
            string1 = tb1.Text.Trim();
            if (string1.Length > 0)
            {
                if (long.TryParse(string1, out long1))
                {
                    entity.UnitsPerSample = long1;
                }
                else
                {
                    err += String.Format(" The {0} is not a valid long.", tb1.Name);
                    return null;
                }
            }
            else
                entity.UnitsPerSample = 0;
            */
            if (err.Length == 0)
                return entity;
            return null;
        }

        public void Enable(bool enable)
        {
            //Here update your label, button or any string related object.
            
            //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));    
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate {
                skeletonImage.Visibility = enable ? Visibility.Hidden : Visibility.Visible; // hide skeleton when enabled
                NameTB.IsEnabled = enable;
                if (this.IdCbx.Items.Count < 1)
                {
                    IdCbx.IsEnabled = enable;
                    IdCbx.IsReadOnly = !enable;
                }
                ParentTB.IsEnabled = enable;
            }));
        }

        private void LoadFieldsSafe(bool reloadIds, bool setSelectedIndexToNew, bool selectIndexFromIdText)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate {
                ClearFields(reloadIds, setSelectedIndexToNew);
                if (PrefillId > -1)
                {
                    Lod lod = Lod.GetById(PrefillId);
                    SetFrom(lod, selectIndexFromIdText); // does set SelectedIndex
                    this.newCB.IsChecked = false;
                }
                else
                {
                    this.newCB.IsChecked = true;
                }
                if (this.IdCbx.Items.Count < 1)
                {
                    // Assume no lods exist.
                    // Only new can be edited when there are no items:
                    this.newCB.IsChecked = true;
                    this.IdCbx.IsEnabled = false;
                    this.IdCbx.IsReadOnly = true;
                    this.IdCbx.Visibility = Visibility.Hidden;
                    this.newCB.IsEnabled = false;
                    this.ParentTB.Text = ""; // The top lod's parent is null.
                }
            }));
            // rhatwar007 https://stackoverflow.com/a/24624095/4541104
        }

        private void ReadPrefilledEntry(bool reloadIds)
        {
            Enable(false);

            //Enable(false);
            // LodId // short
            // Name // string
            // ParentLodId // short
            // UnitsPerSample // long; calculated
            // SamplesPerMapblock // long
            // IsLeaf // bool; calculated&saved
            LoadFieldsSafe(reloadIds, PrefillId <= -1, false);
            
            Enable(true);
        }

        private void IdCbx_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if ((this.IdCbx.SelectedIndex >= 0) && (this.IdCbx.Items.Count > 0))
            {
                this.IdCbx.Text = this.IdCbx.Items[this.IdCbx.SelectedIndex].ToString();
                if (!suppressLoad)
                {
                    this.ClearFields(false, false);
                    string string1;
                    string1 = this.IdCbx.Items[this.IdCbx.SelectedIndex].ToString();
                    if (this.newCB.IsChecked == true) // string1 == SettingModel.NewIdStr)
                    {
                        PrefillId = -1;
                        this.ParentTB.Text = Lod.LastId().ToString(); // -1 is ok (first entry)
                    }
                    else
                    {
                        PrefillId = short.Parse(string1);
                        // MessageBox.Show(String.Format("Getting {0}", PrefillId));
                        SetFrom(Lod.GetById(PrefillId), false); // true sets selected index
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(LoadForm);
            th.Start(true);
        }
        
        private void LoadForm(object o)
        {
            ReadPrefilledEntry((bool)o);
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isNew = (newCB.IsChecked == true); // (this.IdCbx.Text == SettingModel.NewIdStr);
            string err;
            Lod entry = AsEntity(!isNew, out err);
            if (err.Length > 0)
            {
                MessageBox.Show(err);
            }
            else
            {
                if (isNew)
                {
                    // ^ ok since LastId is -1 if there are none, so new one will be 0.
                    short tmpId = entry.Id;
                    string error = Lod.Insert(entry, true);
                    // MessageBox.Show(String.Format("Added Lod (SamplesPerMapblock={0})", entry.SamplesPerMapblock));
                    if ((error != null) && (error.Length > 0))
                    {
                        MessageBox.Show(String.Format(error + " (tmp id: {0}; generated id: {1})", tmpId, entry.Id));
                        return;
                    }
                    else
                    {
                        NewEntry = entry;
                    }
                }
                else
                {
                    Lod.Update(entry);
                    ChangedEntry = entry;
                }
                this.DialogResult = true;
                // this.Close(); // automatic on DialogResult not null
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.ClearFields(false, true);
            this.DialogResult = false;
            // this.Close(); // automatic on DialogResult not null
        }

        private void newCB_Checked(object sender, RoutedEventArgs e)
        {
            this.IdCbx.Text = "";
            this.IdCbx.SelectedItem = null;
            this.IdCbx.IsEnabled = false;
            this.IdCbx.Visibility = Visibility.Hidden;
            this.ClearFields(false, true);
        }

        private void newCB_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.IdCbx.Items.Count > 0)
                this.IdCbx.SelectedIndex = 0;
            this.IdCbx.IsEnabled = true;
            this.IdCbx.IsReadOnly = false;
            this.IdCbx.Visibility = Visibility.Visible;
        }
    }
}

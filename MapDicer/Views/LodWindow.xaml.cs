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

namespace MapDicer
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
        private bool suppressLoad = false;

        private void ClearFields(bool reloadIds, bool setSelectedIndexToNew)
        {
            this.suppressLoad = true;
            if (reloadIds)
            {
                Lod.errors.Clear();
                this.IdCbx.Items.Clear();
                this.IdCbx.Items.Add(SettingModel.NewIdStr);
                List<Lod> items = Lod.All();
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
                }
            }
            if (setSelectedIndexToNew)
                this.IdCbx.SelectedIndex = 0;
            this.suppressLoad = false;

            this.NameTB.Text = "";
            this.UnitsPerSample.Text = "";
            this.SamplesPerMapblockTB.Text = "";
            this.IsLeafCB.IsChecked = false;
        }
        void SetFrom(Lod entity)
        {
            suppressLoad = true;
            // this.IdCbx.Text = entity.LodId.ToString();
            for (int i = 0; i < this.IdCbx.Items.Count; i++)
            {
                if (entity.LodId.ToString() == this.IdCbx.Items[i].ToString())
                {
                    this.IdCbx.SelectedIndex = i;
                }
            }
            suppressLoad = false;
            this.NameTB.Text = entity.Name;
            this.ParentTB.Text = entity.Parent.ToString();
            this.SamplesPerMapblockTB.Text = entity.SamplesPerMapblock.ToString();
            this.UnitsPerSample.Text = entity.UnitsPerSample.ToString();
            this.IsLeafCB.IsChecked = entity.GetIsLeaf();
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
            if (this.IdCbx.Text == SettingModel.NewIdStr)
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
            if (short.TryParse(tb1.Text, out short1))
                entity.Parent = short1;
            else
            {
                err += String.Format(" The {0} is not a valid short.", tb1.Name);
                return null;
            }

            // OPTIONAL (calculated) SamplesPerMapblockTB:
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

            if (err.Length == 0)
                return entity;
            return null;
        }

        private void ReadPrefilledEntry(bool reloadIds)
        {
            // LodId // short
            // Name // string
            // Parent // short
            // UnitsPerSample // long; calculated
            // SamplesPerMapblock // long
            // IsLeaf // bool; calculated&saved
            ClearFields(reloadIds, PrefillId <= -1);
            if (PrefillId > -1)
            {
                Lod lod = Lod.GetById(PrefillId);
                SetFrom(lod); // does set SelectedIndex
            }
        }

        private void IdCbx_SelectionChanged(object sender, RoutedEventArgs e)
        {
            this.IdCbx.Text = this.IdCbx.Text = this.IdCbx.Items[this.IdCbx.SelectedIndex].ToString();
            if (!suppressLoad)
            {
                this.ClearFields(false, false);
                string string1 = this.IdCbx.Items[this.IdCbx.SelectedIndex].ToString();
                if (string1 == SettingModel.NewIdStr)
                {
                    PrefillId = -1;
                    this.ParentTB.Text = Lod.LastId().ToString(); // -1 is ok (first entry)
                }
                else {
                    PrefillId = short.Parse(string1);
                    SetFrom(Lod.GetById(PrefillId)); // does set SelectedIndex
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReadPrefilledEntry(true);
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isNew = (this.IdCbx.Text == SettingModel.NewIdStr);
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
                    Lod.Insert(entry, true);
                }
                else
                {
                    Lod.Update(entry);
                }
                this.DialogResult = true;
                this.Close();
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.ClearFields(false, true);
            this.DialogResult = false;
            this.Close();
        }
    }
}

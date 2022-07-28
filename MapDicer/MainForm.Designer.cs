/*
 * Created by SharpDevelop.
 * User: Jatlivecom
 * Date: 7/28/2022
 * Time: 6:58 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace MapDicer
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.progressbar = new System.Windows.Forms.ProgressBar();
			this.menuTLP = new System.Windows.Forms.TableLayoutPanel();
			this.detailBtn = new System.Windows.Forms.Button();
			this.regionBtn = new System.Windows.Forms.Button();
			this.terrainBtn = new System.Windows.Forms.Button();
			this.terrainColorBtn = new System.Windows.Forms.Button();
			this.settingsButton = new System.Windows.Forms.Button();
			this.lodCBx = new System.Windows.Forms.ComboBox();
			this.regionCBx = new System.Windows.Forms.ComboBox();
			this.terrainCBx = new System.Windows.Forms.ComboBox();
			this.terrainBrushSizeSlider = new System.Windows.Forms.TrackBar();
			this.mapblockBtn = new System.Windows.Forms.Button();
			this.mainTLP = new System.Windows.Forms.TableLayoutPanel();
			this.menuTLP.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.terrainBrushSizeSlider)).BeginInit();
			this.mainTLP.SuspendLayout();
			this.SuspendLayout();
			// 
			// progressbar
			// 
			this.progressbar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.progressbar.Location = new System.Drawing.Point(0, 348);
			this.progressbar.Name = "progressbar";
			this.progressbar.Size = new System.Drawing.Size(904, 23);
			this.progressbar.TabIndex = 1;
			// 
			// menuTLP
			// 
			this.menuTLP.ColumnCount = 8;
			this.menuTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.636364F));
			this.menuTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
			this.menuTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.636364F));
			this.menuTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
			this.menuTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.636364F));
			this.menuTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
			this.menuTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.636364F));
			this.menuTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.636364F));
			this.menuTLP.Controls.Add(this.detailBtn, 0, 0);
			this.menuTLP.Controls.Add(this.regionBtn, 2, 0);
			this.menuTLP.Controls.Add(this.terrainBtn, 4, 0);
			this.menuTLP.Controls.Add(this.terrainColorBtn, 6, 0);
			this.menuTLP.Controls.Add(this.settingsButton, 7, 0);
			this.menuTLP.Controls.Add(this.lodCBx, 1, 0);
			this.menuTLP.Controls.Add(this.regionCBx, 3, 0);
			this.menuTLP.Controls.Add(this.terrainCBx, 5, 0);
			this.menuTLP.Controls.Add(this.terrainBrushSizeSlider, 1, 1);
			this.menuTLP.Controls.Add(this.mapblockBtn, 4, 1);
			this.menuTLP.Dock = System.Windows.Forms.DockStyle.Fill;
			this.menuTLP.Location = new System.Drawing.Point(3, 3);
			this.menuTLP.Name = "menuTLP";
			this.menuTLP.RowCount = 2;
			this.menuTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.menuTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.menuTLP.Size = new System.Drawing.Size(898, 60);
			this.menuTLP.TabIndex = 0;
			// 
			// detailBtn
			// 
			this.detailBtn.Location = new System.Drawing.Point(3, 3);
			this.detailBtn.Name = "detailBtn";
			this.detailBtn.Size = new System.Drawing.Size(26, 23);
			this.detailBtn.TabIndex = 0;
			this.detailBtn.UseVisualStyleBackColor = true;
			this.detailBtn.Click += new System.EventHandler(this.DetailBtnClick);
			// 
			// regionBtn
			// 
			this.regionBtn.Location = new System.Drawing.Point(279, 3);
			this.regionBtn.Name = "regionBtn";
			this.regionBtn.Size = new System.Drawing.Size(26, 23);
			this.regionBtn.TabIndex = 1;
			this.regionBtn.UseVisualStyleBackColor = true;
			this.regionBtn.Click += new System.EventHandler(this.RegionBtnClick);
			// 
			// terrainBtn
			// 
			this.terrainBtn.Location = new System.Drawing.Point(555, 3);
			this.terrainBtn.Name = "terrainBtn";
			this.terrainBtn.Size = new System.Drawing.Size(26, 23);
			this.terrainBtn.TabIndex = 2;
			this.terrainBtn.UseVisualStyleBackColor = true;
			this.terrainBtn.Click += new System.EventHandler(this.TerrainBtnClick);
			// 
			// terrainColorBtn
			// 
			this.terrainColorBtn.ForeColor = System.Drawing.Color.Green;
			this.terrainColorBtn.Location = new System.Drawing.Point(831, 3);
			this.terrainColorBtn.Name = "terrainColorBtn";
			this.terrainColorBtn.Size = new System.Drawing.Size(26, 23);
			this.terrainColorBtn.TabIndex = 3;
			this.terrainColorBtn.Text = "●";
			this.terrainColorBtn.UseVisualStyleBackColor = true;
			this.terrainColorBtn.Click += new System.EventHandler(this.TerrainColorBtnClick);
			// 
			// settingsButton
			// 
			this.settingsButton.Image = ((System.Drawing.Image)(resources.GetObject("settingsButton.Image")));
			this.settingsButton.Location = new System.Drawing.Point(863, 3);
			this.settingsButton.Name = "settingsButton";
			this.settingsButton.Size = new System.Drawing.Size(32, 23);
			this.settingsButton.TabIndex = 4;
			this.settingsButton.UseVisualStyleBackColor = true;
			this.settingsButton.Click += new System.EventHandler(this.SettingsButtonClick);
			// 
			// lodCBx
			// 
			this.lodCBx.Dock = System.Windows.Forms.DockStyle.Top;
			this.lodCBx.FormattingEnabled = true;
			this.lodCBx.Location = new System.Drawing.Point(35, 3);
			this.lodCBx.Name = "lodCBx";
			this.lodCBx.Size = new System.Drawing.Size(238, 21);
			this.lodCBx.TabIndex = 5;
			this.lodCBx.SelectedIndexChanged += new System.EventHandler(this.LodCBxSelectedIndexChanged);
			// 
			// regionCBx
			// 
			this.regionCBx.Dock = System.Windows.Forms.DockStyle.Top;
			this.regionCBx.FormattingEnabled = true;
			this.regionCBx.Location = new System.Drawing.Point(311, 3);
			this.regionCBx.Name = "regionCBx";
			this.regionCBx.Size = new System.Drawing.Size(238, 21);
			this.regionCBx.TabIndex = 6;
			// 
			// terrainCBx
			// 
			this.terrainCBx.Dock = System.Windows.Forms.DockStyle.Top;
			this.terrainCBx.FormattingEnabled = true;
			this.terrainCBx.Location = new System.Drawing.Point(587, 3);
			this.terrainCBx.Name = "terrainCBx";
			this.terrainCBx.Size = new System.Drawing.Size(238, 21);
			this.terrainCBx.TabIndex = 7;
			this.terrainCBx.SelectedIndexChanged += new System.EventHandler(this.TerrainCBxSelectedIndexChanged);
			// 
			// terrainBrushSizeSlider
			// 
			this.terrainBrushSizeSlider.Location = new System.Drawing.Point(35, 33);
			this.terrainBrushSizeSlider.Name = "terrainBrushSizeSlider";
			this.terrainBrushSizeSlider.Size = new System.Drawing.Size(104, 24);
			this.terrainBrushSizeSlider.TabIndex = 8;
			this.terrainBrushSizeSlider.Scroll += new System.EventHandler(this.TerrainBrushSizeSliderScroll);
			// 
			// mapblockBtn
			// 
			this.mapblockBtn.Location = new System.Drawing.Point(555, 33);
			this.mapblockBtn.Name = "mapblockBtn";
			this.mapblockBtn.Size = new System.Drawing.Size(26, 23);
			this.mapblockBtn.TabIndex = 9;
			this.mapblockBtn.UseVisualStyleBackColor = true;
			this.mapblockBtn.Click += new System.EventHandler(this.MapblockBtnClick);
			// 
			// mainTLP
			// 
			this.mainTLP.ColumnCount = 1;
			this.mainTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.mainTLP.Controls.Add(this.menuTLP, 0, 0);
			this.mainTLP.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTLP.Location = new System.Drawing.Point(0, 0);
			this.mainTLP.Name = "mainTLP";
			this.mainTLP.RowCount = 2;
			this.mainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.12226F));
			this.mainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.87775F));
			this.mainTLP.Size = new System.Drawing.Size(904, 348);
			this.mainTLP.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(904, 371);
			this.Controls.Add(this.mainTLP);
			this.Controls.Add(this.progressbar);
			this.Name = "MainForm";
			this.Text = "MapDicer";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.Resize += new System.EventHandler(this.MainFormResize);
			this.menuTLP.ResumeLayout(false);
			this.menuTLP.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.terrainBrushSizeSlider)).EndInit();
			this.mainTLP.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button mapblockBtn;
		private System.Windows.Forms.TrackBar terrainBrushSizeSlider;
		private System.Windows.Forms.TableLayoutPanel mainTLP;
		private System.Windows.Forms.ComboBox terrainCBx;
		private System.Windows.Forms.ComboBox regionCBx;
		private System.Windows.Forms.ComboBox lodCBx;
		private System.Windows.Forms.Button settingsButton;
		private System.Windows.Forms.Button terrainColorBtn;
		private System.Windows.Forms.Button terrainBtn;
		private System.Windows.Forms.Button regionBtn;
		private System.Windows.Forms.Button detailBtn;
		private System.Windows.Forms.TableLayoutPanel menuTLP;
		private System.Windows.Forms.ProgressBar progressbar;
	}
}

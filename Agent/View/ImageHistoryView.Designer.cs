namespace Agent.View
{
    partial class ImageHistoryView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imageListView = new System.Windows.Forms.ListView();
            this.largeImgeList = new System.Windows.Forms.ImageList(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.grid_panel = new System.Windows.Forms.Panel();
            this.image_panel = new System.Windows.Forms.Panel();
            this.title = new System.Windows.Forms.Label();
            this.image = new System.Windows.Forms.PictureBox();
            this.grid_panel.SuspendLayout();
            this.image_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.image)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListView
            // 
            this.imageListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageListView.LargeImageList = this.largeImgeList;
            this.imageListView.Location = new System.Drawing.Point(1, 21);
            this.imageListView.MultiSelect = false;
            this.imageListView.Name = "imageListView";
            this.imageListView.Size = new System.Drawing.Size(662, 515);
            this.imageListView.TabIndex = 0;
            this.imageListView.UseCompatibleStateImageBehavior = false;
            this.imageListView.VirtualMode = true;
            this.imageListView.CacheVirtualItems += new System.Windows.Forms.CacheVirtualItemsEventHandler(this.imageListView_CacheVirtualItems);
            this.imageListView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.imageListView_RetrieveVirtualItem);
            this.imageListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.imageListView_MouseDoubleClick);
            // 
            // largeImgeList
            // 
            this.largeImgeList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.largeImgeList.ImageSize = new System.Drawing.Size(256, 256);
            this.largeImgeList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = Resource1.day;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(39, 0);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker.TabIndex = 5;
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // grid_panel
            // 
            this.grid_panel.Controls.Add(this.image_panel);
            this.grid_panel.Controls.Add(this.dateTimePicker);
            this.grid_panel.Controls.Add(this.imageListView);
            this.grid_panel.Controls.Add(this.label4);
            this.grid_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_panel.Location = new System.Drawing.Point(0, 0);
            this.grid_panel.Name = "grid_panel";
            this.grid_panel.Size = new System.Drawing.Size(662, 536);
            this.grid_panel.TabIndex = 7;
            // 
            // image_panel
            // 
            this.image_panel.BackColor = System.Drawing.SystemColors.Control;
            this.image_panel.Controls.Add(this.image);
            this.image_panel.Controls.Add(this.title);
            this.image_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.image_panel.Location = new System.Drawing.Point(0, 0);
            this.image_panel.Name = "image_panel";
            this.image_panel.Size = new System.Drawing.Size(662, 536);
            this.image_panel.TabIndex = 8;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.title.ForeColor = System.Drawing.Color.Coral;
            this.title.Location = new System.Drawing.Point(4, 4);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(109, 39);
            this.title.TabIndex = 0;
            this.title.Text = "label1";
            // 
            // image
            // 
            this.image.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.image.Location = new System.Drawing.Point(0, 46);
            this.image.Name = "image";
            this.image.Size = new System.Drawing.Size(662, 487);
            this.image.TabIndex = 1;
            this.image.TabStop = false;
            this.image.DoubleClick += new System.EventHandler(this.image_DoubleClick);
            // 
            // ImageHistoryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.grid_panel);
            this.Name = "ImageHistoryView";
            this.Size = new System.Drawing.Size(662, 536);
            this.grid_panel.ResumeLayout(false);
            this.grid_panel.PerformLayout();
            this.image_panel.ResumeLayout(false);
            this.image_panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListView imageListView;
        private System.Windows.Forms.ImageList largeImgeList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Panel grid_panel;
        private System.Windows.Forms.Panel image_panel;
        private System.Windows.Forms.PictureBox image;
        private System.Windows.Forms.Label title;
    }
}

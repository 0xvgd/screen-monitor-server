namespace Agent.View
{
    partial class ImageGridView
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            this.imageListView = new System.Windows.Forms.ListView();
            this.capturedImageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageListView
            // 
            this.imageListView.AllowColumnReorder = true;
            this.imageListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListView.FullRowSelect = true;
            this.imageListView.GridLines = true;
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = "listViewGroup1";
            this.imageListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.imageListView.HideSelection = false;
            this.imageListView.LargeImageList = this.capturedImageList;
            this.imageListView.Location = new System.Drawing.Point(0, 0);
            this.imageListView.Name = "imageListView";
            this.imageListView.ShowItemToolTips = true;
            this.imageListView.Size = new System.Drawing.Size(649, 477);
            this.imageListView.TabIndex = 0;
            this.imageListView.UseCompatibleStateImageBehavior = false;
            this.imageListView.VirtualMode = true;
            this.imageListView.CacheVirtualItems += new System.Windows.Forms.CacheVirtualItemsEventHandler(this.imageListView_CacheVirtualItems);
            this.imageListView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.imageListView_RetrieveVirtualItem);
            this.imageListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.imageListView_MouseDoubleClick);
            // 
            // capturedImageList
            // 
            this.capturedImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.capturedImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.capturedImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ImageGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imageListView);
            this.Name = "ImageGridView";
            this.Size = new System.Drawing.Size(649, 477);
            this.ClientSizeChanged += new System.EventHandler(this.ImageGridView_ClientSizeChanged);
            this.VisibleChanged += new System.EventHandler(this.ImageGridView_VisibleChanged);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView imageListView;
        private System.Windows.Forms.ImageList capturedImageList;
    }
}

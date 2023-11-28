namespace Agent.View
{
    partial class ComListView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComListView));
            this._comListViewPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.comImageList = new System.Windows.Forms.ImageList(this.components);
            this.clvTimer = new System.Windows.Forms.Timer(this.components);
            this.itemMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.chageUserNameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoffMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePasswordMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.usbOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uspCloseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteExecToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.sendMessageToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.comList = new ListViewEmbeddedControls.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._comListViewPanel.SuspendLayout();
            this.itemMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _comListViewPanel
            // 
            this._comListViewPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this._comListViewPanel.Controls.Add(this.label1);
            this._comListViewPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._comListViewPanel.Location = new System.Drawing.Point(0, 0);
            this._comListViewPanel.Name = "_comListViewPanel";
            this._comListViewPanel.Size = new System.Drawing.Size(220, 31);
            this._comListViewPanel.TabIndex = 0;
            this._comListViewPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this._comListViewPanel_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = Resource1.Users;
            // 
            // comImageList
            // 
            this.comImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("comImageList.ImageStream")));
            this.comImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.comImageList.Images.SetKeyName(0, "18.png");
            this.comImageList.Images.SetKeyName(1, "19.png");
            this.comImageList.Images.SetKeyName(2, "20.png");
            this.comImageList.Images.SetKeyName(3, "off.png");
            // 
            // itemMenu
            // 
            this.itemMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chageUserNameMenuItem,
            this.PowerToolStripMenuItem,
            this.AccountToolStripMenuItem,
            this.toolStripMenuItem1,
            this.remoteExecToolStrip,
            this.sendMessageToolStrip});
            this.itemMenu.Name = "itemMenu";
            this.itemMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.itemMenu.Size = new System.Drawing.Size(135, 136);
            // 
            // chageUserNameMenuItem
            // 
            this.chageUserNameMenuItem.Name = "chageUserNameMenuItem";
            this.chageUserNameMenuItem.Size = new System.Drawing.Size(134, 22);
            this.chageUserNameMenuItem.Text = "Client Info";
            this.chageUserNameMenuItem.Click += new System.EventHandler(this.chageUserNameMenuItem_Click);
            // 
            // PowerToolStripMenuItem
            // 
            this.PowerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shutdownMenuItem,
            this.logoffMenuItem,
            this.restartMenuItem});
            this.PowerToolStripMenuItem.Name = "PowerToolStripMenuItem";
            this.PowerToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.PowerToolStripMenuItem.Text = "Power";
            // 
            // shutdownMenuItem
            // 
            this.shutdownMenuItem.Name = "shutdownMenuItem";
            this.shutdownMenuItem.Size = new System.Drawing.Size(134, 22);
            this.shutdownMenuItem.Text = "Off";
            this.shutdownMenuItem.Click += new System.EventHandler(this.shutdownMenuItem_Click);
            // 
            // logoffMenuItem
            // 
            this.logoffMenuItem.Name = "logoffMenuItem";
            this.logoffMenuItem.Size = new System.Drawing.Size(134, 22);
            this.logoffMenuItem.Text = "Sign out";
            this.logoffMenuItem.Click += new System.EventHandler(this.logoffMenuItem_Click);
            // 
            // restartMenuItem
            // 
            this.restartMenuItem.Name = "restartMenuItem";
            this.restartMenuItem.Size = new System.Drawing.Size(134, 22);
            this.restartMenuItem.Text = "Reboot";
            this.restartMenuItem.Click += new System.EventHandler(this.restartMenuItem_Click);
            // 
            // AccountToolStripMenuItem
            // 
            this.AccountToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removePasswordMenuItem,
            this.changePasswordMenuItem});
            this.AccountToolStripMenuItem.Name = "AccountToolStripMenuItem";
            this.AccountToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.AccountToolStripMenuItem.Text = "Account Password";
            // 
            // removePasswordMenuItem
            // 
            this.removePasswordMenuItem.Name = "removePasswordMenuItem";
            this.removePasswordMenuItem.Size = new System.Drawing.Size(158, 22);
            this.removePasswordMenuItem.Text = "Remove";
            this.removePasswordMenuItem.Click += new System.EventHandler(this.removePasswordMenuItem_Click);
            // 
            // changePasswordMenuItem
            // 
            this.changePasswordMenuItem.Name = "changePasswordMenuItem";
            this.changePasswordMenuItem.Size = new System.Drawing.Size(158, 22);
            this.changePasswordMenuItem.Text = "Modify";
            this.changePasswordMenuItem.Click += new System.EventHandler(this.changePasswordMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usbOpenMenuItem,
            this.uspCloseMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem1.Text = "USB Port";
            // 
            // usbOpenMenuItem
            // 
            this.usbOpenMenuItem.Name = "usbOpenMenuItem";
            this.usbOpenMenuItem.Size = new System.Drawing.Size(143, 22);
            this.usbOpenMenuItem.Text = "Allow";
            this.usbOpenMenuItem.Click += new System.EventHandler(this.usbOpenMenuItem_Click);
            // 
            // uspCloseMenuItem
            // 
            this.uspCloseMenuItem.Name = "uspCloseMenuItem";
            this.uspCloseMenuItem.Size = new System.Drawing.Size(143, 22);
            this.uspCloseMenuItem.Text = "Block";
            this.uspCloseMenuItem.Click += new System.EventHandler(this.uspCloseMenuItem_Click);
            // 
            // remoteExecToolStrip
            // 
            this.remoteExecToolStrip.Name = "remoteExecToolStrip";
            this.remoteExecToolStrip.Size = new System.Drawing.Size(134, 22);
            this.remoteExecToolStrip.Text = "Run Remotely";
            this.remoteExecToolStrip.Click += new System.EventHandler(this.remoteExecToolStrip_Click);
            // 
            // sendMessageToolStrip
            // 
            this.sendMessageToolStrip.Name = "sendMessageToolStrip";
            this.sendMessageToolStrip.Size = new System.Drawing.Size(134, 22);
            this.sendMessageToolStrip.Text = "Send Message";
            this.sendMessageToolStrip.Click += new System.EventHandler(this.sendMessageToolStrip_Click);
            // 
            // comList
            // 
            this.comList.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.comList.AllowColumnReorder = true;
            this.comList.AutoArrange = false;
            this.comList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.comList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comList.FullRowSelect = true;
            this.comList.GridLines = true;
            this.comList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.comList.HideSelection = false;
            this.comList.LargeImageList = this.comImageList;
            this.comList.Location = new System.Drawing.Point(0, 31);
            this.comList.Name = "comList";
            this.comList.ShowItemToolTips = true;
            this.comList.Size = new System.Drawing.Size(220, 338);
            this.comList.SmallImageList = this.comImageList;
            this.comList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.comList.TabIndex = 2;
            this.comList.UseCompatibleStateImageBehavior = false;
            this.comList.View = System.Windows.Forms.View.Details;
            this.comList.SelectedIndexChanged += new System.EventHandler(this.comList_SelectedIndexChanged);
            this.comList.Enter += new System.EventHandler(this.comList_Enter);
            this.comList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ComListView_KeyDown);
            this.comList.Leave += new System.EventHandler(this.comList_Leave);
            this.comList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comList_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = Resource1.name;
            this.columnHeader1.Width = 110;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = Resource1.ip;
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 93;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "USB";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 50;
            // 
            // ComListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comList);
            this.Controls.Add(this._comListViewPanel);
            this.Name = "ComListView";
            this.Size = new System.Drawing.Size(220, 369);
            this.Load += new System.EventHandler(this.ComListView_Load);
            this.Enter += new System.EventHandler(this.comList_Enter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ComListView_KeyDown);
            this.Leave += new System.EventHandler(this.comList_Leave);
            this._comListViewPanel.ResumeLayout(false);
            this._comListViewPanel.PerformLayout();
            this.itemMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _comListViewPanel;
        private System.Windows.Forms.Label label1;
        private ListViewEmbeddedControls.ListViewEx comList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Timer clvTimer;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ImageList comImageList;
        private System.Windows.Forms.ContextMenuStrip itemMenu;
        private System.Windows.Forms.ToolStripMenuItem chageUserNameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PowerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoffMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removePasswordMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem uspCloseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usbOpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remoteExecToolStrip;
        private System.Windows.Forms.ToolStripMenuItem sendMessageToolStrip;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}

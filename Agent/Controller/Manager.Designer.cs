namespace Agent.Controller
{


    partial class Manager
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Manager));
            this.mainStatus = new System.Windows.Forms.StatusStrip();
            this.label = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.comListView = new Agent.View.ComListView();
            this.eventListView1 = new Agent.View.EventListView();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.user_monitor_timer = new System.Windows.Forms.Timer(this.components);
            this.connection_history_timer = new System.Windows.Forms.Timer(this.components);
            this.image_caputre_timer = new System.Windows.Forms.Timer(this.components);
            this.thumb_image_capture_timer = new System.Windows.Forms.Timer(this.components);
            this.search_uninstall_timer = new System.Windows.Forms.Timer(this.components);
            this.capturedImagelistView = new Agent.View.ImageGridView();
            this.generalTool = new System.Windows.Forms.ToolStripButton();
            this.imageViewool = new System.Windows.Forms.ToolStripButton();
            this.settingTool = new System.Windows.Forms.ToolStripButton();
            this.mainTool = new System.Windows.Forms.ToolStrip();
            this.mainStatus.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mainTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainStatus
            // 
            this.mainStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.label});
            this.mainStatus.Location = new System.Drawing.Point(0, 451);
            this.mainStatus.Name = "mainStatus";
            this.mainStatus.Size = new System.Drawing.Size(872, 22);
            this.mainStatus.TabIndex = 0;
            this.mainStatus.Text = "statusStrip1";
            // 
            // label
            // 
            this.label.BackColor = System.Drawing.Color.LightCoral;
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 70);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.comListView);
            this.splitContainer1.Panel1MinSize = 210;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.eventListView1);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(872, 381);
            this.splitContainer1.SplitterDistance = 277;
            this.splitContainer1.TabIndex = 2;
            // 
            // comListView
            // 
            this.comListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comListView.Location = new System.Drawing.Point(0, 0);
            this.comListView.MinimumSize = new System.Drawing.Size(192, 360);
            this.comListView.Name = "comListView";
            this.comListView.Size = new System.Drawing.Size(277, 381);
            this.comListView.TabIndex = 0;
            this.comListView.Load += new System.EventHandler(this.comListView_Load);
            // 
            // eventListView1
            // 
            this.eventListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventListView1.Location = new System.Drawing.Point(0, 0);
            this.eventListView1.Name = "eventListView1";
            this.eventListView1.SelectedTabIndex = 0;
            this.eventListView1.Size = new System.Drawing.Size(591, 381);
            this.eventListView1.TabIndex = 0;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = Resource1.remoteMonitor + " v2.0";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // user_monitor_timer
            // 
            this.user_monitor_timer.Enabled = true;
            this.user_monitor_timer.Interval = 5000;
            this.user_monitor_timer.Tick += new System.EventHandler(this.user_monitor_timer_Tick);
            // 
            // connection_history_timer
            // 
            this.connection_history_timer.Enabled = false;
            this.connection_history_timer.Interval = 1000;
            this.connection_history_timer.Tick += new System.EventHandler(this.connection_history_timer_Tick);
            // 
            // image_caputre_timer
            // 
            this.image_caputre_timer.Enabled = true;
            this.image_caputre_timer.Interval = 1000;
            this.image_caputre_timer.Tick += new System.EventHandler(this.image_caputre_timer_Tick);
            // 
            // thumb_image_capture_timer
            // 
            this.thumb_image_capture_timer.Interval = 50000;
            this.thumb_image_capture_timer.Tick += new System.EventHandler(this.thumb_image_capture_timer_Tick);
            // 
            // search_uninstall_timer
            // 
            this.search_uninstall_timer.Enabled = true;
            this.search_uninstall_timer.Interval = 600000;
            this.search_uninstall_timer.Tick += new System.EventHandler(this.search_uninstall_timer_Tick);
            // 
            // capturedImagelistView
            // 
            this.capturedImagelistView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.capturedImagelistView.Location = new System.Drawing.Point(0, 70);
            this.capturedImagelistView.Name = "capturedImagelistView";
            this.capturedImagelistView.Size = new System.Drawing.Size(872, 381);
            this.capturedImagelistView.TabIndex = 3;
            this.capturedImagelistView.Visible = false;
            // 
            // generalTool
            // 
            this.generalTool.Image = ((System.Drawing.Image)(resources.GetObject("generalTool.Image")));
            this.generalTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.generalTool.Name = "generalTool";
            this.generalTool.Size = new System.Drawing.Size(59, 67);
            this.generalTool.Text = Resource1.all;
            this.generalTool.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.generalTool.Click += new System.EventHandler(this.generalTool_Click);
            // 
            // imageViewool
            // 
            this.imageViewool.Image = ((System.Drawing.Image)(resources.GetObject("imageViewool.Image")));
            this.imageViewool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.imageViewool.Name = "imageViewool";
            this.imageViewool.Size = new System.Drawing.Size(59, 67);
            this.imageViewool.Text = Resource1.screen;
            this.imageViewool.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.imageViewool.Click += new System.EventHandler(this.imageViewTool_Click);
            // 
            // settingTool
            // 
            this.settingTool.Image = ((System.Drawing.Image)(resources.GetObject("settingTool.Image")));
            this.settingTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingTool.Name = "settingTool";
            this.settingTool.Size = new System.Drawing.Size(59, 67);
            this.settingTool.Text = Resource1.setting;
            this.settingTool.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.settingTool.Click += new System.EventHandler(this.settingTool_Click);
            // 
            // mainTool
            // 
            this.mainTool.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.mainTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generalTool,
            this.imageViewool,
            this.settingTool});
            this.mainTool.Location = new System.Drawing.Point(0, 0);
            this.mainTool.Name = "mainTool";
            this.mainTool.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainTool.Size = new System.Drawing.Size(872, 70);
            this.mainTool.Stretch = true;
            this.mainTool.TabIndex = 1;
            this.mainTool.Text = "mainTool";
            // 
            // Manager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(872, 473);
            this.Controls.Add(this.capturedImagelistView);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainTool);
            this.Controls.Add(this.mainStatus);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(880, 480);
            this.Name = "Manager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = Resource1.remoteMonitor + " v2.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Manager_FormClosing);
            this.mainStatus.ResumeLayout(false);
            this.mainStatus.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.mainTool.ResumeLayout(false);
            this.mainTool.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip mainStatus;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private View.ComListView comListView;
        private View.EventListView eventListView1;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Timer user_monitor_timer;
        private System.Windows.Forms.Timer connection_history_timer;
        private System.Windows.Forms.Timer image_caputre_timer;
        private View.ImageGridView capturedImagelistView;
        private System.Windows.Forms.Timer thumb_image_capture_timer;
        private System.Windows.Forms.Timer search_uninstall_timer;
        private System.Windows.Forms.ToolStripStatusLabel label;
        private System.Windows.Forms.ToolStripButton generalTool;
        private System.Windows.Forms.ToolStripButton imageViewool;
        private System.Windows.Forms.ToolStripButton settingTool;
        private System.Windows.Forms.ToolStrip mainTool;

    }
}


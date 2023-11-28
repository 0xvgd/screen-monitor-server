using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Agent.Controller
{
    public partial class Manager : Form
    {

        private delegate void RefreshEvnetListDelegate(Conf.Constant.MONITOR_TYPE monitor_type);
        private delegate void RefreshUserListDelegate(String key);
        private delegate void RefreshThumbImageListDelegate(String user_key);

        private Util.TaskbarNotifier taskbarNotifier1 = null;
        private Util.TaskbarNotifier taskbarNotifier2 = null;

        private FormWindowState oldState = FormWindowState.Maximized;
        private bool _shouldShutdown = false;

        private String _selectedUserKey = null;     //physical address

        private Control _currentControl;

        Mutex mut = new Mutex(false, "Manager");

        public Manager()
        {
            InitializeComponent();

            Model.UserListManager.Instance.Load(Conf.Constant.USER_INFO_DIC_NAME);
            foreach (String user_key in Model.UserListManager.Instance.AllKeys)
            {
                comListView.AddUserToList(user_key);
            }
            Init();
        }


        private void Init()
        {
           
            taskbarNotifier1 = new Util.TaskbarNotifier();
            taskbarNotifier1.SetBackgroundBitmap(Resource1.skin2, Color.FromArgb(255, 0, 255));
            taskbarNotifier1.SetCloseBitmap(Resource1.close2, Color.FromArgb(255, 0, 255), new Point(300, 74));
            taskbarNotifier1.TitleRectangle = new Rectangle(123, 80, 176, 16);
            taskbarNotifier1.ContentRectangle = new Rectangle(116, 97, 197, 25);
            taskbarNotifier1.ContentClick += new EventHandler(taskbarNotifier1_ContentClick);

            taskbarNotifier2 = new Util.TaskbarNotifier();
            taskbarNotifier2.SetBackgroundBitmap(Resource1.skin, Color.FromArgb(255, 0, 255));
            taskbarNotifier2.SetCloseBitmap(Resource1.close, Color.FromArgb(255, 0, 255), new Point(127, 8));
            taskbarNotifier2.TitleRectangle = new Rectangle(40, 9, 70, 25);
            taskbarNotifier2.ContentRectangle = new Rectangle(8, 41, 133, 68);
            taskbarNotifier2.ContentClick += new EventHandler(taskbarNotifier2_ContentClick);
            _shouldShutdown = false;

            Conf.Constant.LOCAL_IP = Util.Util.GetIPAddress().ToString();
            Config.LoadConfig(Config.ReadConfig());
            
            NetworkController.Instance.Start();
            NetworkController.Instance.NewDataReceived += AddUserAndRefresh;        // Add user
            NetworkController.Instance.MonitorDataReceived += MonitorDataReceived;  // monitor info
           
            NetworkController.Instance.UsbStateChange += AlertUsbStateChange;
            Model.DataAnalizer._Detectusb += AlertDetectUsb;

            comListView.SelectedUserChanged += new View.ComListView.SelectedUserChangedDelegate(SelectedUserChanged);
            capturedImagelistView.GotoEventListView += new View.ImageGridView.GoToEventListViewDelegate(GotoEventListView);
            _currentControl = eventListView1;

            NetworkController.Instance.SearchUninstallComputer();

            connection_history_timer.Interval = Conf.Constant.CONNECTION_HISTORY_INTERVAL * 1000;
            connection_history_timer.Enabled = true;
        }      

        private void Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            //NetworkModule.EndReceive();
            if (!_shouldShutdown)
            {
                oldState = this.WindowState;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                e.Cancel = true;
            }

            Model.UserListManager.Instance.Save(Conf.Constant.USER_INFO_DIC_NAME);
        }


        #region  delegate
        private void AddUserAndRefresh(int indexInUserList)
        {
            
        }
        
        private void PingDataReceived(String physicalAddress, Boolean isRunning)
        {
           
            comListView.AddUserToList(physicalAddress);
            if (isRunning)
                capturedImagelistView.AddUserToList(physicalAddress);
        }
        private void SelectedUserChanged(String user_key)
        {
            _selectedUserKey = user_key;
            eventListView1.SelectedUserChanged(_selectedUserKey);

        }

        private void MonitorDataReceived(String user_key, Conf.Constant.MONITOR_TYPE monitor_type)
        {
            if (_currentControl == eventListView1)
            {
                if (user_key != _selectedUserKey) return;
                eventListView1.Invoke(new RefreshEvnetListDelegate(eventListView1.Review), monitor_type);
            }
            else if (_currentControl == capturedImagelistView && monitor_type == Conf.Constant.MONITOR_TYPE.SCREEN_CAPTURE)
            {
                Model.UserInfo user = Model.UserListManager.Instance[user_key];
                try
                {
                    capturedImagelistView.Invoke(new RefreshThumbImageListDelegate(capturedImagelistView.Review), user_key);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString() + user.UserName);
                }
            }
        }
   
        
    
        private void GotoEventListView(String macAdress)
        {
            splitContainer1.Visible = true;
            capturedImagelistView.Visible = false;
            comListView.SelectUser(macAdress);
            comListView.Select();
            

            eventListView1.SelectedUserChanged(macAdress);
            eventListView1.SelectedTabIndex = 5;
            eventListView1.SelectedTabIndex = (int)Conf.Constant.MONITOR_TYPE.SCREEN_CAPTURE;
            _currentControl = eventListView1;

            image_caputre_timer.Enabled = true;
            thumb_image_capture_timer.Enabled = false;
        }

        private void AlertDetectUsb(String macAddress)
        {
            Model.UserInfo user = Model.UserListManager.Instance[macAddress];
            MessageBox.Show(String.Format("{0} in {1} has plugged flash memory in", user.UserName, user.GroupName);
        }

        private void AlertUsbStateChange(String macAddress, Boolean usbState)
        {
            Model.UserInfo user = Model.UserListManager.Instance[macAddress];
            user.UsbState = usbState;
            comListView.RefreshUserList();
            if (usbState)
                MessageBox.Show(String.Format("{0} in {1} is now allowed for USB port", user.UserName, user.GroupName);
            else
                MessageBox.Show(String.Format("{0} in {1} is now disallowed for USB port", user.UserName, user.GroupName);
        }
        #endregion

        #region NotifyIcon

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (!this.ShowInTaskbar)
            //{
            //    this.WindowState = FormWindowState.Minimized;
            //    this.ShowInTaskbar = true;
            //    this.WindowState = oldState;
            //    this.Activate();
            //}
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!this.ShowInTaskbar)
                {
                    taskbarNotifier1.Show(Resource1.remoteMonitor, Resource1.clickHereToShowMonitor, 300, 3000, 500);
                }
                else
                {
                    taskbarNotifier1.Show(Resource1.remoteMonitor, Resource1.clickHereToHideMonitor, 300, 3000, 500);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                taskbarNotifier2.Show(Resource1.remoteMonitor, Resource1.clickHereToTurnOffMonitor, 300, 3000, 500);
            }
        }

        private void taskbarNotifier1_ContentClick(object sender, EventArgs e)
        {
            if (!this.ShowInTaskbar)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = true;
                this.WindowState = oldState;
                this.Activate();
                taskbarNotifier1.Hide();
            }
            else
            {
                oldState = this.WindowState;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                taskbarNotifier1.Hide();
            }
        }

        private void taskbarNotifier2_ContentClick(object sender, EventArgs e)
        {
            _shouldShutdown = true;
            this.Close();
        }

        #endregion

        #region ToolBar

        //General
        private void generalTool_Click(object sender, EventArgs e)
        {
            splitContainer1.Visible = true;
            capturedImagelistView.Visible = false;
            _currentControl = eventListView1;

            image_caputre_timer.Enabled = true;
            thumb_image_capture_timer.Enabled = false;
        }
        //Thumb Image Grid View
        private void imageViewTool_Click(object sender, EventArgs e)
        {
            splitContainer1.Visible = false;
            capturedImagelistView.Visible = true;
            _currentControl = capturedImagelistView;

            image_caputre_timer.Enabled = false;
            thumb_image_capture_timer.Enabled = true;
        }

        private void settingTool_Click(object sender, EventArgs e)
        {
            View.SettingForm settingForm = new View.SettingForm();
            settingForm.SetInitialValue(Config.GetConfig());
            if (settingForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Config.Data newConfig = settingForm.GetValue();
                if (Config.GetConfig().historyInterval != newConfig.historyInterval)
                {
                    Model.UdpServer.BroadCastingMSG(
                        Conf.NetCommandMessage.CMD_SET_SCREEN_SHOT_INTERVAL,
                        Conf.Constant.UDP_SEND_PORT,
                        newConfig.historyInterval.ToString()
                   );
                }
                Config.LoadConfig(newConfig);
                Config.SaveConfig(newConfig);
            }
        }

        private void updateConfigTool_Click(object sender, EventArgs e)
        {
            //if (File
        }

        #endregion

        private void comListView_Load(object sender, EventArgs e)
        {

        }

        private void connection_history_timer_Tick(object sender, EventArgs e)
        {
            Directory.CreateDirectory(Conf.Constant.CONNECTION_HISTORY_PATH);
            using (FileStream fs = File.OpenWrite(Conf.Constant.CONNECTION_HISTORY_PATH + "\\" + DateTime.Now.ToString("yy-MM-dd") + ".txt"))
            {
                Model.UserListManager userListManager = Model.UserListManager.Instance;
                string running = null, uninstall = null;
                fs.Position = fs.Length;

                foreach (string key in userListManager.AllKeys)
                {
                    Model.UserInfo userInfo = userListManager[key];
                    if (userInfo.State == Model.UserInfo.ClientState.RUNNING) {
                        running += key;
                    } else if (userInfo.State == Model.UserInfo.ClientState.UNINSTALL) {
                        uninstall += key;
                    }
                }

                string buffer = string.Format("{0:d4}", Conf.Constant.CONNECTION_HISTORY_INTERVAL / 60) +
                                DateTime.Now.ToString("HH:mm") + running + " " + uninstall + "\r\n";
                fs.Write(Encoding.UTF8.GetBytes(buffer), 0, buffer.Length);
            }
            Util.Util.ReleaseMemory();
        }
        
        private void user_monitor_timer_Tick(object sender, EventArgs e)
        {
            if (Model.UserListManager.Instance.IsChangedUserState())
            {
                comListView.Invoke(new RefreshUserListDelegate(comListView.RefreshUserList), "");
             
            }

            //monitor uninstall computer
            object obj;
            while ((obj = Controller.NetworkController.Instance.GetDataFromPingBuffer()) != null)
            {
                Model.UserInfo user = Model.UserListManager.Instance[obj.ToString()];
              //  if (user == null) continue;
                PingDataReceived(obj.ToString(), user.State == Model.UserInfo.ClientState.RUNNING);
            }

            //mainStatus.la = Model.UserListManager.Instance.Count + " users are now registered.";
            mainStatus.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            
            mainStatus.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            
            //  
            // toolStripStatusLabel1 
            // 
            label.Name = "toolStripStatusLabel1";
            label.Size = new System.Drawing.Size(109, 17);
            label.Text = String.Format(Resource1.usersAreNowConnected, Model.UserListManager.Instance.Count);

            Util.Util.ReleaseMemory();
        }

        private void image_caputre_timer_Tick(object sender, EventArgs e)
        {
            if (_selectedUserKey == null) return;
            if (eventListView1.SelectedTabIndex == (int)Conf.Constant.MONITOR_TYPE.SCREEN_CAPTURE)
            {
                Model.UserInfo userInfo = Model.UserListManager.Instance[_selectedUserKey];
                if (userInfo == null) return;

                if (userInfo.State != Model.UserInfo.ClientState.RUNNING) return;
                Thread th = new Thread(new ParameterizedThreadStart(RequestImage));
                th.IsBackground = true;
                th.Start(userInfo.Ip);
                
            }
            Util.Util.ReleaseMemory();
        }

        private void RequestImage(Object ip)
        {
            NetworkController.Instance.SendCommand(ip.ToString(), Conf.NetCommandMessage.GET_SCREEN_FULL_IMAGE);
        }
        private void thumb_image_capture_timer_Tick(object sender, EventArgs e)
        {
            foreach (String macAddress in capturedImagelistView._activatedItemMacAddresses)
            {
                Model.UserInfo userInfo = Model.UserListManager.Instance[macAddress];
                if (userInfo == null) return;
                if (userInfo.State != Model.UserInfo.ClientState.RUNNING) continue;
                Thread t = new Thread(new ParameterizedThreadStart(NetworkController.Instance.RequestScreenThumbImageThread));
                t.IsBackground = true;
                t.Start(userInfo.Ip);
                //NetworkController.Instance.SendCommand(userInfo.Ip, Conf.NetCommandMessage.GET_SCREEN_THUMB_IMAGE);
                Thread.Sleep(1000);
            }
            Util.Util.ReleaseMemory();
        }

        private void search_uninstall_timer_Tick(object sender, EventArgs e)
        {
           // NetworkController.Instance.SearchUninstallComputer();
        }

       


    } 
}

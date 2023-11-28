using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Collections;
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Imaging;

namespace Agent.View
{
    public partial class EventListView : UserControl
    {

        String _selectedUserKey = null;

        Model.UserInfo.ClientState[] _connectionHistory = new Model.UserInfo.ClientState[24 * 60];
        ToolTip tooltip = new ToolTip();
       
        public int SelectedTabIndex
        {
            get { return tabControl1.SelectedIndex; }
            set
            {
                tabControl1.SelectedIndex = value;
            }
        }

        public EventListView()
        {
            InitializeComponent();
            imageHistoryView1.SelectedDate = DateTime.Now;
            imageHistoryView1._dateChanged += new ImageHistoryView.DateChangedDelegate(RequestHistoryImage);
       
        }

    
        public void SelectedUserChanged(String physicalAddress)
        {
            if (physicalAddress == null)
                return;

            Model.UserInfo userInfo = Model.UserListManager.Instance[physicalAddress];
            if (userInfo == null) return;

            _selectedUserKey = physicalAddress;

            _userNameTBox.Text = userInfo.UserName;
            _ipAddrTBox.Text = userInfo.Ip;
            _pAddrTBox.Text = userInfo.PhysicalAddress;
            _hostNameTBox.Text = userInfo.Host;
            _installeddateTBox.Text = userInfo.InstalledDate;

            bool isActivated = (userInfo.State == Model.UserInfo.ClientState.RUNNING);

           
            //if (userInfo.State == Model.UserInfo.ClientState.DISABLE) return;
            
            //Get process list
            if (isActivated && Model.UserDataRecords.Instance._processRecords[_selectedUserKey] == null)
            {
                Thread t = new Thread(new ParameterizedThreadStart(GetProcessList));
                t.IsBackground = true;
                t.Start(userInfo.Ip);
                //Controller.NetworkController.Instance.SendCommand(userInfo.Ip, Conf.NetCommandMessage.GET_PROCESS_LIST);
            }
            //Get Screen Image
            if (isActivated && Model.UserDataRecords.Instance._imageRecords[_selectedUserKey] == null)
            {
                Thread t = new Thread(new ParameterizedThreadStart(GetScreenImage));
                t.IsBackground = true;
                t.Start(userInfo.Ip);
                //Controller.NetworkController.Instance.SendCommand(userInfo.Ip, Conf.NetCommandMessage.GET_SCREEN_FULL_IMAGE);
            } 
            //Get USB record
            if (Model.UserDataRecords.Instance._usbRecords[_selectedUserKey] == null)
            {
                Thread t = new Thread(new ParameterizedThreadStart(GetUsbLogFromServer));
                t.IsBackground = true;
                t.Start(userInfo.Ip);
            }
            //Get Insatlled Application List
            if (isActivated && Model.UserDataRecords.Instance._installAppRecords[_selectedUserKey] == null)
            {
                Thread t = new Thread(new ParameterizedThreadStart(GetInstalledApplicationList));
                t.IsBackground = true;
                t.Start(userInfo.Ip);
                //Controller.NetworkController.Instance.SendCommand(userInfo.Ip, Conf.NetCommandMessage.GET_INSTALLED_APPLICATION);
            }
            //Get Device Info List
            if (isActivated && Model.UserDataRecords.Instance._deviceRecords[_selectedUserKey] == null)
            {
                Thread t = new Thread(new ParameterizedThreadStart(GetDeviceInfoList));
                t.IsBackground = true;
                t.Start(userInfo.Ip);
               // Controller.NetworkController.Instance.SendCommand(userInfo.Ip, Conf.NetCommandMessage.GET_DEVICE_INFO);
            }

            if (tabControl1.SelectedIndex == (int)(Conf.Constant.MONITOR_TYPE.CONNECTION_HISTORY))
            {

            }
            //Request image history
            imageHistoryView1.init();
            imageHistoryView1.HideImageView();
            imageHistoryView1.SelectedUserKey = _selectedUserKey;
            if (tabControl1.SelectedIndex == (int)(Conf.Constant.MONITOR_TYPE.SCREEN_HISTORY))
            {
                RequestHistoryImage(imageHistoryView1.SelectedDate);
            }

            if (tabControl1.SelectedIndex == (int)(Conf.Constant.MONITOR_TYPE.CONNECTION_HISTORY))
            {
                RequestConnectionHistory(connectionHistoryDate.Value);
            }
           


            Review(Conf.Constant.MONITOR_TYPE.ALL);
        }

        private void RequestConnectionHistory(DateTime date)
        {
            string fileName = Conf.Constant.CONNECTION_HISTORY_PATH + "\\" + date.ToString("yy-MM-dd") + ".txt";

            for (int i = 0; i < _connectionHistory.Length; i++)
            {
                _connectionHistory[i] = Model.UserInfo.ClientState.DISABLE;
            }

            if (_selectedUserKey == null || !File.Exists(fileName))
            {
                connectionHistoryPanel.Invalidate();
                return;
            }

            try
            {
                using (StreamReader sr = new StreamReader(File.OpenRead(fileName)))
                {
                    while (sr.Peek() >= 0)
                    {
                        try
                        {
                            string log = sr.ReadLine();
                            int interval = int.Parse(log.Substring(0, 4));
                            int hour = int.Parse(log.Substring(4, 2));
                            int min = int.Parse(log.Substring(7, 2));
                            string[] clientPool = log.Substring(9).Split(new char[] { ' ' });
                            int macLength = 17;

                            for (int i = 0; i < clientPool[0].Length / macLength; i++)
                            {
                                if (clientPool[0].Substring(macLength * i, macLength) == _selectedUserKey)
                                {
                                    for (int j = 0; j < interval; j++)
                                    {
                                        _connectionHistory[hour * 60 + min + j] = Model.UserInfo.ClientState.RUNNING;
                                    }
                                    break;
                                }
                            }

                            for (int i = 0; i < clientPool[1].Length / macLength; i++)
                            {
                                if (clientPool[1].Substring(macLength * i, macLength) == _selectedUserKey)
                                {
                                    for (int j = 0; j < interval; j++)
                                    {
                                        _connectionHistory[hour * 60 + min + j] = Model.UserInfo.ClientState.UNINSTALL;
                                    }
                                    break;
                                }
                            }
                        }
                        catch (Exception e) { }
                    }
                }
            }
            catch (Exception e) { }

            connectionHistoryPanel.Invalidate();
        }

        private void GetScreenImage(object ip)
        {
            String ipStr = ip.ToString();

            Controller.NetworkController.Instance.SendCommand(ipStr, Conf.NetCommandMessage.GET_SCREEN_FULL_IMAGE);
        }
        private void GetInstalledApplicationList(object ip)
        {
            String ipStr = ip.ToString();

            Controller.NetworkController.Instance.SendCommand(ipStr, Conf.NetCommandMessage.GET_INSTALLED_APPLICATION);
        }
        private void GetDeviceInfoList(object ip)
        {
            String ipStr = ip.ToString();

            Controller.NetworkController.Instance.SendCommand(ipStr, Conf.NetCommandMessage.GET_DEVICE_INFO);
        }

        private void GetProcessList(object ip)
        {
            String ipStr = ip.ToString();

            Controller.NetworkController.Instance.SendCommand(ipStr, Conf.NetCommandMessage.GET_PROCESS_LIST);
        }
        private void GetUsbLogFromServer(object ip)
        {
            String ipStr = ip.ToString();
            Model.UserInfo user =  Model.UserListManager.Instance.GetComputerFromIP(ip.ToString());
            String macAddress = user.PhysicalAddress;
            bool result = false;
            //First : Request to Server
            if (Controller.NetworkController.Instance._isServerConnected)
                result = Controller.NetworkController.Instance.SendCommand(Conf.Constant.SERVER_IP, Conf.NetCommandMessage.GET_USB_LOG, macAddress); //First : Request to server

            //Second : Request to Client
            if (!result)
            {
                Controller.NetworkController.Instance._isServerConnected = false;
                if (user.State == Model.UserInfo.ClientState.RUNNING)
                    Controller.NetworkController.Instance.SendCommand(ipStr, Conf.NetCommandMessage.LOG_USB);
            }
        }
        #region Display-related
        public void Review(Conf.Constant.MONITOR_TYPE monitor_type)
        {
            if (_selectedUserKey == null)
                return;

            Model.UserDataRecords userDataRecords = Model.UserDataRecords.Instance;

            switch (monitor_type)
            {
                case Conf.Constant.MONITOR_TYPE.SCREEN_CAPTURE:
                    {
                        DisplayScreenImage();
                        break;
                    }
                case Conf.Constant.MONITOR_TYPE.PROCESS_LIST:
                    {
                        DisplayProcessList();
                        break;
                    }
                case Conf.Constant.MONITOR_TYPE.USB_LOG:
                    {
                        DisplayUsbLog();
                        break;
                    }
                case Conf.Constant.MONITOR_TYPE.INSTALLED_APPLICATION_LIST:
                    {
                        DisplayInstalledAppList();
                        break;
                    }
                case Conf.Constant.MONITOR_TYPE.DEVICE_INFO:
                    {
                        DisplayDeviceInfoList();
                        break;
                    }
                case Conf.Constant.MONITOR_TYPE.SCREEN_HISTORY:
                    {
                        AddHistoryImage();
                        break;
                    }
                case Conf.Constant.MONITOR_TYPE.ALL:
                    {
                        _usbLogListView.Items.Clear();
                        DisplayScreenImage();
                        DisplayProcessList();
                        DisplayUsbLog();
                        DisplayInstalledAppList();
                        DisplayDeviceInfoList();
                        break;
                    }

                    
            }
           

        
           
        }

        // Display screen image
        private void DisplayScreenImage()
        {
            
            _screenImageBox.Image = null;
           

            Model.UserDataRecords userDataRecords = Model.UserDataRecords.Instance;
            Model.Record.ImageRecordSet recordImage = (Model.Record.ImageRecordSet)userDataRecords._imageRecords[_selectedUserKey];

            if (recordImage != null)
            {

                Image img = recordImage.RecordImage;
                if (img != null)
                {
                    Image orig_img = _screenImageBox.Image;
                    _screenImageBox.Image = img;

                    if (orig_img != null)
                        orig_img.Dispose();
                }

            }
        }

        //USB history
        private void DisplayUsbLog()
        {
            

            //mut.WaitOne();
            Model.UserDataRecords userDataRecords = Model.UserDataRecords.Instance;
            Model.Record.UsbRecordSet recordUsb = (Model.Record.UsbRecordSet)userDataRecords._usbRecords[_selectedUserKey];
            //mut.ReleaseMutex();

            if (recordUsb != null)
            {
                _usbLogListView.Items.Clear();

                _usbLogListView.BeginUpdate();

                foreach (Model.Record.UsbRecord r in recordUsb.UsbRecords)
                {
                    ListViewItem item = new ListViewItem(r.DevName);
                    item.ImageIndex = 0;
                    item.Tag = r.Id;

                    item.SubItems.Add(r.PlugTime);

                    item.SubItems.Add(r.EjectTime);

                    item.SubItems.Add(r.State.ToString());

                    item.SubItems.Add(r.Content);

                    _usbLogListView.Items.Add(item);
                }
                _usbLogListView.EndUpdate();
            }
        }

        //process
        private void DisplayProcessList()
        {
            _processListView.Items.Clear();
           
            //mut.WaitOne();
            Model.UserDataRecords userDataRecords = Model.UserDataRecords.Instance;
            Model.Record.ProcessRecordSet recordProcess = (Model.Record.ProcessRecordSet)userDataRecords._processRecords[_selectedUserKey];
            //mut.ReleaseMutex();

            if (recordProcess != null)
            {
                _processListView.Items.Clear();

                _processListView.BeginUpdate();

                foreach (Model.Record.ProcessRecord r in recordProcess.ProcessRecords)
                {
                    ListViewItem item = new ListViewItem(r.ProcessName);
                    item.ImageIndex = 0;

                    item.SubItems.Add(r.PID.ToString());

                    item.SubItems.Add(r.Priority);

                    item.SubItems.Add(r.ProcessFullPath);

                    _processListView.Items.Add(item);
                }
                _processListView.EndUpdate();
            }
        }

        // Show Device info
        private void DisplayDeviceInfoList()
        {
            
            _deviceListView.Items.Clear();
            Model.UserDataRecords userDataRecords = Model.UserDataRecords.Instance;
            //mut.WaitOne();
            Model.Record.DeviceRecordSet recordDev = (Model.Record.DeviceRecordSet)userDataRecords._deviceRecords[_selectedUserKey];
            //mut.ReleaseMutex();

            Model.UserInfo userinfo = Model.UserListManager.Instance[_selectedUserKey];

            if (recordDev != null)
            {
                _deviceListView.Items.Clear();

                _deviceListView.BeginUpdate();

                foreach (Model.Record.DeviceRecord r in recordDev.DeviceRecords)
                {
                    if (r.Value.ToLower() == "category")
                        continue;

                    if (r.DeviceName.ToLower() == "computer name")
                    {
                        userinfo.Host = r.Value;
                        _hostNameTBox.Text = userinfo.Host;
                    }

                    ListViewItem item = new ListViewItem(r.DeviceName);

                    item.ImageIndex = 0;

                    item.SubItems.Add(r.Value);

                    _deviceListView.Items.Add(item);
                }
                _deviceListView.EndUpdate();
            }

        }

        //installed app
        private void DisplayInstalledAppList()
        {
            _installAppListView.Items.Clear();
            Model.UserDataRecords userDataRecords = Model.UserDataRecords.Instance;
            //mut.WaitOne();
            Model.Record.ApplicationRecordSet recordApp = (Model.Record.ApplicationRecordSet)userDataRecords._installAppRecords[_selectedUserKey];
            //mut.ReleaseMutex();

            if (recordApp != null)
            {
                _installAppListView.Items.Clear();

                _installAppListView.BeginUpdate();

                foreach (Model.Record.ApplicationRecord r in recordApp.ApplicationRecords)
                {
                    ListViewItem item = new ListViewItem(r.ApplicationName);
                    item.ImageIndex = 0;

                    if (r.InstalledTime.Year == 1)
                        item.SubItems.Add("---");
                    else
                        item.SubItems.Add(r.InstalledTime.ToString());

                    if (r.Capacity == 0)
                        item.SubItems.Add("---");
                    else
                        item.SubItems.Add(r.Capacity.ToString() + "MB");

                    _installAppListView.Items.Add(item);
                }
                _installAppListView.EndUpdate();
            }

        }
        #endregion



        private void RadioCheckScreenImage(object sender, EventArgs e)
        {
            RadioButton btn = (RadioButton)sender;

            switch (btn.Text)
            {
                case "AutoSize":
                    {
                        _screenImageBox.SizeMode = PictureBoxSizeMode.AutoSize;
                        break;
                    }
                case "Center Image":
                    {
                        _screenImageBox.SizeMode = PictureBoxSizeMode.CenterImage;
                        break;
                    }
                case "Stretch Image":
                    {
                        _screenImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        break;
                    }
                case "Zoom":
                    {
                        _screenImageBox.SizeMode = PictureBoxSizeMode.Zoom;
                        break;
                    }
            }
        }

        private void _installAppListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            _installAppListView.Sorting = SortOrder.Ascending;
            _installAppListView.Sort();
        }

       

        private void _processListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (((ListView)sender).GetItemAt(e.X, e.Y) != null)
                {
                    ContextMenuStrip menu = new ContextMenuStrip();
                    menu.Name = "killProcessMenu";
                    ToolStripItem item = new ToolStripMenuItem();
                    item.Text = "Kill Process";
                    item.Click += new EventHandler(item_Click);
                    menu.Items.Add(item);
                    //((Control)sender).ContextMenuStrip = menu;

                    menu.Show((Control)sender, e.X, e.Y);

                }
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            if (_selectedUserKey == null) return;
            Model.UserInfo userinfo = Model.UserListManager.Instance[_selectedUserKey];
            if (_processListView.SelectedItems.Count == 0) return;

            String pidstr = "";
            foreach (ListViewItem item in _processListView.SelectedItems)
            {
                pidstr += item.SubItems[1].Text + ",";
            }

            pidstr.Remove(pidstr.Length - 1);
            Controller.NetworkController.Instance.SendCommand(userinfo.Ip, Conf.NetCommandMessage.CMD_KILL_PROCESS, System.Text.Encoding.Unicode.GetBytes(pidstr));

        }

        #region Image history View Related

        private void historyImageWork(object date)
        {
            DateTime d = (DateTime)date;
            string path = Conf.Constant.HISTORY_SAVE_PATH + "\\" +
                            d.ToString("yy") + "\\" +
                            d.ToString("MM") + "\\" +
                            d.ToString("dd") + "\\" +
                            _ipAddrTBox.Text + "\\";

            if (Directory.Exists(path))
            {
                IEnumerable<string> images = Directory.EnumerateFiles(path);


                foreach (string image in images)
                {
                    List<byte> buffer = new List<byte>();
                    // type
                    buffer.AddRange(BitConverter.GetBytes(Conf.NetCommandMessage.GET_SCREEN_IMAGE_HISTORY));

                    // mac addr
                    for (int i = 0; i < 6; i++)
                    {
                        buffer.Add(byte.Parse(_selectedUserKey.Substring(i * 3, 2), System.Globalization.NumberStyles.HexNumber));
                    }

                    try
                    {
                        // date time

                        int p = image.LastIndexOf('\\');
                        int hour = int.Parse(image.Substring(p + 1, 2));
                        int min = int.Parse(image.Substring(p + 4, 2));
                        int sec = int.Parse(image.Substring(p + 7, 2));
                        DateTime dt = new DateTime(d.Year, d.Month, d.Day, hour, min, sec);
                        buffer.AddRange(Encoding.Unicode.GetBytes(dt.ToString("MM/dd/yy HH:mm:ss")));

                        // image data
                        using (FileStream fs = File.OpenRead(image))
                        {
                            byte[] bytes = new byte[fs.Length];
                            int numBytesToRead = (int)fs.Length;
                            int numBytesRead = 0;
                            while (numBytesToRead > 0)
                            {
                                // Read may return anything from 0 to numBytesToRead. 
                                int n = fs.Read(bytes, numBytesRead, numBytesToRead);

                                // Break when the end of the file is reached. 
                                if (n == 0)
                                    break;

                                numBytesRead += n;
                                numBytesToRead -= n;
                            }
                            buffer.AddRange(bytes);
                        }

                        Controller.NetworkController.Instance.Analize(buffer, null);
                    }
                    catch (Exception e) { }
                }
                //
            }
            imageHistoryView1.imageListView.Invoke((MethodInvoker)(() => { imageHistoryView1.imageListView.EndUpdate(); }));
        }
        public void RequestHistoryImage(DateTime date)
        {
            Thread t = new Thread(new ParameterizedThreadStart(historyImageWork));
            imageHistoryView1.imageListView.BeginUpdate();
            t.IsBackground = true;
            t.Start(date);
            //if (Controller.NetworkController.Instance._isServerConnected)
            //    Controller.NetworkController.Instance._isServerConnected = Controller.NetworkController.Instance.SendCommand(Conf.Constant.SERVER_IP, Conf.NetCommandMessage.GET_SCREEN_IMAGE_HISTORY, data);
        }

        private void ImageHistoryLoadFinish()
        {

        }
        public void AddHistoryImage()
        {
            Model.Record.ImageRecordSet imgRecord;
            while ((imgRecord = Model.UserDataRecords.Instance.GetImageFromQueue()) != null)
            {
                if (imgRecord.PhysicalAddress == imageHistoryView1.SelectedUserKey)
                    imageHistoryView1.AddImage(imgRecord);
            }
        }
        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case ((int)Conf.Constant.MONITOR_TYPE.SCREEN_HISTORY):
                    if (!imageHistoryView1.isEmpty) return;
                    RequestHistoryImage(imageHistoryView1.SelectedDate);
                    break;

                case ((int)Conf.Constant.MONITOR_TYPE.CONNECTION_HISTORY):
                    RequestConnectionHistory(connectionHistoryDate.Value);
                    break;
            }
        }

        private void connectionHistoryDate_ValueChanged(object sender, EventArgs e)
        {
            RequestConnectionHistory(((DateTimePicker)sender).Value);
        }

        private void connectionHistoryPanel_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Font font = new Font("Arial", 9);
            SolidBrush textBrush = new SolidBrush(Color.Black);

            Brush[] rectBrushes = new Brush[] { new SolidBrush(Color.DarkGreen), new SolidBrush(Color.LightGray), new SolidBrush(Color.Red) };

            Panel canvas = (Panel)sender;

            for (int i = 0; i < 12; i++){
                e.Graphics.DrawString(string.Format("{0:d2} AM", i), font, textBrush, new PointF(10, 30 + i * 17));
            }

            for (int i = 0; i < 12; i++)
            {
                e.Graphics.DrawString(string.Format("{0:d2} PM", i == 0 ? 12 : i), font, textBrush, new PointF(10, 30 + (i + 12) * 17));
            }

            for (int i = 0; i < 60; i++)
            {
                e.Graphics.DrawString(string.Format("{0:d2}", i), font, textBrush, new PointF(60 + i * 17, 10));
            }
            for (int i = 0; i < _connectionHistory.Length; i++)
            {
                Rectangle rect = new Rectangle(60 + (i % 60) * 17, 30 + (i / 60) * 17, 17, 17);
                e.Graphics.FillRectangle(rectBrushes[(int)_connectionHistory[i]], rect);
            }
        }

        private void connectionHistoryPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X >= 60 && e.X < 60 + 60 * 17 && e.Y >= 30 && e.Y < 30 + 24 * 17) {
                int hour = (e.Y - 30) / 17;
                int min = (e.X - 60) / 17;
                Model.UserInfo.ClientState state = _connectionHistory[hour * 60 + min];
                string stateString = state == Model.UserInfo.ClientState.RUNNING ? "OK"
                    : state == Model.UserInfo.ClientState.UNINSTALL ? "Uninstalled"
                    : "Offline";
                tooltip.Show(String.Format("{0:d2}:{1:d2} {2}", hour, min, stateString), (Panel)sender, e.X, e.Y + 20);
            }
            else
            {
                tooltip.Hide((Panel)sender);
            }
        }
    }
}

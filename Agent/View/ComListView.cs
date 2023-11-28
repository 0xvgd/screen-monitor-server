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
using System.Collections.Specialized;
using System.Net.Sockets;
using System.Collections;

namespace Agent.View
{
    public partial class ComListView : UserControl
    {

        public delegate void SelectedUserChangedDelegate(String user_key);
        public event SelectedUserChangedDelegate SelectedUserChanged;

        public delegate void DeleteUserDelegate(List<String> user_keys);
        public event DeleteUserDelegate DeleteUser = null;

        private ListViewItem _selectedItem = null;

        private Hashtable _groupTable = new Hashtable();
        private const string DEFAULT_GROUP_NAME = "Default";




        public String SelectedItemIp
        {
            get
            {
                return _selectedItem.Name;
            }
        }

        public ComListView()
        {
            InitializeComponent();

            
        }
        

       
        /// <summary>
        /// Add new user to ComListView
        /// </summary>
        public void AddUserToList(String physicalAddress)
        {

            Model.UserListManager manager = Model.UserListManager.Instance;

            Model.UserInfo info = manager[physicalAddress];

          
            //check if item whoes name is info.ip is exist
            ListViewItem[] itemList = comList.Items.Find(info.Ip, false);
            ListViewItem item;
          
            if (itemList != null && itemList.Length > 0)
            {
                item = itemList[0];


                //state change
                if (!(item.ImageIndex == (int)Model.UserInfo.ClientState.RUNNING && info.State ==  Model.UserInfo.ClientState.UNINSTALL))
                    item.ImageIndex = (int)info.State;

           
                //information change
                if (info.State != Model.UserInfo.ClientState.UNINSTALL)
                {
                   item.Text = info.UserName;
                    item.Name = info.Ip;
                    ImageButton usbButton = ((ImageButton)(comList.GetEmbeddedControl(item.Tag.ToString())));
               
                    usbButton.SetEnable(true);
                    usbButton.SetOnState(info.UsbState, info.State == Model.UserInfo.ClientState.RUNNING);
                }
               
                return;
            }
            item = new ListViewItem(info.UserName);

            item.Text = info.UserName;
            item.Name = info.Ip;
            item.ImageIndex = (int)info.State;
       
            if (_groupTable.ContainsKey(info.GroupName))
            {
                item.Group = (ListViewGroup)_groupTable[info.GroupName];
            }
            else
            {
                ListViewGroup grp = new ListViewGroup(info.GroupName);
                item.Group = grp;
                _groupTable.Add(info.GroupName, grp);
                comList.Groups.Add(grp);
            }
            
            ///
            item.Tag = info.PhysicalAddress;
            ///
            item.SubItems.Add(info.Ip); 
           
           comList.Items.Add(item);

          // if (info.State == Model.UserInfo.ClientState.RUNNING)
           {
               //add usb button
               ImageButton _usbOnOffBtn = new ImageButton();
               _usbOnOffBtn.SetImages(Resource1.usbon, Resource1.usbon_clicked, Resource1.usboff, Resource1.usboff_clicked, Resource1.usbondisable, Resource1.usboffdisable);
               _usbOnOffBtn.setClickDelegate(UsbOnOffBtnClicked);

               Size size = comList.LargeImageList.ImageSize;
               _usbOnOffBtn.Size = new Size(10, 10);//((int)(size.Width * 0.5f), (int)(size.Height * 0.5f));
               _usbOnOffBtn.Tag = item.Tag;

               if (info.State == Model.UserInfo.ClientState.RUNNING)
                   _usbOnOffBtn.SetOnState(info.UsbState, true);
               else
                   _usbOnOffBtn.SetOnState(info.UsbState, false);
               comList.AddEmbeddedControl(_usbOnOffBtn, 2, item.Index);
 
           }
            comList.Refresh();            
        }
       
        public void SelectUser(String ipAddress)
        {
            if (_selectedItem != null)
            {
                _selectedItem.Focused = false;
                _selectedItem.Selected = false;
            }
            ListViewItem item = comList.Items[comList.Items.IndexOfKey(ipAddress)];
            if (item == null) return;
            item.Focused = true;
            item.Selected = true;

        }
        public void DeleteUsers(List<String> user_keys)
        {
            foreach (String user_key in user_keys)
            {
                comList.Items.RemoveByKey(user_key);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void RefreshUserList(String key = "")
        {
            Model.UserListManager manager = Model.UserListManager.Instance;
            comList.BeginUpdate();
            for (int i = 0; i < comList.Items.Count; i++)
            {
                ListViewItem item = comList.Items[i];
                Model.UserInfo userinfo = manager[item.Tag.ToString()];
                if (userinfo == null) continue;

               
                item.ImageIndex = (int)userinfo.State;
                item.SubItems[0].Text = userinfo.UserName;
                ImageButton usbButton = ((ImageButton)(comList.GetEmbeddedControl(item.Tag.ToString())));
                if (usbButton == null) continue;


                usbButton.SetOnState(userinfo.UsbState);

                if (userinfo.State == Model.UserInfo.ClientState.RUNNING)
                    usbButton.SetOnState(userinfo.UsbState, true);
                else
                    usbButton.SetOnState(userinfo.UsbState, false);

            }
            comList.EndUpdate();
        }
        private void comList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection lv = ((ListView)sender).SelectedItems;
            if (lv.Count == 1)
            {
                _selectedItem = lv[0];

                if (SelectedUserChanged != null)
                {
                    SelectedUserChanged(_selectedItem.Tag.ToString());
                }
            }
        }


        private void comList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewItem item = ((ListView)sender).GetItemAt(e.X, e.Y);
                if (item == null) return;

                Model.UserInfo user = Model.UserListManager.Instance[item.Tag.ToString()];
                if (user.State == Model.UserInfo.ClientState.RUNNING)
                        itemMenu.Show((Control)sender, e.X, e.Y);
            }

        }

        private void chageUserNameMenuItem_Click(object sender, EventArgs e)
        {
            
        }

       
        internal void ManageGroupBox()
        {
           
        }

        internal void Regroup()
        {
           
        }

        private void ChangeGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ComListView_Load(object sender, EventArgs e)
        {
           
        }

        private void comList_Enter(object sender, EventArgs e)
        {
            _comListViewPanel.BackColor = SystemColors.Highlight;
        }

        private void comList_Leave(object sender, EventArgs e)
        {
            _comListViewPanel.BackColor = SystemColors.ControlDark;
        }

        private void _comListViewPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _comListViewPanel.BackColor = SystemColors.Highlight;
        }

        #region "Popup menu"
        private void shutdownMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Continue power off? ", @"Power off", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                foreach (ListViewItem selectedItem in comList.SelectedItems)
                {
                    Model.UserInfo user = Model.UserListManager.Instance[selectedItem.Tag.ToString()];
                    Controller.NetworkController.Instance.SendCommand(user.Ip, Conf.NetCommandMessage.CMD_SHUTDOWN_REQ);
                }
            }
        }

        private void logoffMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"LogOff? ", @"LogOff", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                foreach (ListViewItem selectedItem in comList.SelectedItems)
                {
                    Model.UserInfo user = Model.UserListManager.Instance[selectedItem.Tag.ToString()];
                    Controller.NetworkController.Instance.SendCommand(user.Ip, Conf.NetCommandMessage.CMD_LOGOFF_REQ);
                }
            }
        }

        private void restartMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Continue reboot? ", @"Reboot", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                foreach (ListViewItem selectedItem in comList.SelectedItems)
                {
                    Model.UserInfo user = Model.UserListManager.Instance[selectedItem.Tag.ToString()];
                    Controller.NetworkController.Instance.SendCommand(user.Ip, Conf.NetCommandMessage.CMD_RESTART_REQ);
                }
            }
        }

        private void removePasswordMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Remove password? ", @"Password Remove", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                foreach (ListViewItem selectedItem in comList.SelectedItems)
                {
                    string cmd = "net user Administrator " + "\"\"";
                    Model.UserInfo user = Model.UserListManager.Instance[selectedItem.Tag.ToString()];
                    Controller.NetworkController.Instance.SendCommand(user.Ip, Conf.NetCommandMessage.CMD_EXEC_COMMON_COMMAND, cmd);
                }
            }
        }

        private void changePasswordMenuItem_Click(object sender, EventArgs e)
        {
           
        }
        private void sendMessageToolStrip_Click(object sender, EventArgs e)
        {
            MessageForm messageForm = new MessageForm();
            messageForm.SendMessageToUser += new MessageForm.SendMessageToUserDelegate(sendMessageToUser);
            messageForm.Show();
        }

        private void sendMessageToUser(String content)
        {
            foreach (ListViewItem selectedItem in comList.SelectedItems)
            {
                Model.UserInfo user = Model.UserListManager.Instance[selectedItem.Tag.ToString()];
                Controller.NetworkController.Instance.SendCommand(user.Ip, Conf.NetCommandMessage.CMD_SENDMESSAGE_REQ, content);
            }
        }
        #endregion

        private void usbOpenMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in comList.SelectedItems)
            {
                Model.UserInfo user = Model.UserListManager.Instance[selectedItem.Tag.ToString()];
                if (user.State != Model.UserInfo.ClientState.RUNNING) continue;
                Controller.NetworkController.Instance.SendCommand(user.Ip, Conf.NetCommandMessage.CMD_USBON_REQ);
                //user.UsbState = true;
               // selectedItem.BackColor = Conf.Constant.USB_OFF_COLOR;
                ImageButton usbButton = ((ImageButton)(comList.GetEmbeddedControl(selectedItem.Tag.ToString())));
                if (usbButton == null) continue;

               //usbButton.SetOnState(true); ;
               
                
            }
          
        }

        private void uspCloseMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in comList.SelectedItems)
            {
                Model.UserInfo user = Model.UserListManager.Instance[selectedItem.Tag.ToString()];
                if (user.State != Model.UserInfo.ClientState.RUNNING) continue;
                Controller.NetworkController.Instance.SendCommand(user.Ip, Conf.NetCommandMessage.CMD_USBOFF_REQ);
                //user.UsbState = false;

                ImageButton usbButton = ((ImageButton)(comList.GetEmbeddedControl(selectedItem.Tag.ToString())));
                if (usbButton == null) continue;
                //usbButton.SetOnState(false); ;
            }
        }

        private void remoteExecToolStrip_Click(object sender, EventArgs e)
        {
            
        }

        private void ComListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                Controller.NetworkController.Instance.SearchAllClient();
               // Controller.NetworkController.Instance.SearchUninstallComputer();
                Controller.NetworkController.Instance.TryConnectServerThread();
                
            }
            else if (e.KeyCode == Keys.Delete)
            {
                List<String> deletedUserKeys = new List<String>();
                foreach (ListViewItem selectedItem in comList.SelectedItems)
                {
                    Model.UserInfo user = Model.UserListManager.Instance[selectedItem.Tag.ToString()];
                    if (user.State != Model.UserInfo.ClientState.RUNNING) continue;
                    deletedUserKeys.Add(user.PhysicalAddress);
                }
                if (DeleteUser != null)
                    DeleteUser(deletedUserKeys);
            }
        }


        private void UsbOnOffBtnClicked(object sender)
        {
            String tag = ((ImageButton)sender).Tag.ToString();
            Model.UserInfo user = Model.UserListManager.Instance[tag];
            if (!((ImageButton)sender).GetState() )
            {
                Controller.NetworkController.Instance.SendCommand(user.Ip, Conf.NetCommandMessage.CMD_USBON_REQ);
                //user.UsbState = true;
            }
            else
            {
                Controller.NetworkController.Instance.SendCommand(user.Ip, Conf.NetCommandMessage.CMD_USBOFF_REQ);
                //user.UsbState = false;
            }
         
        }

        
       

       
    }
}

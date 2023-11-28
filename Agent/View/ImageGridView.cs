using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace Agent.View
{
    public partial class ImageGridView : UserControl
    {

       
        private Hashtable _groupTable = new Hashtable();
        private const string DEFAULT_GROUP_NAME = "Default";

        public delegate void GoToEventListViewDelegate(String macAdress);
        public event GoToEventListViewDelegate GotoEventListView = null;

        
        List<ListViewItem> _myCache;
        int firstItem = 0;

        int buffer_size;


        public List<String> _activatedItemMacAddresses;

       
        Thread _image_capture_timer_thread = null;
        int _unrefreshedItemCnt = 0;
        public Queue<String> _refreshQueue = new Queue<string>();


        public ImageGridView()
        {
            _activatedItemMacAddresses = new List<String>();
            _myCache = new List<ListViewItem>();

            InitializeComponent();

            imageListView.VirtualListSize = 0;

            capturedImageList.Images.Clear();

            buffer_size = Conf.Constant.ImageGirdSize.Width * Conf.Constant.ImageGirdSize.Height * 2;

            _image_capture_timer_thread = new Thread(new ThreadStart(RequestThumbImage));
            _image_capture_timer_thread.IsBackground = true;
            _image_capture_timer_thread.Start();
            _image_capture_timer_thread.Suspend();
            
          
        }
        


        private void init()
        {
            capturedImageList.ImageSize = Conf.Constant.Thumb_IMAGE_SIZE;
           
            Review();
        
        }

        private void RequestThumbImage()
        {
            int index = 0;
            while (true)
            {
                if (_activatedItemMacAddresses.Count == 0) continue;
                if (index == _activatedItemMacAddresses.Count)
                {
                    Thread.Sleep(2000);
                    index = 0;
                }
              
                Model.UserInfo userInfo = Model.UserListManager.Instance[_activatedItemMacAddresses[index]];

                if (userInfo == null || userInfo.State != Model.UserInfo.ClientState.RUNNING)
                {
                    index++;
                    continue;
                }
                Thread t = new Thread(new ParameterizedThreadStart(Controller.NetworkController.Instance.RequestScreenThumbImageThread));
                t.IsBackground = true;
                t.Start(userInfo.Ip);
                Thread.Sleep(500);
                index++;
               
            }
        }

        int CompareListViewItem(ListViewItem item1, ListViewItem item2)
        {
            String str1 = item1.Text.Trim(' ');
            String str2 = item2.Text.Trim(' ');
            return str1.CompareTo(str2);
        }
        /// <summary>
        /// Add new user to ComListView
        /// </summary>
        public void AddUserToList(String physicalAddress)
        {
            Model.UserListManager manager = Model.UserListManager.Instance;

            Model.UserInfo info = manager[physicalAddress];

            if (info.State != Model.UserInfo.ClientState.RUNNING) return;

            ListViewItem[] itemList = imageListView.Items.Find(info.Ip, false);

            if (itemList != null && itemList.Length > 0) return;


            ListViewItem item = new ListViewItem(info.UserName, _myCache.Count);
            item.Name = info.Ip;
            Bitmap image = Model.UserDataRecords.Instance._imageRecords[info.PhysicalAddress] != null ? (Bitmap)((Model.Record.ImageRecordSet)(Model.UserDataRecords.Instance._imageRecords[info.PhysicalAddress])).RecordImage : new Bitmap(256, 256);

            capturedImageList.Images.Add(image);


            if (_groupTable.ContainsKey(info.GroupName))
            {
                item.Group = (ListViewGroup)_groupTable[info.GroupName];
            }
            else
            {
                ListViewGroup grp = new ListViewGroup(info.GroupName);
                item.Group = grp;
                _groupTable.Add(info.GroupName, grp);
                imageListView.Groups.Add(grp);
            }

            ///
            item.Tag = info.PhysicalAddress;
            ///

            _myCache.Add(item);

            imageListView.VirtualListSize++;
            _myCache.Sort(new Comparison<ListViewItem>(CompareListViewItem));
        }
            
       
       
        private void ImageGridView_ClientSizeChanged(object sender, EventArgs e)
        {
            Size imageListViewSize = Screen.PrimaryScreen.Bounds.Size;
            Conf.Constant.Thumb_IMAGE_SIZE.Width = (imageListViewSize.Width - 10) / Conf.Constant.ImageGirdSize.Width;
            Conf.Constant.Thumb_IMAGE_SIZE.Height = (imageListViewSize.Height - 100) / Conf.Constant.ImageGirdSize.Height;

            Conf.Constant.Thumb_IMAGE_SIZE.Width = Conf.Constant.Thumb_IMAGE_SIZE.Width > 256 ? 256 : Conf.Constant.Thumb_IMAGE_SIZE.Width;
            Conf.Constant.Thumb_IMAGE_SIZE.Height = Conf.Constant.Thumb_IMAGE_SIZE.Height > 256 ? 256 : Conf.Constant.Thumb_IMAGE_SIZE.Height;
            init();
        }

        private void ImageGridView_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                _image_capture_timer_thread.Resume();
            else
                _image_capture_timer_thread.Suspend();
        }

        private void imageListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (_myCache != null && e.ItemIndex >= firstItem && e.ItemIndex < firstItem + _myCache.Count)
            {
                //A cache hit, so get the ListViewItem from the cache instead of making a new one.
                e.Item = _myCache[e.ItemIndex - firstItem];
            }
            else
            {
                //A cache miss, so create a new ListViewItem and pass it back. 
                int x = e.ItemIndex;
                e.Item = new ListViewItem(x.ToString(), x);
                _myCache.Add(e.Item);
            }

            if (_activatedItemMacAddresses.Contains(e.Item.Tag.ToString())) return;
            if (_activatedItemMacAddresses.Count == buffer_size) _activatedItemMacAddresses.RemoveAt(0);
            _activatedItemMacAddresses.Add(e.Item.Tag.ToString());

          

        }

        private void imageListView_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            //We've gotten a request to refresh the cache. 
            //First check if it's really neccesary. 
            if (_myCache != null && e.StartIndex >= firstItem && e.EndIndex <= firstItem + _myCache.Count)
            {
                //If the newly requested cache is a subset of the old cache,  
                //no need to rebuild everything, so do nothing. 
                return;
            }


        }

      

        public void Review(String physicalAddress)
        {
            _refreshQueue.Enqueue(physicalAddress);
            if (_refreshQueue.Count < 5) return;

            Model.UserDataRecords userDataRecords = Model.UserDataRecords.Instance;

            imageListView.BeginUpdate();
            while (_refreshQueue.Count > 0)
            {
                String macAddress = _refreshQueue.Dequeue();
                Model.UserInfo user = Model.UserListManager.Instance[macAddress];
                Model.Record.ImageRecordSet recordImage = (Model.Record.ImageRecordSet)userDataRecords._imageRecords[macAddress];


                if (recordImage != null)
                {

                    Bitmap img = new Bitmap(recordImage.RecordImage, Conf.Constant.Thumb_IMAGE_SIZE);
                    int index = imageListView.Items.IndexOfKey(user.Ip);
                    ListViewItem item = imageListView.Items[index];
                    index = item.ImageIndex;


                    if (index == -1)
                    {
                        AddUserToList(macAddress);
                        index = imageListView.Items.IndexOfKey(user.Ip);
                        item = imageListView.Items[index];
                        index = item.ImageIndex;
                    }

                    Image orig_img = capturedImageList.Images[index];
                    capturedImageList.Images[index] = img;

                    if (orig_img != null)
                        orig_img.Dispose();
                }
            }
            imageListView.EndUpdate();
       
           
        }

        public void Review()
        {
            Model.UserDataRecords userDataRecords = Model.UserDataRecords.Instance;
            // Show Screen Image
            imageListView.BeginUpdate();
            for (int i = 0; i < _activatedItemMacAddresses.Count; i++)
            {
                String macAddress = _activatedItemMacAddresses[i];

                Model.UserInfo user = Model.UserListManager.Instance[macAddress];
                Model.Record.ImageRecordSet recordImage = (Model.Record.ImageRecordSet)userDataRecords._imageRecords[macAddress];


                if (recordImage != null)
                {

                    Bitmap img = new Bitmap(recordImage.RecordImage, Conf.Constant.Thumb_IMAGE_SIZE);
                    int index = imageListView.Items.IndexOfKey(user.Ip);
                    ListViewItem item = imageListView.Items[index];
                    index = item.ImageIndex;

                    if (index == -1)
                    {
                        AddUserToList(macAddress);
                        index = imageListView.Items.IndexOfKey(user.Ip);
                        item = imageListView.Items[index];
                        index = item.ImageIndex;
                    }

                    Image orig_img = capturedImageList.Images[index];
                    capturedImageList.Images[index] = img;

                    if (orig_img != null)
                        orig_img.Dispose();
                }


            }
            imageListView.EndUpdate();


        }

       
        private void imageListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //find clicked item
            ListViewItem item = imageListView.GetItemAt(e.X, e.Y);
            //go to eventlistview
            if (GotoEventListView != null)
                GotoEventListView(item.Name.ToString());
            
        }

      

    }
}

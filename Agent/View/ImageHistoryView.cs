using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Agent.View
{
    public partial class ImageHistoryView : UserControl
    {

        public delegate void DateChangedDelegate(DateTime date);
        public DateChangedDelegate _dateChanged = null;

        List<ListViewItem> _myCache;
        List<Image> _images;
        int firstItem = 0;

        DateTime _selectedDate;
        String _selectedUserKey;

        public Boolean isEmpty
        {
            get { return imageListView.Items.Count == 0; }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; }
        }
        
        public String SelectedUserKey
        {
            get { return _selectedUserKey; }
            set { _selectedUserKey = value; }
        }

        public ImageHistoryView()
        {
            InitializeComponent();

            _myCache = new List<ListViewItem>();
            _images = new List<Image>();
            image_panel.Hide();
            init();
        }

        public void init()
        {
            imageListView.VirtualListSize = 0;
            _myCache.Clear();
            _images.Clear();
            largeImgeList.Images.Clear();
        }
        public void AddImage(Model.Record.ImageRecordSet imageRecord)
        {
            
            largeImgeList.Images.Add(imageRecord.RecordImage);
            _images.Add(imageRecord.RecordImage);
            ListViewItem item = new ListViewItem(imageRecord.Time.ToString("HH:mm:ss"), largeImgeList.Images.Count - 1);
            _myCache.Add(item);
            _myCache.Sort(compareListItem);
            imageListView.VirtualListSize++;
        }

        private int compareListItem(ListViewItem item1, ListViewItem item2)
        {
            return  item1.Text.CompareTo(item2.Text);
           
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

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime date = dateTimePicker.Value;
            _selectedDate = date;
            init();
            if (_dateChanged != null)
                _dateChanged(date);
                
        }

        private void imageListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
             ListView.SelectedIndexCollection lv = ((ListView)sender).SelectedIndices;
             if (lv.Count == 1)
             {
                 image_panel.Show();
                 ((Label)image_panel.Controls["title"]).Text = "Printed at " + _myCache[lv[0]].Text;
                 int image_index = _myCache[lv[0]].ImageIndex;
                 ((PictureBox)image_panel.Controls["image"]).Image = _images[image_index];
             }
        }

    

        private void image_DoubleClick(object sender, EventArgs e)
        {
            image_panel.Hide();
        }


        public void HideImageView()
        {
            image_panel.Hide();
        }
    }
}

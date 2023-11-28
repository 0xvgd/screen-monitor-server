using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Agent.Model.Record
{
    public class UsbRecordSet:DataRecordSet
    {
        [XmlElement(typeof(ApplicationRecord), ElementName = "Usblog", IsNullable = true)]
        public List<UsbRecord> UsbRecords = null;

        public void AddRecordSet(UsbRecordSet newRecordSet)
        {

            foreach (UsbRecord newRecord in newRecordSet.UsbRecords)
            {
                 UsbRecord[] list = (from q in UsbRecords
                               where q.Id == newRecord.Id
                               select q).ToArray();
                 if (list != null && list.Count() > 0)
                 {
                     list[0].EjectTime = newRecord.EjectTime;
                     continue;
                 }
                 UsbRecords.Add(newRecord);
            }
        }
    }

    public class UsbRecord
    {
        private string _id;
        private string _devname  = string.Empty;
        private string _plugtime = string.Empty;
        private string _ejecttime = string.Empty;
        private bool _state = false;
        private string _content = string.Empty;

        [XmlElement(typeof(int), ElementName = "id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string DevName
        {
            get { return _devname; }
            set { _devname = value; }
        }

        public string PlugTime
        {
            get { return _plugtime; }
            set { _plugtime = value; }
        }
        public string EjectTime
        {
            get { return _ejecttime; }
            set { _ejecttime = value; }
        }
        public Boolean State
        {
            get { return _state; }
            set { _state = value; }
        }
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        
    }
}

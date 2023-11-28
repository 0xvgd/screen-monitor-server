using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Agent.Model.Record
{
    public class DeviceRecordSet:DataRecordSet
    {
        [XmlElement(typeof(ApplicationRecord), ElementName = "DeviceRecords", IsNullable = true)]
        public List<DeviceRecord> DeviceRecords = null;
    }

    public class DeviceRecord
    {
        private string _deviceName = string.Empty;
        private string _value = string.Empty;

        [XmlElement(typeof(string), ElementName = "DeviceName")]
        public string DeviceName
        {
            get { return _deviceName; }
            set { _deviceName = value; }
        }

        [XmlElement(typeof(string), ElementName = "Value")]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}

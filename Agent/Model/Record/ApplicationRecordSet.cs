using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Agent.Model.Record
{
    public class ApplicationRecordSet:DataRecordSet
    {
        [XmlElement(typeof(ApplicationRecord), ElementName="ApplicationRecords", IsNullable = true)]
        public List<ApplicationRecord> ApplicationRecords = null;
    }

    public class ApplicationRecord
    {
        private string _applicationName = string.Empty;
        private UInt64 _capacity;
        private DateTime _installedTime;

        [XmlElement(typeof(string), ElementName = "ApplicationName")]
        public string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        [XmlElement(typeof(UInt64), ElementName = "Capacity")]
        public UInt64 Capacity
        {
            get { return _capacity; }
            set { _capacity = value; }
        }

        [XmlElement(typeof(DateTime), ElementName="InstalledTime", IsNullable=true)]
        public DateTime InstalledTime
        {
            get { return _installedTime; }
            set { _installedTime = value; }
        }
    }
}

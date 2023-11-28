using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Agent.Model.Record
{
    public class ProcessRecordSet : DataRecordSet
    {
        [XmlElement(typeof(ProcessRecord), ElementName = "ProcessRecords", IsNullable = true)]
        public List<ProcessRecord> ProcessRecords = null;
    }

    public class ProcessRecord 
    {
        private string _processName = string.Empty;
        private string _processFullPath = string.Empty;
        private UInt64 _pID;
        private string _priority;

        [XmlElement(typeof(string), ElementName = "ProcessName")]
        public string ProcessName
        {
            get { return _processName; }
            set { _processName = value; }
        }

        [XmlElement(typeof(string), ElementName = "ProcessFullPath")]
        public string ProcessFullPath
        {
            get { return _processFullPath; }
            set { _processFullPath = value; }
        }

        [XmlElement(typeof(UInt64), ElementName = "PID")]
        public UInt64 PID
        {
            get { return _pID; }
            set { _pID = value; }
        }

        [XmlElement(typeof(string), ElementName = "Priority")]
        public string Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Agent.Model.Record
{
    public class DataRecordSet
    {
        private string _physicalAddress = string.Empty;
        private DateTime _time;

        [XmlElement(typeof(string), ElementName = "PhysicalAddress")]
        public string PhysicalAddress
        {
            get { return _physicalAddress; }
            set { _physicalAddress = value; }
        }

        [XmlElement(typeof(DateTime), ElementName = "Time")]
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;





namespace Agent.Model
{
    [Serializable]

    public class UserInfo : ISerializable

    {
        public enum ClientState { RUNNING, DISABLE, UNINSTALL };
        

        private string _ip;
        private string _physicalAddress;                               
        private string _userName;

        private string _host;
        private string _groupName;
        private string _installedDate;
        private ClientState _state = ClientState.DISABLE;
        private Boolean _usbAllowed = false;

        private double _alive_record_time;

        
        public UserInfo()
        {
            _ip = "unknown";
            _host = "unknown";
            _physicalAddress = "unknown";
            _userName = "unknown";
            _groupName = "Default Group";
            _alive_record_time = 0;
        }

        public UserInfo(string ip, string host, string physicalAddress, string userName, string groupName)
        {
            _ip = ip;
            _host = host;
            _physicalAddress = physicalAddress;
            _userName = userName;
            _groupName = groupName;
        }

       public UserInfo(
           SerializationInfo info, StreamingContext context)
        {
            // Instead of serializing this object,  
            // serialize a SingletonSerializationHelp instead.
            _ip = info.GetString("ip");
            _physicalAddress = info.GetString("physicalAddress");
            _userName = info.GetString("username");
            _groupName = Util.Util.GetGroupName(_ip);
           

            // No other values need to be added.
        }

        [XmlElement(typeof(string), ElementName = "PhysicalAddress")]
        public string PhysicalAddress
        {
            get { return _physicalAddress; }
            set { _physicalAddress = value; }
        }

        [XmlElement(typeof(string), ElementName = "UserName")]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        [XmlElement(typeof(string), ElementName = "IPAddress")]
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        [XmlElement(typeof(string), ElementName = "HostName")]
        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        [XmlElement(typeof(string), ElementName = "Group", IsNullable = true)]
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        [XmlIgnore]
        public ClientState State
        {
            get { return _state; }
            set { _state = value; }
        }


        public double Alive_Record_Time
        {
            get { return _alive_record_time; }
            set { _alive_record_time = value; }
        }

        public Boolean UsbState
        {
            get {return _usbAllowed;}
            set{_usbAllowed = value;}
        }

        public String InstalledDate
        {
            get { return _installedDate; }
            set { _installedDate = value; }
        }
        public UserInfo Copy()
        {
            UserInfo newUser = new UserInfo();
            newUser.PhysicalAddress = this.PhysicalAddress;
            newUser.UserName = this.UserName;
            newUser.Ip = this.Ip;
            newUser.Host = this.Host;
            newUser.GroupName = this.GroupName;
            newUser.State = this.State;
            return newUser;
        }
        [SecurityPermissionAttribute(SecurityAction.LinkDemand,
  Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(
            SerializationInfo info, StreamingContext context)
        {
            // Instead of serializing this object,  
            // serialize a SingletonSerializationHelp instead.
            info.AddValue("ip", _ip);
            info.AddValue("physicalAddress", _physicalAddress);
            info.AddValue("username", _userName);
            
            // No other values need to be added.
        }


        
    }

   


}

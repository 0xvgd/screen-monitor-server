using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


namespace Agent.Model
{
    class UserListManager
    {
        private static UserListManager _instance = null;
        private List<UserInfo> _userList = null;

        private Dictionary<String, UserInfo> _userListDic = null;

        private Boolean _ischanged = false;

        private Mutex mut = new Mutex(false, "UserListManager");
        public static UserListManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserListManager();
                return _instance;
            }
        }

        private UserListManager()
        {
            _userList = new List<UserInfo>();
            _userListDic = new Dictionary<String, UserInfo>();
        }

        public void Load(String file_name)
        {
            FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate);
            try
            {
                // Construct a BinaryFormatter and use it 
                // to serialize the data to the stream.
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the array elements.
                fs.Position = 0;
                _userListDic = (Dictionary<String, UserInfo>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                _userListDic = new Dictionary<String, UserInfo>() { };
              
            }
            finally
            {
                fs.Close();
            }
        }

        public void Save(String file_name)
        {

            Dictionary<String, UserInfo> savedUserListDic = new Dictionary<String, UserInfo>();
            foreach (String user_key in _userListDic.Keys)
            {
                if (_userListDic[user_key].UserName == "unknown") continue;
                savedUserListDic[user_key] = _userListDic[user_key];
            }
            FileStream fs = new FileStream(file_name, FileMode.Create);

            try
            {
                // Construct a BinaryFormatter and use it 
                // to serialize the data to the stream.
                BinaryFormatter formatter = new BinaryFormatter();

                // Serialize the array elements.
                formatter.Serialize(fs, savedUserListDic);

            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
               
            }
            finally
            {
                fs.Close();
            }
        }

        public UserInfo this[String key]
        {
            get
            {
                if (!_userListDic.ContainsKey(key)) return null;
                return _userListDic[key];
            }
            set
            {
                _userListDic[key] = value;
            }
        }

        public List<String> AllKeys
        {
            get
            {
                return _userListDic.Keys.ToList<String>();
            }
        }
        public UserInfo this[int i]
        {
            get
            {
                return _userList[i];
            }
        }

        public int Count
        {
            get { return _userListDic.Count; }
        }

      

        public Boolean AddUser(UserInfo user)
        {
           
            mut.WaitOne();
            UserInfo list = null;
        

            if (_userListDic.ContainsKey(user.PhysicalAddress))
                list = _userListDic[user.PhysicalAddress];

          

            if (list == null)
            {
                _userListDic[user.PhysicalAddress] = user;
                mut.ReleaseMutex();
                return true;  
            }

           

            if (list.State == UserInfo.ClientState.RUNNING && user.State == UserInfo.ClientState.UNINSTALL)
            {
                mut.ReleaseMutex();
                return true;
            }


            if (user.UserName != "unknown")
            {
                list.UserName = user.UserName;
                list.GroupName = user.GroupName;
            }
            list.Ip = user.Ip;
            list.Alive_Record_Time = user.Alive_Record_Time;
            list.State = user.State;
            list.InstalledDate = user.InstalledDate;
            list.UsbState = user.UsbState;
         //   list.PhysicalAddress = user.PhysicalAddress;

            mut.ReleaseMutex();
           
            return true;
           
         
        }

        // Get computer object from IP
        public UserInfo GetComputerFromIP(string ip)
        {
            UserInfo[] list = (from q in _userListDic.Values
                               where q.Ip == ip
                               select q).ToArray();

            if (list != null && list.Count() > 0)
                return list[0];
            else
                return null;
        }

      

        public Boolean UpdateAliveTime(String physicalAddress)
        {
            UserInfo user = _userListDic[physicalAddress];//GetUserFromMACAddress(physicalAddress);

            //if (user == null || user.UserName == "unknown") return false;
            if (user == null ) return false;
         
            if (user.State != UserInfo.ClientState.RUNNING)
            {
                user.State = UserInfo.ClientState.RUNNING;
                _ischanged = true;
            }
            user.Alive_Record_Time = Util.Util.CurrentTime();

            if (user.UserName == "unknown") return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsChangedUserState()
        {
            if (_ischanged)
            {
                _ischanged = false;
                return true;
            }

            double current = Util.Util.CurrentTime();
            
            bool isChanged = false;
           
        

            foreach (String key in _userListDic.Keys) 
            {
                if (_userListDic[key].Alive_Record_Time == 0) continue;
              
                if (_userListDic[key].State != UserInfo.ClientState.RUNNING) continue;

                if (current - _userListDic[key].Alive_Record_Time > Conf.Constant.VIEW_REFRESH_INTERVAL) //if client is dead
                {
                    _userListDic[key].Alive_Record_Time = 0;
                  
                    _userListDic[key].State = UserInfo.ClientState.DISABLE;
                    isChanged = true;
                }
            }
            return isChanged;
        }


    }
}

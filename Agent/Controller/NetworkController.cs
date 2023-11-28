using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using System.Net;

namespace Agent.Controller
{
    class NetworkController
    {
        static NetworkController _instance = null;
        public bool _isServerConnected = false;

        // User identification & alive
        public delegate void NewDataReceivedDelegate(int indexInUserList);
        public  event NewDataReceivedDelegate NewDataReceived = null;

        // Process monitor info from user
        public delegate void MonitorDataReceivedDelegate(String user_key, Conf.Constant.MONITOR_TYPE monitor_type);
        public event MonitorDataReceivedDelegate MonitorDataReceived = null;

      

        public delegate void UsbStateChangeDelegate(String user_key,  Boolean state);
        public event UsbStateChangeDelegate UsbStateChange = null;

        private Queue<object> _pingQueue = new Queue<object>();
        private Mutex mut = new Mutex(false, "NetworkController");

        private Boolean _pingOpertationStart = false;


        public static NetworkController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NetworkController();
                return _instance;
            }
        }

       
        
        public void Start()
        {

            //1.Run a thread for receive broadcast packet from server or agetns.
            Model.UdpServer.StartReceiveForBroadCasting();
            Model.UdpServer.DataReceived += new Model.UdpServer.DataReceivedDelegate(AliveCommandReceived);

            //2.Start Tcp Listener
            Model.TcpServer.GetTcpServer().StartServer(Conf.Constant.TCP_RECEIVE_PORT);
            Model.TcpServer.GetTcpServer().DataReceived += new Model.TcpServer.DataReceivedDelegate(TcpDataReceived);

            //3. Broadcast to all client
            SearchAllClient();

            //4. check server
            TryConnectServerThread();


            //5. Search all unintalled computers
            SearchUninstallComputer();
           

            
        }


        #region UDP communication
        public void SearchAllClient()
        {
            //Broadcast to all client
            Model.UdpServer.BroadCastingMSG(Conf.NetCommandMessage.CMD_SERVER_IDENTIFY_REQ, Conf.Constant.UDP_SEND_PORT);
            //Model.UdpServer.BroadCastingMSG(Conf.NetCommandMessage.REQUEST_AGENT_IP_REPLY, Conf.Constant.UDP_SEND_PORT);
           
        }
        private void AliveCommandReceived(String ip, byte[] data)
        {
            int message = BitConverter.ToInt32(data, 0);
            

            switch (message)
            {
                case Conf.NetCommandMessage.REQUEST_AGENT_IP:
                    {
                      //  NetworkController.Instance.SendCommand(ip, Conf.NetCommandMessage.REQUEST_AGENT_IP_REPLY);
                        break;
                    }
                case Conf.NetCommandMessage.SEND_MESSAGE_TEXT:
                    {
                        String macAddress = Util.Util.GetMacAddressFromPacket(data);
                        String msgText = Encoding.Unicode.GetString(data, 10, data.Length - 10);
                        MessageBox.Show(msgText, "Message from " + ip, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                case Conf.NetCommandMessage.CMD_SERVER_IDENTIFY_REQ:
                    {

                        Model.UserInfo userinfo = new Model.UserInfo();
                        userinfo.Ip = ip;
                        userinfo.PhysicalAddress = Util.Util.GetMacAddressFromPacket(data);

                        //Installed date
                        DateTime date = DateTime.Now;
                        try
                        {
                            String dateStr = Encoding.Unicode.GetString(data, 10, 20);
                            String[] dateStrs = dateStr.Split('-');
                            date = new DateTime(int.Parse(dateStrs[0]), int.Parse(dateStrs[1]), int.Parse(dateStrs[2]));
                        }
                        catch (Exception e)
                        {
                        }
                        userinfo.InstalledDate = date.ToString("yyyy/M/d");

                        //Usb State

                        userinfo.UsbState = Encoding.Unicode.GetString(data, 30, 2) == "1";
                        userinfo.UserName = Encoding.Unicode.GetString(data, 32, data.Count() - 32);
                        userinfo.GroupName = Util.Util.GetGroupName(ip);
                        userinfo.Alive_Record_Time = Util.Util.CurrentTime();  //Record current time
                        userinfo.State = Model.UserInfo.ClientState.RUNNING;


                        //userinfo.Host

                        Boolean isAddNewUser = Model.UserListManager.Instance.AddUser(userinfo);

                        //Notify viewer

                        mut.WaitOne();
                        _pingQueue.Enqueue(userinfo.PhysicalAddress);
                        mut.ReleaseMutex();

                        //Set client's Screen Interval
                        NetworkController.Instance.SendCommand(
                            userinfo.Ip,
                            Conf.NetCommandMessage.CMD_SET_SCREEN_SHOT_INTERVAL,
                            Conf.Constant.HISTORY_CAPUTRE_IMAGE_INTERVAL.ToString()
                        );

                        //Get Thumb Image;
                        if (Model.UserDataRecords.Instance._imageRecords[userinfo.PhysicalAddress] == null)
                            NetworkController.Instance.SendCommand(userinfo.Ip, Conf.NetCommandMessage.GET_SCREEN_THUMB_IMAGE);

                        break;
                    }
                case Conf.NetCommandMessage.CMD_CLIENT_RUNNING:
                    {
                       
                        String mac_address = Util.Util.GetMacAddressFromPacket(data);
                      
                        Boolean result =  Model.UserListManager.Instance.UpdateAliveTime(mac_address);
                        if (!result )
                        {
                            Model.UdpServer.BroadCastingMSG(ip, Conf.NetCommandMessage.CMD_SERVER_IDENTIFY_REQ, Conf.Constant.UDP_SEND_PORT);
                            
                        }
                      
                       
                        break;
                    }

                case Conf.NetCommandMessage.CMD_USBON_REQ:
                    {
                         String mac_address = Util.Util.GetMacAddressFromPacket(data);
                        Boolean usbState = Boolean.Parse(Encoding.Unicode.GetString(data, 10, data.Length - 10));
                        if (UsbStateChange != null)
                            UsbStateChange(mac_address, usbState);
                        break;
                    }
            }


        }

        #endregion

        #region TCP communication
        public void TryConnectServerThread()
        {
            Thread serverthread = new Thread(new ParameterizedThreadStart(TryConnectServer));
            serverthread.IsBackground = true;
            serverthread.Start(Conf.Constant.SERVER_IP);

        }
        private void TryConnectServer(Object ipObj)
        {
            String ip = ipObj.ToString();
            _isServerConnected = Model.TcpServer.GetTcpServer().IsConnected(ip, Conf.Constant.TCP_SEND_PORT_SERVER);

        }
        public Boolean SendCommand(String ip, int command)
        {
            return  Model.TcpServer.GetTcpServer().SendMessage(ip, command, Conf.Constant.TCP_SEND_PORT);
        }

        public Boolean SendCommand(String ip, int command, String content)
        {
            if (ip == Conf.Constant.SERVER_IP) return Model.TcpServer.GetTcpServer().SendMessage(ip, command, content, Conf.Constant.TCP_SEND_PORT_SERVER);
            return Model.TcpServer.GetTcpServer().SendMessage(ip, command, content, Conf.Constant.TCP_SEND_PORT);
        }

        public Boolean SendCommand(String ip, int command, byte[] content)
        {
           return  Model.TcpServer.GetTcpServer().SendMessage(ip, command, content, Conf.Constant.TCP_SEND_PORT);
        }


        public void RequestScreenThumbImageThread(Object ip)
        {
            NetworkController.Instance.SendCommand(ip.ToString(), Conf.NetCommandMessage.GET_SCREEN_THUMB_IMAGE);
        }

        private void TcpDataReceived(List<byte> dataBuffer, string ip)
        {
            //ParameterizedThreadStart ts1 = new ParameterizedThreadStart(Analize);
            //Thread th1 = new Thread(ts1);
            //th1.IsBackground = true;
            //th1.Start(dataBuffer);
            Analize(dataBuffer, ip);
        }

        public void Analize(object dataBuffer, string ip)
        {
            if (dataBuffer == null || ((List<byte>)(dataBuffer)).Count == 0) return;
            String user_key = null;

            Conf.Constant.MONITOR_TYPE monitor_type = Conf.Constant.MONITOR_TYPE.ALL;

            try
            {
                Model.Record.DataRecordSet record =  Agent.Model.DataAnalizer.Analize(dataBuffer);
                
                mut.WaitOne();
                user_key = record.PhysicalAddress;

               

                if (record.GetType() == typeof(Model.Record.ImageRecordSet))        //Image Data
                {
                    if (((Model.Record.ImageRecordSet)(record)).message_type == Conf.NetCommandMessage.GET_SCREEN_IMAGE)        //if Current Catpure Image
                    {
                        Model.UserDataRecords.Instance._imageRecords[record.PhysicalAddress] = record;
                        monitor_type = Conf.Constant.MONITOR_TYPE.SCREEN_CAPTURE;
                    }
                    else if (((Model.Record.ImageRecordSet)(record)).message_type == Conf.NetCommandMessage.GET_SCREEN_IMAGE_STORE)
                    {
                        System.DateTime now = System.DateTime.Now;
                        string path = String.Format("{0}\\{1:D2}\\{2:D2}\\{3:D2}\\{4}\\",
                                        Conf.Constant.HISTORY_SAVE_PATH,
                                        (now.Year % 100),
                                        now.Month,
                                        now.Day,
                                        (ip == null ? record.PhysicalAddress : ip));
                        string fileName = String.Format("{0:D2}-{1:D2}-{2:D2}.jpg", now.Hour, now.Minute, now.Second);
                        System.IO.Directory.CreateDirectory(path);
                        ((Model.Record.ImageRecordSet)record).RecordImage.Save(path + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        monitor_type = Conf.Constant.MONITOR_TYPE.SCREEN_CAPTURE;
                    }
                    else if (((Model.Record.ImageRecordSet)(record)).message_type == Conf.NetCommandMessage.GET_SCREEN_IMAGE_HISTORY) //if History Image Data
                    {
                        Model.UserDataRecords.Instance.AddImageToQueue((Model.Record.ImageRecordSet)(record));
                        monitor_type = Conf.Constant.MONITOR_TYPE.SCREEN_HISTORY;
                    }
                }
                else if (record.GetType() == typeof(Model.Record.ProcessRecordSet)) //Process List
                {
                    Model.UserDataRecords.Instance._processRecords[record.PhysicalAddress] = record;
                    monitor_type = Conf.Constant.MONITOR_TYPE.PROCESS_LIST;
                }
                else if (record.GetType() == typeof(Model.Record.UsbRecordSet)) //Usb Log
                {
                    if (Model.UserDataRecords.Instance._usbRecords[record.PhysicalAddress] == null) Model.UserDataRecords.Instance._usbRecords[record.PhysicalAddress] = record;
                    else
                        ((Model.Record.UsbRecordSet)(Model.UserDataRecords.Instance._usbRecords[record.PhysicalAddress])).AddRecordSet((Model.Record.UsbRecordSet)record);
                    monitor_type = Conf.Constant.MONITOR_TYPE.USB_LOG;
                }
                else if (record.GetType() == typeof(Model.Record.ApplicationRecordSet)) //Insatalled Application List
                {
                    Model.UserDataRecords.Instance._installAppRecords[record.PhysicalAddress] = record;
                    monitor_type = Conf.Constant.MONITOR_TYPE.INSTALLED_APPLICATION_LIST;
                }
                else if (record.GetType() == typeof(Model.Record.DeviceRecordSet)) //Device Info List
                {
                    Model.UserDataRecords.Instance._deviceRecords[record.PhysicalAddress] = record;
                    monitor_type = Conf.Constant.MONITOR_TYPE.DEVICE_INFO;
                }
                mut.ReleaseMutex();
            }
            catch(Exception exp)
            {
                Console.WriteLine("EventListView.Analize-->Exception Source:{0}; Message:{1}", exp.Source, exp.Message);
                mut.ReleaseMutex();
            }


            //Notify Manager
            if (MonitorDataReceived != null && user_key != null)
                MonitorDataReceived(user_key, monitor_type);

        }
        #endregion

        #region 
        public void SearchUninstallComputer()
        {
            //Thread thread = new Thread(new ThreadStart(startSearchUninstallComputer));
            //thread.IsBackground = true;
            //thread.Start();

            Console.WriteLine("pingstart");
            startSearchUninstallComputer();
        }

        public void startSearchUninstallComputer()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in nics)
            {

                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    // adapter.Description.Substring(0, realtek.Length) == realtek)
                {
                    foreach (UnicastIPAddressInformation ipInfo in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (ipInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            byte[] bytes = ipInfo.Address.GetAddressBytes();
                            for (int i = 1; i < 255; i++)
                            {
                                if (i != bytes[3])
                                {
                                    PingSender pindSender = new PingSender();
                                    pindSender.PingDataReceived += PingDataReceived;
                                    //Thread thread = new Thread(new ParameterizedThreadStart(pindSender.SendPing));
                                    //thread.IsBackground = true;
                                    //thread.Start(string.Format("{0}.{1}.{2}.{3}", bytes[0], bytes[1], bytes[2], i));
                                    pindSender.SendPing(string.Format("{0}.{1}.{2}.{3}", bytes[0], bytes[1], bytes[2], i));
                                }
                            }
                            GC.Collect();
                        }
                    }
                }
            }
        }
        
        public void PingDataReceived(String user_key)
        {
            mut.WaitOne();
            _pingQueue.Enqueue(user_key);
            mut.ReleaseMutex();
        }
        public object GetDataFromPingBuffer()
        {
            object obj = null;
            mut.WaitOne();
            if (_pingQueue.Count > 0)
            {
                obj = _pingQueue.Dequeue();
            }
            mut.ReleaseMutex();

            return obj;
        }
        #endregion


    }
}

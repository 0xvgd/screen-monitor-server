using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace Agent.Conf
{
    public class Constant
    {
        public const int TCP_RECEIVE_PORT = 35356;
        public const int TCP_SEND_PORT = 35357;
        public const int TCP_SEND_PORT_SERVER = 35356;

       

        public const int UDP_RECEIVE_PORT = 12001;
        public const int UDP_SEND_PORT = 11001;
        public static string BROADCAST_IP = @"255.255.255.255";


        public static string SERVER_IP = @"192.168.1.102";
        public static string LOCAL_IP = "";

        public const int ALIVE_TIME_INTERVAL = 3000;
        public const int VIEW_REFRESH_INTERVAL = ALIVE_TIME_INTERVAL * 2;

        //Captured Image Size
        public  static Size Thumb_IMAGE_SIZE = new Size(256, 256);
        public static Size FULL_IMAGE_SIZE = new Size(500, 500);
        public static Size ImageGirdSize = new Size(5, 3);

        public const int CONNECTION_HISTORY_INTERVAL = 60; // 1 min
      
        public static int HISTORY_CAPUTRE_IMAGE_INTERVAL = 30; // 30s
        public static string HISTORY_SAVE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string CONNECTION_HISTORY_PATH = HISTORY_SAVE_PATH + "\\connection";
        public static string CONF_FILE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\.monitor\\config";

        public enum MONITOR_TYPE {USB_LOG, INSTALLED_APPLICATION_LIST, PROCESS_LIST, DEVICE_INFO, SCREEN_CAPTURE, SCREEN_HISTORY, CONNECTION_HISTORY, ALL };

        //USB related
        public static Color USB_ON_COLOR = Color.FromArgb(1, 255, 0, 0);
        public static Color USB_OFF_COLOR = Color.FromArgb(1, 0, 255, 0);

        //User Info
        public static string USER_INFO_DIC_NAME = @"C:\Windows\System32\userinfo.dic";

        //Unmanaged Ips
        public static string[] UNMANAGED_IPS = { "192.168.101.101", "192.168.101.102", "192.168.101.103", "192.168.101.104", "192.168.100.1", "192.168.101.1", "192.168.101.2", "192.168.100.100" };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent.Conf
{
    public class NetCommandMessage
    {
        public const int CMD_SERVER_IDENTIFY_REQ = 1;

        public const int CMD_SHUTDOWN_REQ = 2;

        public const int CMD_RESTART_REQ = 4;

        public const int CMD_LOGOFF_REQ = 8;

        public const int CMD_SENDMESSAGE_REQ = 16;

        public const int CMD_USBON_REQ = 32;

        public const int CMD_USBOFF_REQ = 64;

        public const int CMD_EXEC_APPLICATION = 128;

        public const int GET_INSTALLED_APPLICATION = 256;

        public const int GET_PROCESS_LIST = 512;

        public const int GET_DEVICE_INFO = 1024;

        public const int GET_SCREEN_IMAGE = 2000;

        public const int GET_SCREEN_IMAGE_STORE = 2024;

        public const int GET_SCREEN_FULL_IMAGE = 2048;

        public const int GET_SCREEN_THUMB_IMAGE = 3072;

        public const int CMD_EXEC_COMMON_COMMAND = 4096;

        public const int CMD_SET_SCREEN_SHOT_INTERVAL = 4128;

        public const int REQUEST_AGENT_IP = 4200;

        public const int REQUEST_AGENT_IP_REPLY = 4300;

        public const int LOG_APPLICATION = 8192;

        public const int LOG_USB = 16384;

        public const int DETECT_USB = 16400;

        public const int CMD_KILL_PROCESS = 2345;

        public const int CMD_CLIENT_RUNNING = 5220;

        public const int GET_INSTALLED_DATE = 6500;

        public const int SEND_MESSAGE_TEXT = 7000;

        //server

        public const int GET_USB_LOG = 16500;

        public const int GET_SCREEN_IMAGE_HISTORY = 16600;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Agent.Controller
{
    public class Config
    {
        public class Data
        {
            public int historyInterval = Conf.Constant.HISTORY_CAPUTRE_IMAGE_INTERVAL;
            public string historyPath = Conf.Constant.HISTORY_SAVE_PATH;
        }

        private const string historySavePathKey = "HISTORY_PATH=";
        private const string historyIntervalKey = "HISTORY_INTERVAL=";

        public static Data ReadConfig()
        {
            Data data = new Data();
            if (File.Exists(Conf.Constant.CONF_FILE_PATH))
            {
                using (FileStream fs = File.OpenRead(Conf.Constant.CONF_FILE_PATH))
                {
                    byte[] b = new byte[1024];
                    int bytesRead = fs.Read(b, 0, b.Length);

                    if (bytesRead > 0)
                    {
                        string[] lines = Encoding.UTF8.GetString(b, 0, bytesRead).Split(new string[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                        
                        foreach (string line in lines)
                        {
                            if (line.Substring(0, historySavePathKey.Length) == historySavePathKey)
                            {
                                data.historyPath = line.Substring(historySavePathKey.Length);
                            }
                            else if (line.Substring(0, historyIntervalKey.Length) == historyIntervalKey)
                            {
                                try
                                {
                                    data.historyInterval = int.Parse(line.Substring(historyIntervalKey.Length));
                                }
                                catch (Exception e) { }
                            }
                        }
                    }
                }
            }
            return data;
        }

        public static void LoadConfig(Data data)
        {
            Conf.Constant.HISTORY_CAPUTRE_IMAGE_INTERVAL = data.historyInterval;
            Conf.Constant.HISTORY_SAVE_PATH = data.historyPath;
        }

        public static Data GetConfig()
        {
            Data data = new Data();
            data.historyPath = Conf.Constant.HISTORY_SAVE_PATH;
            data.historyInterval = Conf.Constant.HISTORY_CAPUTRE_IMAGE_INTERVAL;
            return data;
        }

        public static void SaveConfig(Data data)
        {
            if (File.Exists(Conf.Constant.CONF_FILE_PATH))
            {
                File.Delete(Conf.Constant.CONF_FILE_PATH);
            }

            int length = Conf.Constant.CONF_FILE_PATH.LastIndexOf('\\');
            Directory.CreateDirectory(Conf.Constant.CONF_FILE_PATH.Substring(0, length));
            using (FileStream fs = File.OpenWrite(Conf.Constant.CONF_FILE_PATH))
            {
                string content = historySavePathKey + Conf.Constant.HISTORY_SAVE_PATH + "\r\n" +
                                   historyIntervalKey + Conf.Constant.HISTORY_CAPUTRE_IMAGE_INTERVAL;
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                fs.Write(buffer, 0, buffer.Length);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Agent.Model
{
    public class DataAnalizer
    {

        public delegate void DetectUsbDelegate(String user_key);
        public static DetectUsbDelegate _Detectusb = null;

        private DataAnalizer()
        {

        }

        public static Record.DataRecordSet Analize(object obj)
        {
            if (obj == null)
                return null;

            Record.DataRecordSet retVal = null;

            try
            {
                List<byte> data = (List<byte>)obj;

                byte[] buffer = data.ToArray();

                Int32 type = BitConverter.ToInt32(buffer, 0);

                string pAddr = string.Empty;

                for (int i = 4; i < 10; i++)
                {
                    pAddr += buffer[i].ToString("X2");
                    if (i != 9)
                    {
                        pAddr += "-";
                    }
                }

                List<byte> bufferList = new List<byte>(buffer);
                byte[] databuffer = bufferList.GetRange(10, bufferList.Count - 10).ToArray();

               
                switch (type)
                {
                    case Conf.NetCommandMessage.GET_PROCESS_LIST:
                        {
                            retVal = AnalizeProcessList(pAddr, databuffer);
                            break;
                        }
                    case Conf.NetCommandMessage.LOG_USB :
                        {
                            retVal = AnalizeUsbLog(pAddr, databuffer);
                            break;
                        }
                    case Conf.NetCommandMessage.GET_USB_LOG:
                        {
                            retVal = AnalizeUsbLog(pAddr, databuffer);
                            break;
                        }
                    case Conf.NetCommandMessage.GET_DEVICE_INFO:
                        {
                            retVal = AnalizeDeviceInfoList(pAddr, databuffer);
                            break;
                        }
                    case Conf.NetCommandMessage.GET_INSTALLED_APPLICATION:
                        {
                            retVal = AnalizeInstalledApplicationList(pAddr, databuffer);
                            break;
                        }
                    case Conf.NetCommandMessage.GET_SCREEN_IMAGE:
                        {
                            retVal = AnalizeScreenCaptureImage(pAddr, databuffer);
                            ((Record.ImageRecordSet)(retVal)).message_type = Conf.NetCommandMessage.GET_SCREEN_IMAGE;
                            break;
                        }
                    case Conf.NetCommandMessage.GET_SCREEN_IMAGE_STORE:
                        {
                            retVal = AnalizeScreenHistoryImage(pAddr, databuffer);
                            ((Record.ImageRecordSet)(retVal)).message_type = Conf.NetCommandMessage.GET_SCREEN_IMAGE_STORE;
                            break;
                        }
                    case Conf.NetCommandMessage.GET_SCREEN_IMAGE_HISTORY:
                        {
                            retVal = AnalizeScreenHistoryImage(pAddr, databuffer);
                            ((Record.ImageRecordSet)(retVal)).message_type = Conf.NetCommandMessage.GET_SCREEN_IMAGE_HISTORY;
                            break;
                        }
                    case Conf.NetCommandMessage.DETECT_USB:
                        {
                            retVal = AnalizeUsbLog(pAddr, databuffer);
                            if (((Record.UsbRecordSet)retVal).UsbRecords[0].EjectTime == "" && _Detectusb != null)
                                _Detectusb(retVal.PhysicalAddress);
                            break;
                        }
                    case Conf.NetCommandMessage.GET_INSTALLED_DATE:
                        {
                            UserInfo user = UserListManager.Instance[pAddr];
                            user.InstalledDate =  Encoding.Unicode.GetString(buffer).Trim();
                            break;
                        }
                    
                }

                data.Clear();
                data = null;

                bufferList.Clear();
                bufferList = null;

            }catch(Exception exp)
            {
                Console.WriteLine("Source:{0}; Message:{1}", exp.Source, exp.Message);
                retVal = null;
            }            

            return retVal;
        }

        public static Record.DataRecordSet AnalizeProcessList(String pAddr, Byte[] buffer)
        {
            String str = Encoding.Unicode.GetString(buffer).Trim(); ;
            Record.DataRecordSet retVal = null;
            Model.Record.ProcessRecordSet recordSet = new Model.Record.ProcessRecordSet();
            recordSet.PhysicalAddress = pAddr;
            recordSet.Time = DateTime.Now;
            int count = buffer.Count() - 10;
          

            // ML  
            recordSet.ProcessRecords = new List<Model.Record.ProcessRecord>();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(str);
                XmlElement root = doc.DocumentElement;
                XmlNodeList recordElementList = root.GetElementsByTagName("record");

                foreach (XmlNode node in recordElementList)
                {
                    XmlNodeList nodes = node.ChildNodes;

                    Record.ProcessRecord r = new Record.ProcessRecord();
                    foreach (XmlNode child in nodes)
                    {
                        switch (child.Name)
                        {
                            case "name":
                                {
                                    r.ProcessName = child.InnerText.Trim();
                                    break;
                                }
                            case "id":
                                {
                                    if (child.InnerText.ToLower() != "unknown")
                                        r.PID = (ulong)Convert.ToUInt64(child.InnerText.Trim(), 10);
                                    else
                                        r.PID = 0;
                                    break;
                                }
                            case "priority":
                                {
                                    r.Priority = child.InnerText.Trim();
                                    break;
                                }
                            case "path":
                                {
                                    r.ProcessFullPath = child.InnerText.Trim();
                                    break;
                                }
                        }
                    }

                    recordSet.ProcessRecords.Add(r);
                }

                retVal = recordSet;
            }
            catch (System.Xml.XmlException exp)
            {
                Console.WriteLine("Source:{0}; Message:{1}", exp.Source, exp.Message);
                retVal = null;
            }
            return retVal;
        }

        public static Record.DataRecordSet AnalizeInstalledApplicationList(String pAddr, Byte[] buffer)
        {
            String str = Encoding.Unicode.GetString(buffer).Trim();
            Record.DataRecordSet retVal = null;
            Model.Record.ApplicationRecordSet recordSet = new Model.Record.ApplicationRecordSet();
            recordSet.PhysicalAddress = pAddr;
            recordSet.Time = DateTime.Now;
            int count = buffer.Count() - 10;
            //MessageBox.Show(str);

            //XML
           recordSet.ApplicationRecords = new List<Model.Record.ApplicationRecord>();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(str);
                XmlElement root = doc.DocumentElement;
                XmlNodeList recordElementList = root.GetElementsByTagName("record");

                foreach (XmlNode node in recordElementList)
                {
                    XmlNodeList nodes = node.ChildNodes;

                    Model.Record.ApplicationRecord r = new Model.Record.ApplicationRecord();
                    foreach (XmlNode child in nodes)
                    {
                        switch (child.Name)
                        {
                            case "name":
                                {
                                    r.ApplicationName = child.InnerText.Trim();
                                    break;
                                }
                            case "capacity":
                                {
                                    if (child.InnerText.ToLower() != "unknown")
                                        r.Capacity = (ulong)Convert.ToUInt64(child.InnerText.Trim(), 10);
                                    else
                                        r.Capacity = 0;
                                    break;
                                }
                            case "date":
                                {
                                    if (child.InnerText.ToLower() != "unknown")
                                    {
                                        try
                                        {
                                            r.InstalledTime = Convert.ToDateTime(child.InnerText.Trim());
                                        }
                                        catch (System.Exception)
                                        {
                                            r.InstalledTime = new DateTime(1, 1, 1, 1, 1, 1);
                                        }
                                    }
                                    else
                                        r.InstalledTime = new DateTime(1, 1, 1, 1, 1, 1);
                                    break;
                                }
                        }
                    }

                    recordSet.ApplicationRecords.Add(r);
                }

                retVal = recordSet;
            }
            catch (System.Xml.XmlException exp)
            {
                Console.WriteLine("Source:{0}; Message:{1}", exp.Source, exp.Message);
                retVal = null;
            }
            return retVal;
        }

        public static Record.DataRecordSet AnalizeDeviceInfoList(String pAddr, Byte[] buffer)
        {
            String str = Encoding.Unicode.GetString(buffer).Trim();
            Record.DataRecordSet retVal = null;
            Model.Record.DeviceRecordSet recordSet = new Model.Record.DeviceRecordSet();
            recordSet.PhysicalAddress = pAddr;
            recordSet.Time = DateTime.Now;
            int count = buffer.Count() - 10;
          
            //MessageBox.Show("Device:" + str);
            //XML
            recordSet.DeviceRecords = new List<Model.Record.DeviceRecord>();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(str);
                XmlElement root = doc.DocumentElement;
                XmlNodeList recordElementList = root.GetElementsByTagName("record");

                foreach (XmlNode node in recordElementList)
                {
                    XmlNodeList nodes = node.ChildNodes;

                    Model.Record.DeviceRecord r = new Model.Record.DeviceRecord();
                    foreach (XmlNode child in nodes)
                    {
                        switch (child.Name)
                        {
                            case "name":
                                {
                                    r.DeviceName = child.InnerText.Trim();
                                    break;
                                }
                            case "value":
                                {
                                    r.Value = child.InnerText.Trim();
                                    break;
                                }
                        }
                    }

                    recordSet.DeviceRecords.Add(r);
                }

                retVal = recordSet;
            }
            catch (System.Xml.XmlException exp)
            {
                Console.WriteLine("Source:{0}; Message:{1}", exp.Source, exp.Message);
                retVal = null;
            }
            return retVal;
        }

        public static Record.DataRecordSet AnalizeUsbLog(String pAddr, Byte[] buffer)
        {
            String str = Encoding.Unicode.GetString(buffer).Trim();
            Record.DataRecordSet retVal = null;
            Model.Record.UsbRecordSet recordSet = new Model.Record.UsbRecordSet();
            recordSet.PhysicalAddress = pAddr;
            recordSet.Time = DateTime.Now;
            
          
            //MessageBox.Show("PROCESSLIST:::" + str);

            //XML
            recordSet.UsbRecords = new List<Model.Record.UsbRecord>();


            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(str);
                XmlElement root = doc.DocumentElement;
                XmlNodeList recordElementList = root.GetElementsByTagName("record");

                foreach (XmlNode node in recordElementList)
                {
                    XmlNodeList nodes = node.ChildNodes;

                    Record.UsbRecord r = new Record.UsbRecord();
                    foreach (XmlNode child in nodes)
                    {
                        switch (child.Name)
                        {
                            case "id":
                                {
                                    r.Id = child.InnerText;
                                    break;
                                }
                            case "devname":
                                {
                                    r.DevName = child.InnerText.Trim();
                                    break;
                                }
                            case "plugtime":
                                {
                                    r.PlugTime = child.InnerText.Trim();
                                    break;
                                }
                            case "ejecttime":
                                {
                                    r.EjectTime = child.InnerText.Trim();
                                    break;
                                }
                            case "state":
                                {
                                    r.State = Boolean.Parse(child.InnerText.Trim());
                                    break;
                                }
                            case "content":
                                {
                                    r.Content = child.InnerText.Trim();
                                    break;
                                }
                        }
                    }

                    recordSet.UsbRecords.Add(r);
                }
                retVal = recordSet;
            }
            catch (System.Xml.XmlException exp)
            {
                Console.WriteLine("Source:{0}; Message:{1}", exp.Source, exp.Message);
                retVal = null;
            }
            return retVal;
        }
    
        public static Record.DataRecordSet AnalizeScreenCaptureImage(String pAddr, Byte[] buffer)
        {
            Record.DataRecordSet retVal = null;

            Record.ImageRecordSet recordSet = null;


            try
            {
                List<byte> bytes = new List<byte>(buffer);

                recordSet = new Record.ImageRecordSet(buffer);
                recordSet.PhysicalAddress = pAddr;
                recordSet.Time = DateTime.Now;
            }
            catch (Exception)
            {
                Console.WriteLine(string.Format(Resource1.imageBufferError, buffer.Length.ToString()));
                recordSet = null;
            }

            retVal = recordSet;
           
            return retVal;
        }

        public static Record.DataRecordSet AnalizeScreenHistoryImage(String pAddr, Byte[] buffer)
        {
            Record.DataRecordSet retVal = null;

            Record.ImageRecordSet recordSet = null;

            try
            {

                List<byte> bytes = new List<byte>(buffer);
                recordSet = new Record.ImageRecordSet(bytes.GetRange(34, bytes.Count - 34).ToArray());
                recordSet.PhysicalAddress = pAddr;
                string str = Encoding.Unicode.GetString(buffer, 0, 34);
                recordSet.Time = DateTime.Parse(str); //"15/08/06 12:30:55"
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.ToString());
                recordSet = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format(Resource1.imageBufferError, buffer.Length.ToString()));
                recordSet = null;
            }

            retVal = recordSet;

            return retVal;
        }

    }
}

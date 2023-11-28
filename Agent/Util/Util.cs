using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.NetworkInformation;


namespace Agent.Util
{
    class Util
    {

        #region Mac Address Related
        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"><cmd:int><MAC address : 6byte string><content : string></param>
        /// <returns></returns>
        public static String GetMacAddressFromPacket(byte[] packet)
        {

            if (packet.Length < 10) return "";
            string pAddr = string.Empty;

            for (int i = 4; i < 10; i++)
            {
                pAddr += packet[i].ToString("X2");
                if (i != 9)
                {
                    pAddr += "-";
                }
            }
            return pAddr;
        }

        /// <summary>
        /// Get Mac Address from ip using ARP protocal
        /// </summary>
        /// <param name="ip"></param>
       

         struct _IPAddress {
                byte b1, b2, b3, b4;
                public _IPAddress(byte a1, byte a2, byte a3, byte a4)
                {
                    b1 = a1;
                    b2 = a2;
                    b3 = a3;
                    b4 = a4;
                }

                public static _IPAddress Parse(String ipstr)
                {
                    ipstr.Trim();
                    String[] strs = ipstr.Split('.');

                    if (strs.Length < 4) 
                    {
                        ipstr = "127.0.0.1";
                        strs = ipstr.Split('.');
                     }
                    _IPAddress address = new _IPAddress(byte.Parse(strs[0]), byte.Parse(strs[1]), byte.Parse(strs[2]), byte.Parse(strs[3]));
                    return address;

                }


             } 

        [DllImport("Iphlpapi.dll", SetLastError = true)]
        private static extern int SendARP(_IPAddress destip, _IPAddress srcip, long[] macaddress, ref long macaddresslen);

        public static  String GetMacAddressFromIP(String ip)
        {
           
            _IPAddress destip = _IPAddress.Parse(ip);
            _IPAddress srcip = _IPAddress.Parse(Conf.Constant.LOCAL_IP);
            
            long[] MacAddr = new long[2];
            
            long PhysAddrLen = 6;
            int dwRetVal = SendARP(destip, srcip,  MacAddr, ref PhysAddrLen);

            byte[] address = BitConverter.GetBytes(MacAddr[0]);
            String pAddr = "";

            for (int i = 0; i < 6; i++)
            {
                pAddr += address[i].ToString("X2");
                if (i != 5)
                {
                    pAddr += "-";
                }
            }
            return pAddr;
        }


        public static IPAddress GetIPAddress()
        {

            List<IPAddress> list = new List<IPAddress>();
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            if (nics == null || nics.Length < 1)
            {
                //  Console.WriteLine("  No network interfaces found.");
                return IPAddress.Parse("127.0.0.1");
            }

            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.NetworkInterfaceType != NetworkInterfaceType.Ethernet) continue;
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                UnicastIPAddressInformationCollection anyCast = adapterProperties.UnicastAddresses;

                if (anyCast != null)
                {
                    foreach (UnicastIPAddressInformation any in anyCast)
                    {
                        if (!any.Address.IsIPv6LinkLocal)
                            list.Add(any.Address);
                        
                    }
                    Console.WriteLine();
                }

            }

            if (list.Count > 0)
                return list[0];
            else
                return null;

            
        }
        #endregion


        public static string GetGroupName(String ip)
        {
            int start_index = -1;
            for (int i = 0; i < 2; i++)
            {
                start_index = ip.IndexOf(".", start_index + 1);
            }
            int second_index = ip.IndexOf(".", start_index + 1);

            String s = ip.Substring(start_index + 1, second_index - start_index - 1);
            if (s.CompareTo("101") == 0)
                s = "Program";
            else if (s.CompareTo("102") == 0)
                s = "3D";
            else if (s.CompareTo("103") == 0)
                s = "2D";
            else if (s.CompareTo("100") == 0)
                s = "Commander";
            else
                s = "Other";
            return s;
        }

        public static void ReleaseMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public static double CurrentTime()
        {
            DateTime now = DateTime.Now;
            DateTime centuryBegin = new DateTime(2001, 1, 1);
            long elapsedTicks = now.Ticks - centuryBegin.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            double current = elapsedSpan.TotalMilliseconds;
            return current;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Agent.Model
{
    public class NetData
    {
        public string ip = null;
        public int message = -1;
        public int port = -1;
        public string msgContent = null;
    }

    /// <summary>
    /// Created on 2012/6/5 by M.HM
    /// </summary>
    public class UdpServer
    {

        public delegate void DataReceivedDelegate(String ip, byte[] data);

        public static DataReceivedDelegate DataReceived = null;

        private static bool isReceiveStarted = false;

        

       

        private static Thread receiveThread = null;

        private static UdpClient listener = null;

     
        private static Mutex mut = new Mutex(false, "NetworkModule");
        

      

        #region send broadcasting

        public static void BroadCastingMSG(int message, int port)
        {
            NetData data = new NetData();
            data.ip = Conf.Constant.BROADCAST_IP;
            data.message = message;
            data.port = port;

            Thread th = new Thread(new ParameterizedThreadStart(BroadCastingThread));
            th.IsBackground = true;
            th.Start(data);
        }

        public static void BroadCastingMSG(string ip, int message, int port)
        {
            NetData data = new NetData();
            data.ip = ip;
            data.message = message;
            data.port = port;

            Thread th = new Thread(new ParameterizedThreadStart(BroadCastingThread));
            th.IsBackground = true;
            th.Start(data);
        }

        //
        public static void BroadCastingMSG(int message, int port, string msgContent)
        {
            NetData data = new NetData();
            data.ip = Conf.Constant.BROADCAST_IP;
            data.message = message;
            data.port = port;
            data.msgContent = msgContent;

            Thread th = new Thread(new ParameterizedThreadStart(BroadCastingThread));
            th.IsBackground = true;
            th.Start(data);
        }

        //
        public static void BroadCastingMSG(string ip, int message, int port, string msgContent)
        {
            NetData data = new NetData();
            data.ip = ip;
            data.message = message;
            data.port = port;
            data.msgContent = msgContent;

            Thread th = new Thread(new ParameterizedThreadStart(BroadCastingThread));
            th.IsBackground = true;
            th.Start(data);
        }

        private static void BroadCastingThread(object objData)
        {
            NetData data = (NetData)objData;

            mut.WaitOne();

           // Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                //IPAddress broadcast = IPAddress.Parse(data.ip);

                List<byte> buf = new List<byte>();
                byte[] tempbuf = BitConverter.GetBytes(data.message);
                buf.AddRange(tempbuf);

                if (!string.IsNullOrEmpty(data.msgContent))
                    buf.AddRange(Encoding.Unicode.GetBytes(data.msgContent));


                byte[] sendbuf = buf.ToArray();

                UdpClient client = new UdpClient();

                client.Send(sendbuf, sendbuf.Length, data.ip, data.port);
                client.Close();

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //s.Shutdown(SocketShutdown.Both);
                //s.Close();
                
            }

            mut.ReleaseMutex();
        }

        #endregion

        #region receive response of broadcasting // Get response from broadcasting

        public static void StartReceiveForBroadCasting()
        {
            if (isReceiveStarted)
                return;

            //done = false;
            if (receiveThread != null && receiveThread.IsAlive)
                receiveThread.Abort();

            ThreadStart ts = new ThreadStart(receiveForBroadCasting);

            receiveThread = new Thread(ts);

            receiveThread.IsBackground = true;

            receiveThread.Start();
        }
        
        // Receive broadcasting and its result is remote ip & port
        
        private static void receiveForBroadCasting()
        {
            try
            {
                listener = new UdpClient(Conf.Constant.UDP_RECEIVE_PORT);
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, Conf.Constant.UDP_RECEIVE_PORT);

                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);

                    String ip = groupEP.Address.ToString();

                    //Notify to NetworkController
                   
                    try
                    {
                        if (DataReceived != null)
                            DataReceived(ip, bytes);
                    }
                    catch (Exception e)
                    {
                    }
                  //  Console.WriteLine(ip);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (listener != null)
                    listener.Close();
            }
        }

        #endregion
      

        public static void EndReceive()
        {
            //done = true;
            if (listener != null)
            {
                listener.Close();
                listener = null;
            }
        }

       

    }
}

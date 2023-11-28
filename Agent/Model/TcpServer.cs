using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace Agent.Model
{
    public class TcpServer
    {
        private TcpListener server = null;

        private static TcpServer _tcpServer = null;

        private Mutex mut = new Mutex(false, "TcpServer");

        public delegate void DataReceivedDelegate(List<byte> data, string ip);

        public DataReceivedDelegate DataReceived = null;

        private TcpServer()
        {
            server = null;
        }

        public static TcpServer GetTcpServer()
        {
            if (_tcpServer == null)
            {
                _tcpServer = new TcpServer();
                return _tcpServer;
            }
            return _tcpServer;
        }

        public bool IsConnected(String server, int port)
        {
            try
            {
                TcpClient client = new TcpClient(server, port);

                client.Close();
                return true;
            }
            catch (IOException e)
            {
                Console.Write(e.ToString());
                return false;
            }
            catch (SocketException e)
            {
                Console.Write(e.ToString());
                return false;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return false;

            }
        }

        #region TCP Server related
        public void StartServer(int port)
        {
            ParameterizedThreadStart start = new ParameterizedThreadStart(internalStartServer);
            Thread th = new Thread(start);
          //  th.IsBackground = true;
            th.Start(port);
        }

      

        private void internalStartServer(object port)
        {
            try
            {
                server = new TcpListener(IPAddress.Any, (int)port);

                server.Start();

                while (true)
                {
                    //Console.WriteLine("Waiting for connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    client.ReceiveBufferSize = 100000;
                    internalReceiveDataFromRemoteHost(client);
                    //ParameterizedThreadStart ts = new ParameterizedThreadStart(internalReceiveDataFromRemoteHost);
                    //Thread th = new Thread(ts);
                    //th.IsBackground = true;
                    //th.Start(client);
                }

            }catch(System.Net.Sockets.SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }catch(System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }finally
            {
                server.Stop();
                StartServer((int)port);
            }
        }
        
        private void internalReceiveDataFromRemoteHost(object clientHost)
        {
            TcpClient client = (TcpClient)clientHost;
            List<byte> dataBuffer = new List<byte>();
            string ip = null;

            try
            {
                NetworkStream stream = client.GetStream();

                byte[] bytes = new byte[1024];

                
                int i = 0;

                // Loop to receive all the data sent by the client.
                do
                {
                    i = stream.Read(bytes, 0, bytes.Length);

                    IEnumerable<byte> tempBuffer = bytes.ToList().Take(i);
                    dataBuffer.AddRange(tempBuffer);
                } while (i > 0);

                //Controller.NetworkController.Instance.PushQueue(dataBuffer);

                ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                stream.Close();

            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();

                //notify NetworkController
                if (DataReceived != null && dataBuffer.Count != 0)
                {
                    DataReceived(dataBuffer, ip);
                }
            }
        }
        #endregion

        #region TCP client

        public Boolean SendMessage(String server, int message, int port)
        {
            Boolean result;
            mut.WaitOne();
            try
            {
                TcpClient client = new TcpClient(server, port);
                // Translate the passed message into ASCII and store it as a Byte array.
                byte[] data = BitConverter.GetBytes(message);


                // Get a client stream for reading and writing. 
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();
                
                // Send the message to the connected TCPServer. 
                stream.Write(data, 0, data.Length);

                //Close everything
                stream.Close();
                client.Close();
                result = true;

            }
            catch (IOException e)
            {
                Console.Write(e.ToString());
                result = false;
            }
            catch (SocketException e)
            {
                Console.Write(e.ToString());
                result = false;
            }

            mut.ReleaseMutex();
            return result;

        }
        public Boolean SendMessage(String server, int message, byte[] content, int port)
        {
            Boolean result;
            mut.WaitOne();
            try
            {
                TcpClient client = new TcpClient(server, port);
                // Translate the passed message into ASCII and store it as a Byte array.
                List<byte> buffer = new List<byte>();
                buffer.AddRange(BitConverter.GetBytes(message));
                buffer.AddRange(content);



                // Get a client stream for reading and writing. 
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();
                client.SendBufferSize = 10000;

                // Send the message to the connected TCPServer. 
                stream.Write(buffer.ToArray(), 0, buffer.Count);

                //Close everything
                stream.Close();
                client.Close();
                result = true;

            }
            catch (IOException e)
            {
                Console.Write(e.ToString());
                result = false;
            }
            catch (SocketException e)
            {
                Console.Write(e.ToString());
                result = false;
            }

            mut.ReleaseMutex();
            return result;

        }
        public Boolean SendMessage(String server, int message, String content, int port)
        {
            Boolean result;
            mut.WaitOne();
            try
            {
                TcpClient client = new TcpClient(server, port);
                // Translate the passed message into ASCII and store it as a Byte array.
                List<byte> buffer = new List<byte>();
                buffer.AddRange(BitConverter.GetBytes(message));
                buffer.AddRange(System.Text.Encoding.Unicode.GetBytes(content));
                


                // Get a client stream for reading and writing. 
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();
                client.SendBufferSize = 10000;
                // Send the message to the connected TCPServer. 
                stream.Write(buffer.ToArray(), 0, buffer.Count);

                //Close everything
                stream.Close();
                client.Close();
                result = true;

            }
            catch (IOException e)
            {
                Console.Write(e.ToString());
                result = false;
            }
            catch (SocketException e)
            {
                Console.Write(e.ToString());
                result = false;
            }

            mut.ReleaseMutex();
            return result;

        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading;

namespace Agent.Controller
{
    class PingSender : IDisposable
    {
        public delegate void PingDataReceivedDelegate(String user_key);
        public  PingDataReceivedDelegate PingDataReceived = null;

        private bool disposed = false;



        public void SendPing(object ip)
        {
            AutoResetEvent waiter = new AutoResetEvent(false);

            Ping pingSender = new Ping();

            // When the PingCompleted event is raised,
            // the PingCompletedCallback method is called.
            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            // Wait 12 seconds for a reply.
            int timeout = 1200;

            // Set options for transmission:
            // The data can go through 64 gateways or routers
            // before it is destroyed, and the data packet
            // cannot be fragmented.
            PingOptions options = new PingOptions(64, true);

            //Console.WriteLine("Time to live: {0}", options.Ttl);
            //Console.WriteLine("Don't fragment: {0}", options.DontFragment);

            // Send the ping asynchronously.
            // Use the waiter as the user token.
            // When the callback completes, it can wake up this thread.

            try
            {
                pingSender.SendAsync((string)ip, timeout, buffer, options, waiter);

            }
            catch (PingException e)
            {
                Console.WriteLine(e.ToString());
            }

            // Prevent this example application from ending.
            // A real application should do something useful
            // when possible.
            //waiter.WaitOne();
            // Console.WriteLine("Ping example completed.");
        }

        public  void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            // If the operation was canceled, display a message to the user.
            if (e.Cancelled)
            {
                Console.WriteLine("Ping canceled.");

                // Let the main thread resume. 
                // UserToken is the AutoResetEvent object that the main thread 
                // is waiting for.
                ((AutoResetEvent)e.UserState).Set();
            }

            // If an error occurred, display the exception to the user.
            if (e.Error != null)
            {
                Console.WriteLine("Ping failed:");
                Console.WriteLine(e.Error.ToString());

                // Let the main thread resume. 
                ((AutoResetEvent)e.UserState).Set();
            }

            if (e.Reply.Status == IPStatus.Success)
            {
                PingReply reply = e.Reply;

                if (PingDataReceived != null)
                {
                    String ip = reply.Address.ToString();
                    if (!Conf.Constant.UNMANAGED_IPS.Contains(ip))
                    {

                    
                        //add user
                        Model.UserInfo userinfo = new Model.UserInfo();
                        userinfo.Ip = reply.Address.ToString();
                        userinfo.PhysicalAddress = Util.Util.GetMacAddressFromIP(userinfo.Ip);

                        userinfo.GroupName = Util.Util.GetGroupName(userinfo.Ip);
                        userinfo.State = Model.UserInfo.ClientState.UNINSTALL;

                        Boolean isAddNewUser = Model.UserListManager.Instance.AddUser(userinfo);

                        //Notify viewer
                   //     if (isAddNewUser)
                        {
                            PingDataReceived(userinfo.PhysicalAddress);
                        }
                      
                    }

                }

            }

            // Let the main thread resume.
            ((AutoResetEvent)e.UserState).Set();
        }
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios. 
        // If disposing equals true, the method has been called directly 
        // or indirectly by a user's code. Managed and unmanaged resources 
        // can be disposed. 
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed. 
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                   // component.Dispose();
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here. 
                // If disposing is false, 
                // only the following code is executed.
               // CloseHandle(handle);
               // handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;

            }
        }

        ~PingSender()
        {
            Dispose();
        }

    }
}

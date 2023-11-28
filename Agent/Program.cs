using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Forms;
using System.Security.Permissions;

namespace Agent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;

            String[] s = new String[] { };
            App app = new App();
            app.Run(s);
        }
    }

    /// <summary>
    ///  We inherit from WindowsFormApplicationBase which contains the logic for the application model, including
    ///  the single-instance functionality.
    /// </summary>
    class App : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {
        public App()
        {
            IsSingleInstance = true; // makes this a single-instance app
            ShutdownStyle = Microsoft.VisualBasic.ApplicationServices.ShutdownMode.AfterMainFormCloses; // the vb app model supports two different shutdown styles.  We'll use this one for the sample.
        }

        /// <summary>
        /// This is how the application model learns what the main form is
        /// </summary>
        protected override void OnCreateMainForm()
        {
            this.MainForm = new Controller.Manager();
        }

        /// <summary>
        /// Gets called when subsequent application launches occur.  The subsequent app launch will result in this function getting called
        /// and then the subsequent instances will just exit.  You might use this method to open the requested doc, or whatever 
        /// </summary>
        /// <param name="eventArgs"></param>
        protected override void OnStartupNextInstance(Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs eventArgs)
        {
            base.OnStartupNextInstance(eventArgs);
            System.Windows.Forms.MessageBox.Show("RCMS is already running", "RCMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

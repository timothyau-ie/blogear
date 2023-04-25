using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace blocal
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //disable the following to allow having more than 1 blogear (directed to different paths)
            //try
            //{
            //    // get the name of our process
            //    string proc = Process.GetCurrentProcess().ProcessName;
            //    // get the list of all processes by that name
            //    Process[] processes = Process.GetProcessesByName(proc);
            //    // if there is more than one process...
            //    if (processes.Length > 1)
            //    {
            //        MessageBox.Show("Another Blogear is running.", "Blogear");
            //        return;
            //    }

            //}
            //catch (Exception)
            //{
            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        } 
    }
}

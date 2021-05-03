/*
 * @author            : Blake Pell
 * @initial date      : 2007-03-31
 * @last updated      : 2021-05-02
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AudioRecorder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Set the high DPI awareness for supported OS's.
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

    }
}

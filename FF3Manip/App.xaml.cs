using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FF3Manip
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string savedTimeZone = string.Empty;

        
        private void AppExit(object sender, ExitEventArgs e)
        {
            
            RevertTimeZone();
            SyncTime();
        }

        private void AppStart(object sender, StartupEventArgs e)
        {
            // Store local timezone on startup so that we can revert it later
            savedTimeZone = TimeZoneInfo.Local.StandardName;
        }

        private void RevertTimeZone()
        {
            string args = "/s \"" + savedTimeZone + "\"";
            ProcessStartInfo timeZoneSync = new ProcessStartInfo("tzutil.exe", args);
            timeZoneSync.CreateNoWindow = true;
            timeZoneSync.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(timeZoneSync);

            p.WaitForExit();
            TimeZoneInfo.ClearCachedData();
        }

        private void SyncTime()
        {
            ProcessStartInfo timeSync = new ProcessStartInfo("w32tm.exe", " /resync");
            timeSync.CreateNoWindow = true;
            timeSync.Verb = "runas";
            timeSync.UseShellExecute = true;
            timeSync.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(timeSync);
            p.WaitForExit();
        }
    }
}
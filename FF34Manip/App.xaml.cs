using System;
using System.Diagnostics;
using System.Windows;

namespace FF34Manip
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
            using(Process timeZoneRevertProcess = new Process
                  {
                      StartInfo = new ProcessStartInfo
                      {
                          FileName = "tzutil.exe",
                          Arguments = $"/s \"{savedTimeZone}\"",
                          CreateNoWindow = true,
                          WindowStyle = ProcessWindowStyle.Hidden
                      }
                  })
            {
                timeZoneRevertProcess.Start();
                timeZoneRevertProcess.WaitForExit();
                timeZoneRevertProcess.Close();
            }
            TimeZoneInfo.ClearCachedData();
        }

        private void SyncTime()
        {
            using (Process syncProcess = new Process
                   {
                       StartInfo = new ProcessStartInfo
                       {
                           FileName = "w32tm.exe",
                           Arguments = "/resync",
                           CreateNoWindow = true,
                           Verb = "runas",
                           UseShellExecute = true,
                           WindowStyle = ProcessWindowStyle.Hidden
                       }
                   })
            {
                syncProcess.Start();
                syncProcess.WaitForExit();
                syncProcess.Close();
            }
        }
    }
}
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace FF3Manip
{
    public class ManipController
    {
        private string savedTimeZone;
        private string currentTimeZone;

        private enum DateFormats
        {
            DDMMYYYY,
            MMDDYYYY,
            YYYYMMDD
        }

        public static class TimeZones
        {
            public const string ET = "Eastern Standard Time";
            public const string UTC = "UTC";
            public const string JST = "Tokyo Standard Time";
            public const string GMT = "GMT Standard Time";
            public const string CEST = "W. Europe Standard Time";
            // Add more as needed - string needs to match output from tzutil.exe /l
        }
        
        private DateFormats GetDateFormat()
        {
            string datePattern = MainWindow.systemDateFormat;
            if (datePattern[0] == 'd')
            {
                return DateFormats.DDMMYYYY;
            }

            if (datePattern[0] == 'M')
            {
                return DateFormats.MMDDYYYY;
            }
            return DateFormats.YYYYMMDD;
        }

        private bool GameRunning()
        {
            return Process.GetProcessesByName("FF3_Win32").Length > 0;
        }

        public void ExecuteManip(ManipList.ManipNames name)
        {
            savedTimeZone = TimeZoneInfo.Local.StandardName;

            ManipList manipList = new ManipList();
            SetDateTime(manipList.GetManipByValue(name));
        }

        private void SetTimeZone(string targetTimeZone)
        {
            string args = "/s \"" + targetTimeZone + "\"";
            using(Process setTimeZoneProcess = new Process
                  {
                      StartInfo = new ProcessStartInfo
                      {
                          FileName = "tzutil.exe",
                          Arguments = args,
                          CreateNoWindow = true,
                          WindowStyle = ProcessWindowStyle.Hidden
                      }
                  })
            {
                setTimeZoneProcess.Start();
                setTimeZoneProcess.WaitForExit();
                setTimeZoneProcess.Close();
            }
            
            currentTimeZone = targetTimeZone;
        }

        private void SetDateTime(Manip targetManip)
        {
            if (currentTimeZone != targetManip.TimeZone)
            {
                SetTimeZone(targetManip.TimeZone);
            }

            // Format strings for use in cmd
            string time = targetManip.Hour + ":" + targetManip.Minute + ":" + targetManip.Second + ".00";
            string date = string.Empty;

            switch (GetDateFormat())
            {
                case DateFormats.DDMMYYYY:
                    date = $"{targetManip.Day}-{targetManip.Month}-{targetManip.Year}";
                    break;
                case DateFormats.MMDDYYYY:
                    date = $"{targetManip.Month}-{targetManip.Day}-{targetManip.Year}";
                    break;
                case DateFormats.YYYYMMDD:
                    date = $"{targetManip.Year}-{targetManip.Month}-{targetManip.Day}";
                    break;
            }

            using (Process setTimeProcess = new Process
                   {
                       StartInfo = new ProcessStartInfo
                       {
                           FileName = "cmd.exe",
                           Arguments = "/C time " + time,
                           CreateNoWindow = true,
                           Verb = "runas",
                           UseShellExecute = true,
                           WindowStyle = ProcessWindowStyle.Hidden
                       }
                   })
            {
                setTimeProcess.Start();
                setTimeProcess.WaitForExit();
                setTimeProcess.Close();
            }

            using (Process setDateProcess = new Process
                   {
                       StartInfo = new ProcessStartInfo
                       {
                           FileName = "cmd.exe",
                           Arguments = "/C date " + date,
                           CreateNoWindow = true,
                           Verb = "runas",
                           UseShellExecute = true,
                           WindowStyle = ProcessWindowStyle.Hidden
                       }
                   })
            {
                setDateProcess.Start();
                setDateProcess.WaitForExit();
                setDateProcess.Close();
            }

            // Set time every 0.1s
            // Fast enough to not creep into the next second, but not so fast that we melt CPUs
            Thread.Sleep(100);
            try
            {
                if (!GameRunning() && Application.Current.MainWindow.IsActive)
                {
                    SetDateTime(targetManip);
                }
            }
            catch
            {
                // Program closed while a manip is active, or otherwise crashes
                RevertTime();
            }
        }

        private void RevertTime()
        {
            // Small buffer to allow the game to launch before reverting
            Thread.Sleep(2000);
            // Revert Time zone
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
;
            TimeZoneInfo.ClearCachedData();
            currentTimeZone = savedTimeZone;

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
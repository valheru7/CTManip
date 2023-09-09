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
            return Process.GetProcessesByName("FF4").Length > 0;
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
            ProcessStartInfo setTimeZone = new ProcessStartInfo("tzutil.exe", args);
            setTimeZone.CreateNoWindow = true;
            setTimeZone.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(setTimeZone);
            p.WaitForExitAsync();
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
            
            ProcessStartInfo setTime = new ProcessStartInfo("cmd.exe", "/C time " + time);
            setTime.CreateNoWindow = true;
            setTime.Verb = "runas";
            setTime.UseShellExecute = true;
            setTime.WindowStyle = ProcessWindowStyle.Hidden;
            Process setTimeProcess = Process.Start(setTime);
            setTimeProcess.WaitForExitAsync();
            
            ProcessStartInfo setDate = new ProcessStartInfo("cmd.exe", "/C date " + date);
            setDate.CreateNoWindow = true;
            setDate.Verb = "runas";
            setDate.UseShellExecute = true;
            setDate.WindowStyle = ProcessWindowStyle.Hidden;
            Process setDateProcess = Process.Start(setDate);
            setDateProcess.WaitForExitAsync();
      
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
            string args = $"/s \"{savedTimeZone}\"";
            ProcessStartInfo timeZoneSync = new ProcessStartInfo("tzutil.exe", args);
            timeZoneSync.CreateNoWindow = true;
            timeZoneSync.WindowStyle = ProcessWindowStyle.Hidden;
            Process tzProcess = Process.Start(timeZoneSync);

            tzProcess.WaitForExit();
            TimeZoneInfo.ClearCachedData();
            currentTimeZone = savedTimeZone;

            // Sync time
            ProcessStartInfo timeSync = new ProcessStartInfo("w32tm.exe", "/resync");
            timeSync.CreateNoWindow = true;
            timeSync.Verb = "runas";
            timeSync.UseShellExecute = true;
            timeSync.WindowStyle = ProcessWindowStyle.Hidden;
            Process syncProcess = Process.Start(timeSync);
            if (syncProcess != null)
            {
                syncProcess.WaitForExit();
            }
        }
    }
}
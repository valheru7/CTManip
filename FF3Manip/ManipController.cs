using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace FF3Manip
{
    public class ManipController
    {
        private string savedTimeZone;

        public enum DateFormats
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
        
        public DateFormats GetDateFormat()
        {
            if (CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern.StartsWith("d"))
            {
                return DateFormats.DDMMYYYY;
            }

            if (CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern.StartsWith("m"))
            {
                return DateFormats.MMDDYYYY;
            }

            return DateFormats.YYYYMMDD;
        }

        private bool GameRunning()
        {
            return Process.GetProcessesByName("FF4_Win32").Length > 0;
        }

        public void ExecuteManip(ManipList.ManipNames name)
        {
            savedTimeZone = TimeZoneInfo.Local.StandardName;

            ManipList manipList = new ManipList();
            SetDateTime(manipList.GetManipByValue(name));
        }

        public void SetDateTime(Manip targetManip)
        {
            // Set time zone
            string args = "/s \"" + targetManip.TimeZone + "\"";
            //Process p = Process.Start("tzutil.exe", args);
            ProcessStartInfo setTimeZone = new ProcessStartInfo("tzutil.exe", args);
            setTimeZone.CreateNoWindow = true;
            setTimeZone.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(setTimeZone);
            p.WaitForExitAsync();
            

            // Format strings for use in cmd
            string time = targetManip.Hour + ":" + targetManip.Minute + ":" + targetManip.Second + ".00";
            string date = String.Empty;

            switch (GetDateFormat())
            {
                case DateFormats.DDMMYYYY:
                    date = targetManip.Day + "-" + targetManip.Month + "-" + targetManip.Year;
                    break;
                case DateFormats.MMDDYYYY:
                    date = targetManip.Month + "-" + targetManip.Day + "-" + targetManip.Year;
                    break;
                case DateFormats.YYYYMMDD:
                    date = targetManip.Year + "-" + targetManip.Month + "-" + targetManip.Day;
                    break;
            }

            TaskCompletionSource<bool> TimeEventHandled;
            TaskCompletionSource<bool> DateEventHandled;

            async Task SetTimeAsync()
            {
                TimeEventHandled = new TaskCompletionSource<bool>();

                ProcessStartInfo setTime = new ProcessStartInfo("cmd.exe", "/C time " + time);
                setTime.CreateNoWindow = true;
                setTime.Verb = "runas";
                setTime.UseShellExecute = true;
                setTime.WindowStyle = ProcessWindowStyle.Hidden;
                Process p = Process.Start(setTime);
                p.WaitForExitAsync();
                
                await Task.WhenAny(TimeEventHandled.Task);
            }

            async Task SetDateAsync()
            {
                DateEventHandled = new TaskCompletionSource<bool>();

                ProcessStartInfo setDate = new ProcessStartInfo("cmd.exe", "/C date " + date);
                setDate.CreateNoWindow = true;
                setDate.Verb = "runas";
                setDate.UseShellExecute = true;
                setDate.WindowStyle = ProcessWindowStyle.Hidden;
                Process p = Process.Start(setDate);
                p.WaitForExitAsync();

                await Task.WhenAny(DateEventHandled.Task);
            }
            
            Task.Run(async () =>
            {
                await SetTimeAsync();
            });
            Task.Run(async () =>
            {
                await SetDateAsync();
            });
            
            // Set time every 0.1s
            // Fast enough to not creep into the next second, but not so fast that we melt CPUs
            Thread.Sleep(100);
            if (!GameRunning())
            {
                SetDateTime(targetManip);
                return;
            }

            RevertTime();
        }

        public void RevertTime()
        {
            // Small buffer to allow the game to launch before reverting
            Thread.Sleep(2000);
            // Revert Time zone
            string args = "/s \"" + savedTimeZone + "\"";
            ProcessStartInfo timeZoneSync = new ProcessStartInfo("tzutil.exe", args);
            timeZoneSync.CreateNoWindow = true;
            timeZoneSync.WindowStyle = ProcessWindowStyle.Hidden;
            Process tzProcess = Process.Start(timeZoneSync);

            tzProcess.WaitForExit();
            TimeZoneInfo.ClearCachedData();

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
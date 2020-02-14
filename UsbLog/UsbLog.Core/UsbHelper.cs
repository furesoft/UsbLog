using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;

namespace UsbLog.Core
{
    public class UsbHelper
    {
        public static bool IsConfigured(string drivename)
        {
            DISK.SafeStreamManager myStream;

            myStream = DISK.CreateStream(drivename, FileAccess.Read);

            try
            {
                var magic = DISK.ReadBytes(0, 4, myStream);
                Console.WriteLine(magic);
            }
            catch { }

            DISK.DropStream(myStream);

            return false;
        }

        public static void OnAttach(Action<string> callback)
        {
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");

            insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler((_, e) =>
            {
                var drives = DriveInfo.GetDrives().Where(__ => __.DriveType == DriveType.Removable);
                string phyd = @"\\.\" + drives.FirstOrDefault().Name;

                List<string> allDrives = DriveAccess.GetAllDrives(null);

                if (phyd != null)
                {
                    callback(phyd);
                }
            });

            insertWatcher.Start();
        }

        public static void OnDettach(Action<string> callback)
        {
            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

            removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler((_, e) =>
            {
                var drives = DriveInfo.GetDrives().Where(__ => __.DriveType == DriveType.Removable);
                string phyd = @"\\.\" + drives?.FirstOrDefault().Name;

                if (phyd != null)
                {
                    callback(phyd);
                }
            });

            removeWatcher.Start();
        }

        public static void Stop()
        {
            insertWatcher.Stop();
            removeWatcher.Stop();
        }

        private static ManagementEventWatcher insertWatcher;
        private static ManagementEventWatcher removeWatcher;
    }
}
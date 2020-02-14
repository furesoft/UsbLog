using System;
using System.IO;
using System.Linq;
using System.Management;

namespace UsbLog.Core
{
    public class UsbEvent
    {
        public static void OnAttach(Action<DriveInfo[]> callback)
        {
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");

            insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler((_, e) =>
            {
                var drives = DriveInfo.GetDrives().Where(__ => __.DriveType == DriveType.Removable);
                callback(drives.ToArray());
            });

            insertWatcher.Start();
        }

        public static void OnDettach(Action<DriveInfo[]> callback)
        {
            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

            removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler((_, e) =>
            {
                var drives = DriveInfo.GetDrives().Where(__ => __.DriveType == DriveType.Removable);
                callback(drives.ToArray());
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
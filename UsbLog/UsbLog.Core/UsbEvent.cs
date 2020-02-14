using System;
using System.Management;

namespace UsbLog.Core
{
    internal class UsbEvent
    {
        public static void OnAttach(Action<string> callback)
        {
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

            ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler((_, e) => callback(e.NewEvent.Properties["DriveName"].ToString()));
            insertWatcher.Start();
        }

        public static void OnDettach(Action<string> callback)
        {
            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

            ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler((_, e) => callback(e.NewEvent.Properties["DriveName"].ToString()));
            removeWatcher.Start();
        }
    }
}
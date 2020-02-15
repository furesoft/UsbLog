using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;

namespace UsbLog.Core
{
    public class UsbHelper
    {
        public static bool IsConfigured(string name)
        {
            DISK.SafeStreamManager myStream;

            myStream = DISK.CreateStream($@"\\.\{name}", FileAccess.ReadWrite);

            byte[] firstBlock;

            if (!myStream.f_error)
            {
                firstBlock = DISK.ReadBytes(0, 4, myStream);

                Logger.Trace(Encoding.ASCII.GetString(firstBlock));

                try
                {
                    Logger.Trace($"Read USB Magic Number");
                    var magic = DISK.ReadBytes(0, 4, myStream);
                    Logger.Trace(magic);
                }
                catch (Exception ex)
                {
                    Logger.Trace(ex.ToString());
                }

                DISK.DropStream(myStream);
            }

            return false;
        }

        public static void OnAttach(Action<string> callback)
        {
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");

            insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler((_, e) =>
            {
                var drives = DriveInfo.GetDrives().Where(__ => __.DriveType == DriveType.Removable);

                Logger.Trace($"USB inserted");
                callback(drives?.FirstOrDefault()?.Name.Replace("\\", ""));
            });

            insertWatcher.Start();
        }

        public static void OnDettach(Action callback)
        {
            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

            removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler((_, e) =>
            {
                Logger.Trace($"USB ejected");
                callback();
            });

            removeWatcher.Start();
        }

        public static void Stop()
        {
            insertWatcher.Stop();
            removeWatcher.Stop();
        }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static ManagementEventWatcher insertWatcher;
        private static ManagementEventWatcher removeWatcher;
    }
}
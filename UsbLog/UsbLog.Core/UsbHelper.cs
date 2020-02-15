using DiscUtils.Registry;
using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;

namespace UsbLog.Core
{
    public class UsbHelper
    {
        public static void Configure(DISK.SafeStreamManager myStream, Action<RegistryKey> configurator)
        {
            var ms = new MemoryStream();

            var regf = RegistryHive.Create(ms);
            configurator(regf.Root);

            var buffer = ms.ToArray();

            DISK.WriteBytes(0, buffer.Length, buffer, myStream);
        }

        public static DISK.SafeStreamManager GetStream(string name)
        {
            return DISK.CreateStream($@"\\.\{name}", FileAccess.ReadWrite);
        }

        public static void Initialize(DISK.SafeStreamManager myStream)
        {
            Configure(myStream, _ =>
            {
                _.SetValue("Version", new Version(1, 0, 0, 0));
                _.SetValue("ByteCode", new byte[] { 0x1 });

                _.CreateSubKey("Config");
                _.CreateSubKey("Update");

                var data = _.CreateSubKey("Data");
                data.CreateSubKey("Keys");
                data.CreateSubKey("ScreenShots");
            });
        }

        public static (bool, DISK.SafeStreamManager) IsConfigured(string name)
        {
            DISK.SafeStreamManager myStream;

            myStream = DISK.CreateStream($@"\\.\{name}", FileAccess.ReadWrite);

            if (!myStream.f_error)
            {
                try
                {
                    Logger.Trace($"Read USB Magic Number");
                    var magic = DISK.ReadBytes(0, 4, myStream);
                    var magicString = Encoding.ASCII.GetString(magic);

                    return (magicString == "regf", myStream);
                }
                catch (Exception ex)
                {
                    Logger.Trace(ex.ToString());
                }
            }

            return (false, default(DISK.SafeStreamManager));
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
            insertWatcher?.Stop();
            removeWatcher?.Stop();
        }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static ManagementEventWatcher insertWatcher;
        private static ManagementEventWatcher removeWatcher;
    }
}
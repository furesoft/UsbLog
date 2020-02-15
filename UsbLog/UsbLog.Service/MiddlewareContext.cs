using DiscUtils.Registry;
using UsbLog.Core;

namespace UsbLog.Service
{
    internal class MiddlewareContext
    {
        public RegistryHive Hive { get; set; }

        public DISK.SafeStreamManager Strm { get; set; }
    }
}
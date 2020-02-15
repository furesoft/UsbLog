using Topshelf;
using UsbLog.Core;

namespace UsbLog.Service
{
    public class LogginService : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            Logger.Trace($"Service started");

            UsbHelper.OnAttach(_ =>
            {
                var configured = UsbHelper.IsConfigured(_);
            });

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Logger.Trace($"Service stopped");

            UsbHelper.Stop();

            return true;
        }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
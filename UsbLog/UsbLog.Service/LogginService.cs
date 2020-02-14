using Topshelf;
using UsbLog.Core;

namespace UsbLog.Service
{
    public class LogginService : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            UsbHelper.OnAttach(_ =>
            {
                var configured = UsbHelper.IsConfigured(_);
            });

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            UsbHelper.Stop();
            return true;
        }
    }
}
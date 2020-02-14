using Topshelf;
using UsbLog.Core;

namespace UsbLog.Service
{
    public class LogginService : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            UsbEvent.OnAttach(_ =>
            {
                foreach (var d in _)
                {
                    System.Console.WriteLine(d.Name);
                }
            });
            UsbEvent.OnDettach(_ =>
            {
                foreach (var d in _)
                {
                    System.Console.WriteLine(d.Name);
                }
            });

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            UsbEvent.Stop();
            return true;
        }
    }
}
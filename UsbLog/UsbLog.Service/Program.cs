using System;
using Topshelf;

namespace UsbLog.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<LogginService>();
                x.EnableServiceRecovery(r => r.RestartService(TimeSpan.FromSeconds(10)));
                x.SetServiceName("UsbLog");
                x.SetDescription("UsbLog  Service");
                x.StartAutomatically();
                x.UseNLog();
            });
        }
    }
}
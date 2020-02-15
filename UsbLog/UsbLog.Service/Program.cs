using System;
using Topshelf;

namespace UsbLog.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: Console
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets
            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logconsole);

            // Apply config
            NLog.LogManager.Configuration = config;

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
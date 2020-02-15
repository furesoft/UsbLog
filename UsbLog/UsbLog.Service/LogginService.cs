using PipelineNet.MiddlewareResolver;
using PipelineNet.Pipelines;
using Topshelf;
using UsbLog.Core;
using UsbLog.Service.Middleware;

namespace UsbLog.Service
{
    public class LogginService : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            Logger.Trace($"Service started");

            UsbHelper.OnAttach(_ =>
            {
                var (configured, stream) = UsbHelper.IsConfigured(_);
                Logger.Trace($"IsConfigured: {configured}");

                if (configured)
                {
                    var pipeline = new Pipeline<MiddlewareContext>(new ActivatorMiddlewareResolver());

                    pipeline.Add<InitMiddleware>();
                    pipeline.Add<UpdateServiceMiddleware>();
                    pipeline.Add<ReadByteCodeMiddleware>();
                    pipeline.Add<StoreDataMiddleware>();
                    pipeline.Add<CleanupMiddleware>();

                    var context = new MiddlewareContext();
                    context.Strm = UsbHelper.GetStream(_);

                    pipeline.Execute(context);
                }
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
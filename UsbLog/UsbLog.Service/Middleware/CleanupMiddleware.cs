using PipelineNet.Middleware;
using System;
using UsbLog.Core;

namespace UsbLog.Service.Middleware
{
    internal class CleanupMiddleware : IMiddleware<MiddlewareContext>
    {
        public void Run(MiddlewareContext strm, Action<MiddlewareContext> next)
        {
            DISK.DropStream(strm.Strm);

            next(strm);
        }
    }
}
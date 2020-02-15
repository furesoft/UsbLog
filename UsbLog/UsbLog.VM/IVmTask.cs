using DiscUtils.Registry;

namespace UsbLog.VM
{
    public interface IVmTask
    {
        int ID { get; }

        void Invoke(RegistryKey root);
    }
}
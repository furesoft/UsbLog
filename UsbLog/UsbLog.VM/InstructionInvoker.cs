using DiscUtils.Registry;
using System.Collections.Generic;

namespace UsbLog.VM
{
    public class InstructionInvoker
    {
        public static void AddTask(IVmTask task)
        {
            _tasks.Add(task.ID, task);
        }

        public static void Invoke(byte[] bytecode, RegistryKey root)
        {
            byte opcode = 0;
            int iP = -1;

            while (opcode != 0x1)
            {
                opcode = bytecode[iP++];

                _tasks[opcode].Invoke(root);
            }
        }

        private static Dictionary<int, IVmTask> _tasks = new Dictionary<int, IVmTask>();
    }
}
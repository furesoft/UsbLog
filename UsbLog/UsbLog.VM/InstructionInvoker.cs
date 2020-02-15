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
            int iP = 0;

            do
            {
                opcode = bytecode[iP];

                if (opcode != 0x1)
                {
                    _tasks[opcode].Invoke(root);
                }

                iP++;
            } while (opcode != 0x1);
        }

        private static Dictionary<int, IVmTask> _tasks = new Dictionary<int, IVmTask>();
    }
}
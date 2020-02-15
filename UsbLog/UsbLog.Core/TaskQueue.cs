using System;
using System.Collections.Generic;

namespace UsbLog.Core
{
    public static class TaskQueue
    {
        public static void Add(int id, Action action)
        {
            if (!_tasks.ContainsKey(id))
            {
                _tasks.Add(id, action);
            }
        }

        public static void RunAllTasks()
        {
            foreach (var t in _tasks)
            {
                t.Value();
            }
        }

        public static void RunTask(int id)
        {
            _tasks[id]();
        }

        private static Dictionary<int, Action> _tasks = new Dictionary<int, Action>();
    }
}
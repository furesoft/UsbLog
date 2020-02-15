using System;
using System.Collections.Generic;
using System.Text;

namespace UsbLog.Core
{
    public static class DisposePipe
    {
        public static void Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public static void DisposeAll()
        {
            foreach (var d in _disposables)
            {
                d.Dispose();
            }
        }

        private static List<IDisposable> _disposables = new List<IDisposable>();
    }
}
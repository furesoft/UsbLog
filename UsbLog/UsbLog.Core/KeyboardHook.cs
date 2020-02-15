using System;
using System.Runtime.InteropServices;

namespace UsbLog.Core
{
    public sealed class KeyboardHook : IDisposable
    {
        #region Constant, Structure and Delegate Definitions

        public KeyboardHook()
        {
            khp = new keyboardHookProc(hookProc);
            hook();
        }

        public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);

        public delegate void KeyEventHandler(int keycode);

        public event KeyEventHandler KeyDown;

        public event KeyEventHandler KeyUp;

        public void Dispose()
        {
            unhook();
        }

        public void hook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, khp, hInstance, 0);
        }

        public int hookProc(int code, int wParam, ref keyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && (KeyDown != null))
                {
                    KeyDown(lParam.vkCode);
                }
                else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && (KeyUp != null))
                {
                    KeyUp(lParam.vkCode);
                }
            }

            return CallNextHookEx(hhook, code, wParam, ref lParam);
        }

        public void unhook()
        {
            UnhookWindowsHookEx(hhook);
        }

        public struct keyboardHookStruct
        {
            public int dwExtraInfo;
            public int flags;
            public int scanCode;
            public int time;
            public int vkCode;
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        #endregion Constant, Structure and Delegate Definitions

        private IntPtr hhook = IntPtr.Zero;
        private keyboardHookProc khp;

        #region Constructors and Destructors

        ~KeyboardHook()
        {
            unhook();
        }

        #endregion Constructors and Destructors

        #region DLL imports

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        #endregion DLL imports
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinApi
{
    public class GlobalKeyboardHookEventArgs : HandledEventArgs
    {
        public WindowsApi.KeyboardState KeyboardState { get; private set; }
        public WindowsApi.LowLevelKeyboardInputEvent KeyboardData { get; private set; }

        public GlobalKeyboardHookEventArgs(
            WindowsApi.LowLevelKeyboardInputEvent keyboardData,
            WindowsApi.KeyboardState keyboardState)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
        }
    }

    public class GlobalKeyboardHook : IDisposable
    {
        private bool _disposed = false;
        private IntPtr _hookId;
        public event EventHandler<GlobalKeyboardHookEventArgs>? KeyboardPressed;
        private static WindowsApi.HookProc _hookDelegate;
        
        public GlobalKeyboardHook()
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule;
            _hookDelegate = LowLevelKeyboardProc;
            _hookId = WindowsApi.SetWindowsHookEx(WindowsApi.WhKeyboardLl, _hookDelegate, WindowsApi.GetModuleHandle(curModule.ModuleName), 0);
        }
        
        private IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var wParamTyped = wParam.ToInt32();
            
            var o = Marshal.PtrToStructure(lParam, typeof(WindowsApi.LowLevelKeyboardInputEvent));
            if (!Enum.IsDefined(typeof(WindowsApi.KeyboardState), wParamTyped) || o is null)
                return WindowsApi.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            
            var p = (WindowsApi.LowLevelKeyboardInputEvent)o;
            var eventArguments = new GlobalKeyboardHookEventArgs(p, (WindowsApi.KeyboardState)wParamTyped);

            KeyboardPressed?.Invoke(null, eventArguments);
            
            return eventArguments.Handled ? (IntPtr)1 : WindowsApi.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        ~GlobalKeyboardHook()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(_disposed) return;

            if (_hookId != IntPtr.Zero)
            {
                WindowsApi.UnhookWindowsHookEx(_hookId);
            }

            _hookId = IntPtr.Zero;
            _disposed = true;
        }
    }
}

using System.Drawing;
using System.Runtime.InteropServices;

namespace WinApi
{
    public static class WindowsApi
    {
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public const int GwlExStyle = -20;
        
        [Flags]
        public enum WindowStyle : int
        {
            WsExTransparent = 0x00000020,
            WsExLayered = 0x00080000,
        }
        
        public enum SystemMetric
        {
            SmCxScreen = 0,
            SmCyScreen = 1,
            VirtualScreenWidth = 78,
            VirtualScreenHeight = 79,
        }

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric metric);

        public static Size GetScreenSize()
        {
            var width = GetSystemMetrics(SystemMetric.SmCxScreen);
            var height = GetSystemMetrics(SystemMetric.SmCyScreen);

            return new Size(width, height);
        }
        
        public static Size GetVirtualDisplaySize()
        {
            var width = GetSystemMetrics(SystemMetric.VirtualScreenWidth);
            var height = GetSystemMetrics(SystemMetric.VirtualScreenHeight);

            return new Size(width, height);
        }
        
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GwlExStyle);
            SetWindowLong(hwnd, GwlExStyle, extendedStyle | (int)WindowStyle.WsExTransparent | (int)WindowStyle.WsExLayered);
        }

        public static void SetWindowExNotTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GwlExStyle);
            SetWindowLong(hwnd, GwlExStyle, extendedStyle & ~(int)WindowStyle.WsExTransparent);
        }
        
        public const int WhKeyboardLl = 13;
    
        [StructLayout(LayoutKind.Sequential)]
        public struct LowLevelKeyboardInputEvent
        {
            public int VirtualCode;
            public int HardwareScanCode;
            public int Flags;
            public int TimeStamp;
            public IntPtr AdditionalInformation;
        }

        public enum KeyboardState
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SysKeyDown = 0x0104,
            SysKeyUp = 0x0105
        }
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc hookProc, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        
        [Flags]
        public enum ProcessAccessRights : int
        {
            ProcessCreateProcess = 0x0080,
            ProcessCreateThread = 0x0002,
            ProcessDupHandle = 0x0040,
            ProcessQueryInformation = 0x0400,
            ProcessQueryLimitedInformation = 0x1000,
            ProcessSetInformation = 0x0200,
            ProcessSetQuota = 0x0100,
            ProcessSuspendResume = 0x0800,
            ProcessTerminate = 0x0001,
            ProcessVmOperation = 0x0008,
            ProcessVmRead = 0x0010,
            ProcessVmWrite = 0x0020,
            Synchronize = 0x00100000,
            ProcessAllAccess = 0x000F0000 | 0x00100000 | 0xFFFF,
        }
    
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessRights dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, uint dwSize, out int lpNumberOfBytesRead);
        
        [Flags]
        public enum InputType : uint
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }
        
        [Flags]
        public enum MouseEvent : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            MoveNoCoalesce = 0x2000,
            VirtualDesk = 0x4000,
            Wheel = 0x0800,
            Absolute  = 0x8000,
        }
        
        [Flags]
        public enum KeyboardEvent : uint
        {
            KeyDown = 0x0000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            Scancode = 0x0008
        }
        
        public const uint XBUTTON1 = 0x0001;
        public const uint XBUTTON2 = 0x0002;
        
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardInput
        {
            public ushort vk;
            public ushort scan;
            public uint flags;
            public uint time;
            public IntPtr extraInfo;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct HardwareInput
        {
            public uint msg;
            public ushort paramL;
            public ushort paramH;
        }
        
        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)] public MouseInput mi;
            [FieldOffset(0)] public KeyboardInput ki;
            [FieldOffset(0)] public HardwareInput hi;
        }  

        [StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public uint type;
            public InputUnion u;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] Input[] pInputs, int cbSize);
        
        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();
        
        [DllImport("user32.dll")]
        public static extern ushort MapVirtualKey(ushort uCode, uint uMapType);
        
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int x;
            public int y;
        }
    }
}
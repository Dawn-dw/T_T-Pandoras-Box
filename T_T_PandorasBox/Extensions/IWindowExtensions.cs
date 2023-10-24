using Silk.NET.Windowing;
using WinApi;

namespace T_T_PandorasBox.Extensions
{
    public static class IWindowExtensions
    {
        public static void SetWindowExTransparent(this IWindow window)
        {
            if(window?.Native is null) return;
            var wh = window.Native.Win32;
            WindowsApi.SetWindowExTransparent(wh.Value.Hwnd);
        }

        public static void SetWindowExNotTransparent(this IWindow window)
        {
            if(window?.Native is null) return;
            var wh = window.Native.Win32;
            WindowsApi.SetWindowExNotTransparent(wh.Value.Hwnd);
        }
    }
}

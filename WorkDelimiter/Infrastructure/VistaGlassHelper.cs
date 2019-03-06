using System;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace WorkDelimiter.Infrastructure
{
    public static class VistaGlassHelper
    {
        [DllImport("DwmApi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(
               IntPtr hwnd,
               ref Margins pMarInset);

        [StructLayout(LayoutKind.Sequential)]
        public struct Margins
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }
        public static Margins GetDpiAdjustedMargins(IntPtr windowHandle, int left, int right, int top, int bottom)
        {
            // Получить Dpi системы
            System.Drawing.Graphics desktop = System.Drawing.Graphics.FromHwnd(windowHandle);
            float DesktopDpiX = desktop.DpiX;
            float DesktopDpiY = desktop.DpiY;

            // Установка полей
            VistaGlassHelper.Margins margins = new VistaGlassHelper.Margins();

            margins.cxLeftWidth = Convert.ToInt32(left * (DesktopDpiX / 96));
            margins.cxRightWidth = Convert.ToInt32(right * (DesktopDpiX / 96));
            margins.cyTopHeight = Convert.ToInt32(top * (DesktopDpiX / 96));
            margins.cyBottomHeight = Convert.ToInt32(right * (DesktopDpiX / 96));

            return margins;
        }
        public static void ExtendGlass(Window win, int left, int right, int top, int bottom)
        {
            // Получение Win32-дескриптора для окна WPF
            WindowInteropHelper windowInterop = new WindowInteropHelper(win);
            IntPtr windowHandle = windowInterop.Handle;
            HwndSource mainWindowSrc = HwndSource.FromHwnd(windowHandle);
            mainWindowSrc.CompositionTarget.BackgroundColor = Colors.Transparent;

            VistaGlassHelper.Margins margins =
                VistaGlassHelper.GetDpiAdjustedMargins(windowHandle, left, right, top, bottom);

            int returnVal = VistaGlassHelper.DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);

            if (returnVal < 0)
            {
                throw new NotSupportedException("Operation failed.");
            }
        }
    }
}

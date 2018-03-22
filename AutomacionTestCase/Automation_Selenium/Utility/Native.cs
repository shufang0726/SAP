

namespace Automation_Selenium
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.InteropServices;


    public class Native
    {
        [DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern int BitBlt(IntPtr destDC, int xDest, int yDest, int width, int height, IntPtr sourceDC, int xSource, int ySource, uint rasterOperation);
        [DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr CreateCompatibleBitmap(IntPtr dc, int width, int height);
        [DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr CreateCompatibleDC(IntPtr dc);
        [DllImport("gdi32.dll", EntryPoint = "CreateDCW", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr CreateDC(string driver, IntPtr device, IntPtr output, IntPtr devMode);
        [DllImport("gdi32.dll", ExactSpelling = true)]
        internal static extern int DeleteDC(IntPtr dc);
        [DllImport("gdi32.dll", ExactSpelling = true)]
        internal static extern int DeleteObject(IntPtr gdiObject);
        [DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr SelectObject(IntPtr dc, IntPtr gdi);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);

        const int MF_REMOVE = 0x1000;
        const int SC_RESTORE = 0xF120;
        const int SC_MOVE = 0xF010;
        const int SC_SIZE = 0xF000;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_CLOSE = 0xF060;

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static UInt32 SWP_NOSIZE = 0x0001;
        public static UInt32 SWP_NOMOVE = 0x0002;
        public static UInt32 SWP_NOZORDER = 0x0004;
        public static UInt32 SWP_NOREDRAW = 0x0008;
        public static UInt32 SWP_NOACTIVATE = 0x0010;
        public static UInt32 SWP_FRAMECHANGED = 0x0020;
        public static UInt32 SWP_SHOWWINDOW = 0x0040;
        public static UInt32 SWP_HIDEWINDOW = 0x0080;
        public static UInt32 SWP_NOCOPYBITS = 0x0100;
        public static UInt32 SWP_NOOWNERZORDER = 0x0200;
        public static UInt32 SWP_NOSENDCHANGING = 0x0400;
        public static UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        public static void MaxWindow(IntPtr hwnd)
        {
            SendMessage(hwnd, 0x0112, 0xF030, 0);
        }

        public static void MinWindow(IntPtr hwnd)
        {
            SendMessage(hwnd, 0x0112, 0xF020, 0);
        }

        public static void RestoreWindow(IntPtr hwnd)
        {
            SendMessage(hwnd, 0x0112, 0xF120, 0);
        }

        //public static void SendKeys(AccObject accObj, string keys)
        //{
        //    accObj.ScreenElement.SendKeys(keys);
        //}

        #region Resolution

        public enum DMDO
        {
            DEFAULT = 0,
            D90 = 1,
            D180 = 2,
            D270 = 3
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct DEVMODE
        {
            public const int DM_DISPLAYFREQUENCY = 0x400000;
            public const int DM_PELSWIDTH = 0x80000;
            public const int DM_PELSHEIGHT = 0x100000;
            private const int CCHDEVICENAME = 32;
            private const int CCHFORMNAME = 32;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public DMDO dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        //static extern int ChangeDisplaySettings( DEVMODE lpDevMode, int dwFlags);
        static extern int ChangeDisplaySettings([In] ref DEVMODE lpDevMode, int dwFlags);

        public static void ChangeResolutionSetting(int width, int height)
        {
            long RetVal = 0;
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            dm.dmPelsWidth = width;
            dm.dmPelsHeight = height;
            dm.dmDisplayFrequency = 85;
            dm.dmFields = DEVMODE.DM_PELSWIDTH | DEVMODE.DM_PELSHEIGHT | DEVMODE.DM_DISPLAYFREQUENCY;
            RetVal = ChangeDisplaySettings(ref dm, 0);
        }


        #endregion

    }
}

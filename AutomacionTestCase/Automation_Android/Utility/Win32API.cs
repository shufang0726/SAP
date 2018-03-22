using System;
using System.Text;
using System.Runtime.InteropServices;
using Accessibility;
namespace Automation_Android
{
    /// <summary>
    /// All Win32APIs used for capturing screenshots and get accessibility information
    /// </summary>
    internal static class Win32API
    {
        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetProcessDPIAware();

        internal static bool IsBitSet(int flags, int bit)
        {
            return (flags & bit) == bit;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public RECT(RECT rcSrc)
            {
                this.left = rcSrc.left;
                this.top = rcSrc.top;
                this.right = rcSrc.right;
                this.bottom = rcSrc.bottom;
            }

            public bool IsEmpty
            {
                get
                {
                    return left >= right || top >= bottom;
                }
            }
            static public RECT Empty
            {
                get
                {
                    return new RECT(0, 0, 0, 0);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int x;
            public int y;

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        internal const int GW_HWNDNEXT = 2;
        internal const int GW_CHILD = 5;
        internal const int WM_CLOSE = 0x0010;

        public const uint WS_VISIBLE = 0x10000000;
        internal const int CHILD_SELF = 0x0;

        internal const int WM_GETTEXT = 0x0D;
        internal const int WM_GETTEXTLENGTH = 0x0E;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int Param, System.Text.StringBuilder text);

        // GetWindowLong()
        internal const int GWL_EXSTYLE = (-20);
        internal const int GWL_STYLE = (-16);

        internal const int WS_EX_TOOLWINDOW = 0x00000080;

        internal static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        internal const int SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);


        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hwndAfter, int x, int y, int width, int height, int flags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetCursorPos([In, Out] ref POINT pt);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        internal static extern bool UpdateWindow(IntPtr hwnd);

        public const uint SPI_GETMESSAGEDURATION = 0x2016;
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SystemParametersInfo(uint uiAction, uint uiParam, out int pvParam, UInt32 fWinIni);

        internal const int SW_HIDE = 0;
        internal const int SW_SHOWNORMAL = 1;
        internal const int SW_NORMAL = 1;
        internal const int SW_SHOWMINIMIZED = 2;
        internal const int SW_SHOWMAXIMIZED = 3;
        internal const int SW_MAXIMIZE = 3;
        internal const int SW_SHOWNOACTIVATE = 4;
        internal const int SW_SHOW = 5;
        internal const int SW_MINIMIZE = 6;
        internal const int SW_SHOWMINNOACTIVE = 7;
        internal const int SW_SHOWNA = 8;
        internal const int SW_RESTORE = 9;

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public RECT rcNormalPosition;
        }

        [Flags]
        internal enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0000,
            SMTO_BLOCK = 0x0001,
            SMTO_ABORTIFHUNG = 0x0002,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x0008,
            SMTO_ERRORONEXIT = 0x0020
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetWindowPlacement(IntPtr hwnd, ref WINDOWPLACEMENT place);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetWindowPlacement(IntPtr hwnd, ref WINDOWPLACEMENT place);
        [DllImport("user32.dll")]
        internal static extern IntPtr FindWindow(string className, string windowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindowEx(IntPtr parent, IntPtr next, string sClassName, string sWindowTitle);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindowRole(IntPtr parent, IntPtr next, string sClassName, string sWindowTitle);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr WindowFromPoint(POINT pt);

        internal const int GA_PARENT = 1;
        internal const int GA_ROOT = 2;
        internal const int GA_ROOTOWNER = 3;
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr GetAncestor(IntPtr hwnd, int gaFlags);

        #region Hotkeys
        [DllImport("user32", SetLastError = true)]
        internal static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);
        [DllImport("user32", SetLastError = true)]
        internal static extern int UnregisterHotKey(IntPtr hwnd, int id);

        internal const int MOD_ALT = 1;
        internal const int MOD_CONTROL = 2;
        internal const int MOD_SHIFT = 4;
        internal const int MOD_WIN = 8;
        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hwnd, ref RECT rc);

        internal const int SRCCOPY = 0x00CC0020;

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr GetDC(IntPtr hwnd);

        const int LOGPIXELSX = 88;
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        // Simplified version of ReleaseDC
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetClassName(IntPtr hWnd, StringBuilder classname, int nMax);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMax);


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out UIntPtr result);

        internal const int MAX_PATH = 260;
        internal static string GetWindowText(IntPtr hWnd)
        {
            System.Text.StringBuilder windowName = new System.Text.StringBuilder(MAX_PATH + 1);
            GetWindowText(hWnd, windowName, MAX_PATH);
            return windowName.ToString();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool IsWindow(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint dwProcessId);

        [DllImport("kernel32.dll")]
        internal static extern int GetCurrentProcessId();

        #region Windows
        // Enum windows delegate
        internal delegate bool WinEnumProc(IntPtr hwnd, ref IntPtr lParam);

        // Enum windows delegate
        internal delegate bool EnumDesktopWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnumDesktopWindows(IntPtr hWndParent, EnumDesktopWindowsProc enumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hWndParent, WinEnumProc enumFunc, ref IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnumWindows(WinEnumProc enumFunc, ref IntPtr lParam);
        #endregion

        #region Accessibility
        internal const int SELFLAG_NONE = 0x00000000;
        internal const int SELFLAG_TAKEFOCUS = 0x00000001;
        internal const int SELFLAG_TAKESELECTION = 0x00000002;
        internal const int SELFLAG_EXTENDSELECTION = 0x00000004;
        internal const int SELFLAG_ADDSELECTION = 0x00000008;
        internal const int SELFLAG_REMOVESELECTION = 0x00000010;
        internal const int SELFLAG_VALID = 0x0000001F;
        #endregion

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct GUITHREADINFO
        {
            public int cbSize;
            public int dwFlags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public RECT rc;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetGUIThreadInfo(int idThread, ref GUITHREADINFO guiThreadInfo);

        [DllImport("oleacc.dll")]
        internal static extern int AccessibleObjectFromPoint(POINT pt, [In, Out] ref IAccessible ppvObject, [In, Out] ref object varChild);

        [DllImport("oleacc.dll")]
        internal static extern int AccessibleObjectFromWindow(IntPtr hwnd, uint id, ref Guid iid,
            [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object ppvObject);

        [DllImport("oleacc.dll")]
        internal static extern int WindowFromAccessibleObject(IAccessible acc, ref IntPtr hwnd);

        [DllImport("user32.dll")]
        internal static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        // Overload for IAccessibles, much user friendly.
        internal static int AccessibleObjectFromWindow(IntPtr hwnd, uint idObject, ref IAccessible acc)
        {
            Guid IID_IUnknown = new Guid(0x00000000, 0x0000, 0x0000, 0xc0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46);

            object obj = null;
            int hr = AccessibleObjectFromWindow(hwnd, idObject, ref IID_IUnknown, ref obj);

            acc = (IAccessible)obj;
            return hr;
        }

        [DllImport("oleacc.dll")]
        internal static extern int AccessibleChildren(Accessibility.IAccessible paccContainer, int iChildStart, int cChildren, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2), In, Out] object[] rgvarChildren, out int pcObtained);

        [DllImport("oleacc.dll")]
        internal static extern int AccessibleObjectFromEvent(
            IntPtr hwnd,
            int idObject,
            int idChild,
            [In, Out] ref IAccessible ppvObject,
            [In, Out] ref Object varChild);

        internal const int ChildIdSelf = 0;
        internal const int StateSystemFocused = 0x00000004;
        internal const int StateSystemUnavailable = 0x00000001;
        internal const int ObjIdCaret = -8;
        internal const int ObjIdClient = -4;
        internal const int ObjIdWindow = 0;

        [DllImport("oleacc.dll", CharSet = CharSet.Unicode)]
        internal static extern int GetRoleText(int dwRole, StringBuilder lpszRole, uint cchRoleMax);

        #region COM
        internal static Guid IID_IUnknown = new Guid(0x00000000, 0x0000, 0x0000, 0xc0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46);
        internal static Guid IID_IAccessible = new Guid(0x618736e0, 0x3c3d, 0x11cf, 0x81, 0x0c, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        internal const int S_OK = 0;
        #endregion

        #region Accessibility


        internal const int ROLE_SYSTEM_TITLEBAR = 0x1;
        internal const int ROLE_SYSTEM_MENUBAR = 0x2;
        internal const int ROLE_SYSTEM_SCROLLBAR = 0x3;
        internal const int ROLE_SYSTEM_GRIP = 0x4;
        internal const int ROLE_SYSTEM_SOUND = 0x5;
        internal const int ROLE_SYSTEM_CURSOR = 0x6;
        internal const int ROLE_SYSTEM_CARET = 0x7;
        internal const int ROLE_SYSTEM_ALERT = 0x8;
        internal const int ROLE_SYSTEM_WINDOW = 0x9;
        internal const int ROLE_SYSTEM_CLIENT = 0xa;
        internal const int ROLE_SYSTEM_MENUPOPUP = 0xb;
        internal const int ROLE_SYSTEM_MENUITEM = 0xc;
        internal const int ROLE_SYSTEM_TOOLTIP = 0xd;
        internal const int ROLE_SYSTEM_APPLICATION = 0xe;
        internal const int ROLE_SYSTEM_DOCUMENT = 0xf;
        internal const int ROLE_SYSTEM_PANE = 0x10;
        internal const int ROLE_SYSTEM_CHART = 0x11;
        internal const int ROLE_SYSTEM_DIALOG = 0x12;
        internal const int ROLE_SYSTEM_BORDER = 0x13;
        internal const int ROLE_SYSTEM_GROUPING = 0x14;
        internal const int ROLE_SYSTEM_SEPARATOR = 0x15;
        internal const int ROLE_SYSTEM_TOOLBAR = 0x16;
        internal const int ROLE_SYSTEM_STATUSBAR = 0x17;
        internal const int ROLE_SYSTEM_TABLE = 0x18;
        internal const int ROLE_SYSTEM_COLUMNHEADER = 0x19;
        internal const int ROLE_SYSTEM_ROWHEADER = 0x1a;
        internal const int ROLE_SYSTEM_COLUMN = 0x1b;
        internal const int ROLE_SYSTEM_ROW = 0x1c;
        internal const int ROLE_SYSTEM_CELL = 0x1d;
        internal const int ROLE_SYSTEM_LINK = 0x1e;
        internal const int ROLE_SYSTEM_HELPBALLOON = 0x1f;
        internal const int ROLE_SYSTEM_CHARACTER = 0x20;
        internal const int ROLE_SYSTEM_LIST = 0x21;
        internal const int ROLE_SYSTEM_LISTITEM = 0x22;
        internal const int ROLE_SYSTEM_OUTLINE = 0x23;
        internal const int ROLE_SYSTEM_OUTLINEITEM = 0x24;
        internal const int ROLE_SYSTEM_PAGETAB = 0x25;
        internal const int ROLE_SYSTEM_PROPERTYPAGE = 0x26;
        internal const int ROLE_SYSTEM_INDICATOR = 0x27;
        internal const int ROLE_SYSTEM_GRAPHIC = 0x28;
        internal const int ROLE_SYSTEM_STATICTEXT = 0x29;
        internal const int ROLE_SYSTEM_TEXT = 0x2a;
        internal const int ROLE_SYSTEM_PUSHBUTTON = 0x2b;
        internal const int ROLE_SYSTEM_CHECKBUTTON = 0x2c;
        internal const int ROLE_SYSTEM_RADIOBUTTON = 0x2d;
        internal const int ROLE_SYSTEM_COMBOBOX = 0x2e;
        internal const int ROLE_SYSTEM_DROPLIST = 0x2f;
        internal const int ROLE_SYSTEM_PROGRESSBAR = 0x30;
        internal const int ROLE_SYSTEM_DIAL = 0x31;
        internal const int ROLE_SYSTEM_HOTKEYFIELD = 0x32;
        internal const int ROLE_SYSTEM_SLIDER = 0x33;
        internal const int ROLE_SYSTEM_SPINBUTTON = 0x34;
        internal const int ROLE_SYSTEM_DIAGRAM = 0x35;
        internal const int ROLE_SYSTEM_ANIMATION = 0x36;
        internal const int ROLE_SYSTEM_EQUATION = 0x37;
        internal const int ROLE_SYSTEM_BUTTONDROPDOWN = 0x38;
        internal const int ROLE_SYSTEM_BUTTONMENU = 0x39;
        internal const int ROLE_SYSTEM_BUTTONDROPDOWNGRID = 0x3a;
        internal const int ROLE_SYSTEM_WHITESPACE = 0x3b;
        internal const int ROLE_SYSTEM_PAGETABLIST = 0x3c;
        internal const int ROLE_SYSTEM_CLOCK = 0x3d;
        internal const int ROLE_SYSTEM_SPLITBUTTON = 0x3e;
        internal const int ROLE_SYSTEM_IPADDRESS = 0x3f;
        internal const int ROLE_SYSTEM_OUTLINEBUTTON = 0x40;

        internal const int STATE_SYSTEM_UNAVAILABLE = 0x00000001;  // Disabled
        internal const int STATE_SYSTEM_SELECTED = 0x00000002;
        internal const int STATE_SYSTEM_FOCUSED = 0x00000004;
        internal const int STATE_SYSTEM_PRESSED = 0x00000008;
        internal const int STATE_SYSTEM_CHECKED = 0x00000010;
        internal const int STATE_SYSTEM_MIXED = 0x00000020;  // 3-state checkbox or toolbar button
        internal const int STATE_SYSTEM_INDETERMINATE = STATE_SYSTEM_MIXED;
        internal const int STATE_SYSTEM_READONLY = 0x00000040;
        internal const int STATE_SYSTEM_HOTTRACKED = 0x00000080;
        internal const int STATE_SYSTEM_DEFAULT = 0x00000100;
        internal const int STATE_SYSTEM_EXPANDED = 0x00000200;
        internal const int STATE_SYSTEM_COLLAPSED = 0x00000400;
        internal const int STATE_SYSTEM_BUSY = 0x00000800;
        internal const int STATE_SYSTEM_FLOATING = 0x00001000;  // Children "owned" not "contained" by parent
        internal const int STATE_SYSTEM_MARQUEED = 0x00002000;
        internal const int STATE_SYSTEM_ANIMATED = 0x00004000;
        internal const int STATE_SYSTEM_INVISIBLE = 0x00008000;
        internal const int STATE_SYSTEM_OFFSCREEN = 0x00010000;
        internal const int STATE_SYSTEM_SIZEABLE = 0x00020000;
        internal const int STATE_SYSTEM_MOVEABLE = 0x00040000;
        internal const int STATE_SYSTEM_SELFVOICING = 0x00080000;
        internal const int STATE_SYSTEM_FOCUSABLE = 0x00100000;
        internal const int STATE_SYSTEM_SELECTABLE = 0x00200000;
        internal const int STATE_SYSTEM_LINKED = 0x00400000;
        internal const int STATE_SYSTEM_TRAVERSED = 0x00800000;
        internal const int STATE_SYSTEM_MULTISELECTABLE = 0x01000000;  // Supports multiple selection
        internal const int STATE_SYSTEM_EXTSELECTABLE = 0x02000000;  // Supports extended selection
        internal const int STATE_SYSTEM_ALERT_LOW = 0x04000000;  // This information is of low priority
        internal const int STATE_SYSTEM_ALERT_MEDIUM = 0x08000000;  // This information is of medium priority
        internal const int STATE_SYSTEM_ALERT_HIGH = 0x10000000;  // This information is of high priority
        internal const int STATE_SYSTEM_PROTECTED = 0x20000000;  // access to this is restricted
        internal const int STATE_SYSTEM_HAS_POPUP = 0x40000000;
        internal const int STATE_SYSTEM_VALID = 0x3FFFFFFF;
        #endregion


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool GetUpdateRect(IntPtr hWnd, ref RECT rect, bool bErase);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        internal static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("oleacc.dll", PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        internal static extern object ObjectFromLresult(UIntPtr lResult, [MarshalAs(UnmanagedType.LPStruct)] Guid refiid, IntPtr wParam);
    }
}

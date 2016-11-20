// ***********************************************************
// 封装系统中使用到的一些Windows API
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Windy.WebMVC.Web2.Utility
{
    public struct NativeConstants
    {
        public const int SW_SHOWNORMAL = 0x0001;
        public const int SW_SHOWNOACTIVATE = 0x0004;
        public const int SW_SHOWNA = 0x0008;
        public const int SW_RESTORE = 0x0009;

        public const uint MA_ACTIVATE = 1;
        public const uint MA_ACTIVATEANDEAT = 2;
        public const uint MA_NOACTIVATE = 3;
        public const uint MA_NOACTIVATEANDEAT = 4;

        public const int ERROR_SUCCESS = 0;
        public const int ERROR_ALREADY_EXISTS = 183;
        public const int ERROR_CANCELLED = 1223;
        public const int ERROR_IO_PENDING = 0x3e5;
        public const int ERROR_NO_MORE_ITEMS = 259;
        public const int ERROR_TIMEOUT = 1460;

        public const uint SECTION_QUERY = 0x0001;
        public const uint SECTION_MAP_WRITE = 0x0002;
        public const uint SECTION_MAP_READ = 0x0004;
        public const uint SECTION_MAP_EXECUTE_EXPLICIT = 0x0020;

        public const uint SEC_IMAGE = 0x1000000;
        public const uint SEC_RESERVE = 0x4000000;
        public const uint SEC_COMMIT = 0x8000000;
        public const uint SEC_NOCACHE = 0x10000000;

        public const uint PAGE_NOACCESS = 0x01;
        public const uint PAGE_READONLY = 0x02;
        public const uint PAGE_READWRITE = 0x04;
        public const uint PAGE_WRITECOPY = 0x08;
        public const uint PAGE_EXECUTE = 0x10;
        public const uint PAGE_EXECUTE_READ = 0x20;
        public const uint PAGE_EXECUTE_READWRITE = 0x40;
        public const uint PAGE_EXECUTE_WRITECOPY = 0x80;
        public const uint PAGE_GUARD = 0x100;
        public const uint PAGE_NOCACHE = 0x200;
        public const uint PAGE_WRITECOMBINE = 0x400;

        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        public const uint FILE_MAP_COPY = SECTION_QUERY;
        public const uint FILE_MAP_WRITE = SECTION_MAP_WRITE;
        public const uint FILE_MAP_READ = SECTION_MAP_READ;
        public const uint FILE_MAP_EXECUTE = SECTION_MAP_EXECUTE_EXPLICIT;

        //转换字符为全角的标志量
        public const int LCMAP_FULLWIDTH = 0x800000;
        //转换字符为半角的标志量
        public const int LCMAP_HALFWIDTH = 0x400000;

        public const int SWP_DRAWFRAME = 0x20;
        public const int SWP_NOMOVE = 0x2;
        public const int SWP_NOSIZE = 0x1;
        public const int SWP_NOZORDER = 0x4;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_SHOWWINDOW = 0x0040;

        public const int GWL_STYLE = -16;
        public const int WS_CAPTION = 0xC00000;

        //窗体标题栏默认上下文菜单控制
        public const int MF_REMOVE = 0x1000;
        public const int SC_RESTORE = 0xF120;   //还原
        public const int SC_MOVE = 0xF010;      //移动
        public const int SC_SIZE = 0xF000;      //大小
        public const int SC_MINIMIZE = 0xF020;  //最小化
        public const int SC_MAXIMIZE = 0xF030;  //最大化
        public const int SC_CLOSE = 0xF060;     //关闭 

        public const int WM_USER = 0x400;
        public const int WM_COPYDATA = 0x004A;
        public const int WM_CLOSE = 0x10;
        public const int WM_TIMER = 0x0113;
        public const int WM_PAINT = 0xF;

        public const int WM_DRAWCLIPBOARD = 0x308;
        public const int WM_CHANGECBCHAIN = 0x030D;

        public const int WM_CHAR = 0x102;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_SYSKEYDOWN = 0x104;
        public const int WM_SYSKEYUP = 0x105;

        public const int WM_MOUSEACTIVATE = 0x21;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_MBUTTONUP = 0x208;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDBLCLK = 0x206;
        public const int WM_MBUTTONDBLCLK = 0x209;
        public const int WM_MOUSEWHEEL = 0x20A;
        public const int WM_HSCROLL = 0x114;
        public const int WM_VSCROLL = 0x115;

        public const int WM_SYSCOMMAND = 0x112;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCRBUTTONDOWN = 0xA4;
        public const int WM_NCMBUTTONDOWN = 0xA7;

        public const int IME_CMODE_FULLSHAPE = 0x8;
        public const int IME_CHOTKEY_SHAPE_TOGGLE = 0x11;
    }

    public struct NativeStructs
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MESSAGE_DATA
        {
            public IntPtr Handle;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string Data;

            public MESSAGE_DATA(IntPtr handle, string data)
            {
                Handle = handle;
                Data = data;
            }
        }
    }

    public struct NativeMethods
    {
        public struct User32
        {
            [DllImport("user32.dll")]
            public static extern IntPtr GetDC(int hwnd);

            [DllImport("user32.dll")]
            public static extern IntPtr GetFocus();

            [DllImport("user32.dll")]
            public static extern int ReleaseDC(int hwnd, IntPtr hdc);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.DLL")]
            public static extern IntPtr GetSystemMenu(IntPtr hWnd, int bRevert);

            [DllImport("user32.DLL")]
            public static extern bool RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

            [DllImport("user32.dll", SetLastError = false)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsIconic(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsWindow(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

            [DllImport("user32.dll")]
            public static extern IntPtr GetParent(IntPtr hWndChild);

            [DllImport("user32.dll")]
            public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            [DllImport("user32.dll", EntryPoint = "MoveWindow")]
            public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SetFocus(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int SetActiveWindow(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            public static extern bool GetCaretPos(ref NativeStructs.POINT point);

            [DllImport("user32.dll")]
            public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);
        }

        public struct Kernel32
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern void SetLastError(int dwErrCode);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr CreateFileMappingW(
                IntPtr hFile,
                IntPtr lpFileMappingAttributes,
                uint flProtect,
                uint dwMaximumSizeHigh,
                uint dwMaximumSizeLow,
                [MarshalAs(UnmanagedType.LPTStr)] string lpName);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr MapViewOfFile(
                IntPtr hFileMappingObject,
                uint dwDesiredAccess,
                uint dwFileOffsetHigh,
                uint dwFileOffsetLow,
                UIntPtr dwNumberOfBytesToMap);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CloseHandle(IntPtr hObject);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr LoadLibrary(string lpLibFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern void FreeLibrary(IntPtr hLibFile);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

            /// <summary>
            /// 锁定内存中指定的内存块,并返回一个地址值,令其指向内存块的起始处。
            /// 除非用 GlobalUnlock 函数将内存块解锁,否则地址会一直保持有效。
            /// </summary>
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
            public static extern IntPtr GlobalLock(IntPtr handle);

            /// <summary>
            /// 将使用GlobalLock锁定的内存块解锁
            /// </summary>
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
            public static extern bool GlobalUnlock(IntPtr handle);
        }

        public struct Ime32
        {
            [DllImport("imm32.dll ")]
            public static extern IntPtr ImmGetContext(IntPtr hwnd);

            [DllImport("imm32.dll ")]
            public static extern bool ImmGetOpenStatus(IntPtr himc);

            [DllImport("imm32.dll ")]
            public static extern bool ImmSetOpenStatus(IntPtr himc, bool b);

            [DllImport("imm32.dll ")]
            public static extern bool ImmGetConversionStatus(IntPtr himc, ref int lpdw, ref int lpdw2);

            [DllImport("imm32.dll ")]
            public static extern int ImmSimulateHotKey(IntPtr hwnd, int lngHotkey);
        }

        public struct WinSpool
        {
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct StructPrinterDefaults
            {
                [MarshalAs(UnmanagedType.LPTStr)]
                public String pDatatype;
                public IntPtr pDevMode;
                [MarshalAs(UnmanagedType.I4)]
                public int DesiredAccess;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct PRINTER_INFO
            {
                [MarshalAs(UnmanagedType.LPTStr)]
                public string pServerName;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string pPrinterName;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string pShareName;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string pPortName;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string pDriverName;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string pComment;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string pLocation;
                public IntPtr pDevMode;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string pSepFile;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string pPrintProcessor;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string pDatatype;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string pParameters;
                public IntPtr pSecurityDescriptor;
                public uint Attributes;
                public uint Priority;
                public uint DefaultPriority;
                public uint StartTime;
                public uint UntilTime;
                public uint Status;
                public uint cJobs;
                public uint AveragePPM;
            }

            [DllImport("winspool.drv", SetLastError = true)]
            public static extern bool SetDefaultPrinter(string szPrinterName);

            [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool OpenPrinter(string printer, out IntPtr handle, ref StructPrinterDefaults pDefault);

            [DllImport("winspool.drv")]
            public static extern bool ClosePrinter(IntPtr handle);

            [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool GetPrinter(IntPtr handle, Int32 level, IntPtr buffer, Int32 size, out Int32 sizeNeeded);
        }

        public struct Advapi32
        {
            [DllImport("Advapi32.dll")]
            public static extern IntPtr OpenEventLog(string lpUNCServerName, string lpSourceName);

            [DllImport("Advapi32.dll")]
            public static extern bool CloseEventLog(IntPtr hEventLog);

            [DllImport("Advapi32.dll")]
            public static extern bool BackupEventLog(IntPtr hEventLog, string lpBackupFileName);
        }

        public struct WinInet
        {
            public const int INTERNET_FLAG_PASSIVE = 0x8000000; //被动模式
            public const int INTERNET_FLAG_PORT = 0x0;          //主动模式

            public const uint INTERNET_FLAG_RELOAD = 0x80000000;         //
            public const uint INTERNET_FLAG_KEEP_CONNECTION = 0x400000;  //
            public const uint INTERNET_FLAG_MULTIPART = 0x200000;        //

            public const int INTERNET_OPEN_TYPE_PRECONFIG = 0;
            public const int INTERNET_OPEN_TYPE_DIRECT = 1;
            public const int INTERNET_OPEN_TYPE_PROXY = 3;

            public const int INTERNET_SERVICE_FTP = 1;
            public const int INTERNET_SERVICE_GOPHER = 2;
            public const int INTERNET_SERVICE_HTTP = 3;

            public const int FTP_TRANSFER_TYPE_ASCII = 0x1;
            public const int FTP_TRANSFER_TYPE_BINARY = 0x2;

            public const int FILE_ATTRIBUTE_READONLY = 0x1;
            public const int FILE_ATTRIBUTE_HIDDEN = 0x2;
            public const int FILE_ATTRIBUTE_SYSTEM = 0x4;
            public const int FILE_ATTRIBUTE_DIRECTORY = 0x10;
            public const int FILE_ATTRIBUTE_ARCHIVE = 0x20;
            public const int FILE_ATTRIBUTE_NORMAL = 0x80;
            public const int FILE_ATTRIBUTE_TEMPORARY = 0x100;
            public const int FILE_ATTRIBUTE_COMPRESSED = 0x800;

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class WIN32_FIND_DATA
            {
                public UInt32 dwFileAttributes = 0;
                public FILETIME ftCreationTime;
                public FILETIME ftLastAccessTime;
                public FILETIME ftLastWriteTime;
                public UInt32 nFileSizeHigh = 0;
                public UInt32 nFileSizeLow = 0;
                public UInt32 dwReserved0 = 0;
                public UInt32 dwReserved1 = 0;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
                public string cFileName = null;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
                public string cAlternateFileName = null;
            };
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class FILETIME
            {
                public int dwLowDateTime = 0;
                public int dwHighDateTime = 0;
            };

            // 连接和初始化
            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr InternetOpen(string strAppName, int nAccessType, string strProxy, string strProxyBypass, int nFlags);

            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr InternetConnect(IntPtr hInet, string strServer, int nPort, string strUser, string strPassword
                , int nService, int nFlags, int nContext);

            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool InternetCloseHandle(IntPtr hSession);

            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool InternetGetConnectedState(ref int ulFlags, int ulReserved);

            // Ftp文件操作命令
            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr FtpFindFirstFile(IntPtr hSession, string strPath, [In, Out] WIN32_FIND_DATA dirData, int nFlags, int nContext);

            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool InternetFindNextFile(IntPtr hFind, [In, Out] WIN32_FIND_DATA dirData);

            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool FtpGetFile(IntPtr hFtpSession, string lpszRemoteFile, string lpszNewFile
                , bool fFailIfExists, int dwFlagsAndAttributes, int dwFlags, int dwContext);

            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool FtpPutFile(IntPtr hFtpSession, string lpszLocalFile, string lpszRemoteFile
                , int dwFlags, int dwContext);

            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool FtpDeleteFile(IntPtr hFtpSession, string lpszFileName);

            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool FtpRenameFile(IntPtr hFtpSession, string lpszExisting, string lpszNew);

            // Ftp目录操作命令
            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool FtpGetCurrentDirectory(IntPtr hFtpSession, [In, Out] string lpszCurrentDirectory, ref int lpdwCurrentDirectory);

            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool FtpSetCurrentDirectory(IntPtr hFtpSession, string lpszCurrentDirectory);

            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool FtpCreateDirectory(IntPtr hFtpSession, string lpszDirectory);

            [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool FtpRemoveDirectory(IntPtr hFtpSession, string lpszDirectory);
        }

        public struct Gdi32
        {
            /// <summary>
            /// 打印API使用的文档信息类实现
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class DOCINFO
            {
                public int cbSize;
                public string lpszDocName;
                public string lpszOutput;
                public string lpszDatatype;
                public int fwType;
                public DOCINFO()
                {
                    this.cbSize = 20;
                }
            }

            /// <summary>
            /// 为专门设备创建设备场景
            /// </summary>
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateDC(string lpDriverName, string lpDeviceName, int lpOutput, int lpInitData);

            /// <summary>
            /// 删除专用设备场景或信息场景，释放所有相关窗口资源
            /// </summary>
            [DllImport("gdi32.dll")]
            public static extern int DeleteDC(IntPtr hdc);

            /// <summary>
            /// 根据提供的DEVMODE结构，对一个设备场景进行重设。
            /// </summary>
            [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr ResetDC(IntPtr hDC, IntPtr lpDevMode);

            /// <summary>
            /// 开始一个打印作业
            /// 如执行成功，返回文档的作业编号，常数SP_ERROR失败。会设置GetLastError
            /// </summary>
            [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int StartDoc(IntPtr hDC, DOCINFO lpDocInfo);

            /// <summary>
            /// 打印一个新页前要先调用这个函数
            /// 非零表示成功，零表示失败
            /// </summary>
            [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
            public static extern int StartPage(IntPtr hDC);

            /// <summary>
            /// 取消一份文档的打印。自上次调用StartDoc函数以来的所有输出都会被取消。
            /// 如对打印机进行了配置，令其在正式打印文档之前先在打印缓冲区内对文档进行排队，那么文档的任何一部分都不会打印；
            /// 否则，就可能出现文档打印到一半被取消的情况
            /// </summary>
            [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
            public static extern int AbortDoc(IntPtr hDC);

            /// <summary>
            /// 结束一份文档的打印
            /// </summary>
            [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
            public static extern int EndDoc(IntPtr hDC);

            /// <summary>
            /// 结束打印一个新页前要先调用这个函数
            /// </summary>
            [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
            public static extern int EndPage(IntPtr hDC);

            /// <summary>
            /// 根据指定设备场景代表的设备的功能返回信息
            /// </summary>
            [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
            public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);
        }
    }
}

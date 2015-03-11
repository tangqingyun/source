using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Basement.Framework.IExplorer
{
    public class IECache
    {
        //方法二 （静默清除）
        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowCommands nShowCmd);
        public static void IECacheClear()
        { 
            //清除IE临时文件
            ShellExecute(IntPtr.Zero, "open", "rundll32.exe", " InetCpl.cpl,ClearMyTracksByProcess 255", "", ShowCommands.SW_HIDE);
        }

    }

    public enum ShowCommands : int
    {
        SW_HIDE = 0,
        SW_SHOWNORMAL = 1,
        SW_NORMAL = 1,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMAXIMIZED = 3,
        SW_MAXIMIZE = 3,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOW = 5,
        SW_MINIMIZE = 6,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_RESTORE = 9,
        SW_SHOWDEFAULT = 10,
        SW_FORCEMINIMIZE = 11,
        SW_MAX = 11
    }
}

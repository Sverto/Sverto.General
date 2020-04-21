using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sverto.General.OS
{
    public static class ProcessHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        private static extern bool ShowWindow(IntPtr hWnd, WindowState nCmdShow);

        #region "Enums & Structures"
        public enum WindowState
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
        #endregion

        #region "Process & Windowstate"
        public static Process StartProcess(string filePath, string parameters, string workingDirectory = null, ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            Process p = new Process();
            ProcessStartInfo pi = new ProcessStartInfo();
            try
            {
                // Set window state
                pi.WindowStyle = windowStyle;
                pi.UseShellExecute = false;
                // Set working dir
                if (!(workingDirectory == null))
                {
                    pi.WorkingDirectory = workingDirectory;
                }
                pi.Arguments = parameters;
                pi.FileName = filePath;
                p.StartInfo = pi;
                if (p.Start())
                    return p;
            }
            catch
            {
            }
            return null;
        }

        public static void WindowSetState(Process p, WindowState windowState)
        {
            ShowWindow(p.MainWindowHandle, windowState);
        }

        public static Process[] FindProcess(string processName)
        {
            // Find the process by processname
            return Process.GetProcessesByName(processName);
        }

        public static Process FindProcess(string processName, string windowTitle)
        {
            // Find the process by processname and windowtitle
            Process[] pList = null;
            pList = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(processName));
            //
            if (pList.Any())
            {
                // is process with specified window title is running
                foreach (Process p in pList)
                {
                    if (p.MainWindowTitle.StartsWith(windowTitle))
                        return p;
                }
            }
            return null;
        }
        #endregion

    }
}

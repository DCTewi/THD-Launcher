using System;
using System.Runtime.InteropServices;
using System.Text;

public static class LibLoader
{
    [DllImport("thd-core")]
    public static extern int InstallScript();

    [DllImport("thd-core")]
    public static extern int FirstGetDota2WorkingDir();

    [DllImport("thd-core")]
    public static extern int FileLogInit(string FileName);

    [DllImport("thd-core")]
    public static extern int FileLog(string info);

    [DllImport("thd-core")]
    public static extern int LaunchServer(int sv_id, int wait, int enable_bot);

    [DllImport("thd-core")]
    public static extern int Connect(string ip, int wait);

    [DllImport("thd-core")]
    public static extern int GetScriptVersion(string path);

    [DllImport("thd-core")]
    public static extern int SendToVConsole(string ip, int port, string msg);

    [DllImport("thd-core", EntryPoint = "LaunchProxy")]
    public static extern int LaunchProxy([Out, MarshalAs(UnmanagedType.LPStr)]StringBuilder addr_sv, StringBuilder addr_ob, int wait);

    [DllImport("user32")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32")]
    public static extern IntPtr GetForegroundWindow();
}

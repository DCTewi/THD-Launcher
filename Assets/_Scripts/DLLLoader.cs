using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DLLLoader : MonoBehaviour
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
    public static extern int LaunchServer(long sv_id, long wait);

    [DllImport("thd-core")]
    public static extern int Connect(string ip, long wait);

    [DllImport("thd-core")]
    public static extern int GetScriptVersion(string path);
}

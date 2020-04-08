using Microsoft.Win32;
using System;
using System.IO;
using UnityEngine;

public class DotaSeeker : MonoBehaviour
{
    public const string ININame = "dotapath.ini";
    public static event Action DotaSeekerInited;

    public static DotaSeeker Instance;
    private static string dotapath = "";
    public static string DotaPath
    {
        get
        {
            if (string.IsNullOrEmpty(dotapath))
            {
                Instance.LoadINI();
            }
            return dotapath;
        }
        set
        {
            dotapath = value;
            Instance.CreateINI(false);
        }
    }

    [ContextMenu("Create .ini")]
    public void CreateINI(bool isFirstrun)
    {
        string path = Path.Combine(Application.dataPath, "..", ININame);

        if (isFirstrun)
        {
            SeekDotaRegistry();
        }

        File.WriteAllText(path, dotapath);

        Debug.Log("dotapath file created.");
    }

    [ContextMenu("Load .ini")]
    public void LoadINI()
    {
        string path = Path.Combine(Application.dataPath, "..", ININame);
        if (!File.Exists(path))
        {
            Debug.LogWarning("dotapath file not found, try to create one.");
            CreateINI(true);
        }
        else
        {
            dotapath = File.ReadAllText(path);
            if (string.IsNullOrEmpty(dotapath))
            {
                Debug.LogError("dotapath is invalid.");
                File.Delete(path);
                SeekDotaRegistry();
            }
            else if (!File.Exists(dotapath))
            {
                Debug.LogError("init dotapath file invalid, try to create one.");
                CreateINI(true);
                dotapath = File.ReadAllText(path);
            }
            if (dotapath.EndsWith("\n"))
            {
                dotapath = dotapath.Substring(0, dotapath.Length - 1);
            }
            Debug.Log("dotapath file loaded.");
        }
    }

    [ContextMenu("Find Dota")]
    public void SeekDotaRegistry()
    {
        RegistryKey regDota = Registry.ClassesRoot.OpenSubKey("dota2")?.OpenSubKey("Shell")?.OpenSubKey("Open")?.OpenSubKey("Command");

        string path = "";

        if (regDota == null)
        {
            Debug.LogError("can't find dota2 in registry");
        }
        else
        {
            path = regDota.GetValue("").ToString();
            path = path.Substring(1, path.IndexOf("\"", 2) - 1);
        }
        
        dotapath = path;

        CreateINI(false);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        LoadINI();

        DotaSeekerInited?.Invoke();
    }
}

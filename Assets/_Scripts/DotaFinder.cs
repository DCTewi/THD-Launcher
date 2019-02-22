using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Win32;

public class DotaFinder : MonoBehaviour
{
    public static string DotaPath = "";
    public static string IniName = "dotapath.ini";

    public OptionManager option;

    [ContextMenu("Create ini")]
    public void CreateINI(int k)
    {
        string path = Application.dataPath + "/../" + IniName;

        if (k == 1)
        {
            FindDotaReg();
        }
        
        File.WriteAllText(path, DotaPath);
        option.FreshPathShow();
        Debug.Log("ini File Created.");
    }

    [ContextMenu("Load ini")]
    public void LoadINI()
    {
        string path = Application.dataPath + "/../" + IniName;
        if (!File.Exists(path))
        {
            CreateINI(1);
            Debug.Log("ini File Not Found and Created a new one.");
        }
        else
        {
            DotaPath = File.ReadAllText(path);
            if (DotaPath.EndsWith("\n"))
            {
                DotaPath = DotaPath.Substring(0, DotaPath.Length - 1);
            }
            Debug.Log("ini File Loaded.");
        }
    }

    [ContextMenu("FindDota")]
    public void FindDotaReg()
    {
        RegistryKey regDota = Registry.ClassesRoot.OpenSubKey("dota2").OpenSubKey("Shell").OpenSubKey("Open").OpenSubKey("Command");
        string path = regDota.GetValue("").ToString();
        path = path.Substring(1, path.IndexOf('\"', 2) - 1);
        DotaPath = path;
    }

    private void Awake()
    {
        DLLLoader.FileLogInit(Application.dataPath + "/launcher.log");
        LoadINI();
    }
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Options
{
    public enum Keys : short
    {
        UseTest = 0,
        UseBot,
        ProxyObserve,
        WaitInGame,
        UpdateAddress,
        AutoUpdate,
    }

    public const string INIPath = "config.ini";
    public const string INISection = "thd-launcher";

    private static Dictionary<Keys, object> configs = new Dictionary<Keys, object>
    {
        { Keys.UseTest, false },
        { Keys.UseBot, true },
        { Keys.ProxyObserve, false },
        { Keys.WaitInGame, false },
        { Keys.AutoUpdate, true },
        { Keys.UpdateAddress, "http://39.105.200.223:81/thd2/bots/script.pak"},
    };

    public static T Get<T>(Keys key)
    {
        return (T)configs[key];
    }

    public static void Read()
    {
        string path = Path.Combine(Application.dataPath, "..", INIPath);
        if (!File.Exists(path))
        {
            Init();
        }

        INIParser ini = new INIParser();
        ini.Open(path);

        configs[Keys.UseTest] = ini.ReadValue(INISection, "UseTest", false);
        configs[Keys.UseBot] = ini.ReadValue(INISection, "UseBot", true);
        configs[Keys.ProxyObserve] = ini.ReadValue(INISection, "ProxyObserve", false);
        configs[Keys.WaitInGame] = ini.ReadValue(INISection, "WaitInGame", false);
        configs[Keys.AutoUpdate] = ini.ReadValue(INISection, "AutoUpdate", true);
        configs[Keys.UpdateAddress] = ini.ReadValue(INISection, "UpdateAddress", "http://39.105.200.223:81/thd2/bots/script.pak");
        

        ini.Close();

        path = Path.Combine(Application.dataPath, "..", "proxy.ini");
        if (!File.Exists(path))
        {
            File.WriteAllText(path, Constance.DefaultProxySetting);
        }
    }

    public static void Init()
    {
        string path = Path.Combine(Application.dataPath, "..", INIPath);
        if (!File.Exists(path))
        {
            Debug.LogWarning("can't find config.ini, try to create one.");
            File.Create(path).Dispose();
        }

        configs[Keys.UseTest] = false;
        configs[Keys.UseBot] = true;
        configs[Keys.WaitInGame] = false;
        configs[Keys.ProxyObserve] = false;
        configs[Keys.AutoUpdate] = true;
        configs[Keys.UpdateAddress] = "http://39.105.200.223:81/thd2/bots/script.pak";

        INIParser ini = new INIParser();
        ini.Open(path);

        ini.WriteValue(INISection, "UseTest", false);
        ini.WriteValue(INISection, "UseBot", true);
        ini.WriteValue(INISection, "WaitInGame", false);
        ini.WriteValue(INISection, "ProxyObserve", false);
        ini.WriteValue(INISection, "AutoUpdate", true);
        ini.WriteValue(INISection, "UpdateAddress", "http://39.105.200.223:81/thd2/bots/script.pak");

        ini.Close();
        Debug.Log("new config.ini created.");
    }
}

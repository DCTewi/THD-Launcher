using System.IO;
using UnityEngine;

public class Updater : MonoBehaviour
{
    public static Updater Instance;
    public static string TempPath;
    public static string ScriptPath;

    public void CheckUpdate()
    {
        string addr = Options.Get<string>(Options.Keys.UpdateAddress);

        if (addr != "local")
        {
            Debug.Log($"checking lastest version from {addr}");
            Downloader.HttpDownloadFile(addr, TempPath);

            if (LibLoader.GetScriptVersion(TempPath) > LibLoader.GetScriptVersion(ScriptPath))
            {
                File.Delete(ScriptPath);
                File.Move(TempPath, ScriptPath);
                Debug.Log("new version found!");
            }
            else
            {
                Debug.Log("no update found!");
            }
        }

        Debug.Log("update installed with " + LibLoader.InstallScript());
        int ver = LibLoader.GetScriptVersion(ScriptPath);
        int a = ver / 100, b = ver % 100 / 10, c = ver % 10;
        Debug.Log("now version: " + a + "." + b + "." + c);
        File.Delete(TempPath);
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

        TempPath = Path.Combine(Application.temporaryCachePath, "script.pak.tmp");
        ScriptPath = Path.Combine(Application.dataPath, "..", "script.pak");

        if (Options.Get<bool>(Options.Keys.AutoUpdate))
        {
            DotaSeeker.DotaSeekerInited += CheckUpdate;
        }
    }
}

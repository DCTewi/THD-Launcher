using System;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;

public class SetupScript : MonoBehaviour
{
    public static string tempPath;
    public static string scriptPath;
    public static string updateAddress = "http://39.105.200.223/thd2/bots/script.pak";

    private void Awake()
    {
        tempPath = Application.dataPath + "/../script.pak.tmp";
        scriptPath = Application.dataPath + "/../script.pak";
    }

    private void Start()
    {
        OnClick();
    }

    public void OnClick()
    {
        Debug.Log("Checking Lastest Version...");
        Downloader.HttpDownloadFile(updateAddress, tempPath);

        if (DLLLoader.GetScriptVersion(tempPath) > DLLLoader.GetScriptVersion(scriptPath))
        {
            File.Delete(scriptPath);
            File.Move(tempPath, scriptPath);
            Debug.Log("New Update Found!");
        }
        else
        {
            Debug.Log("No Update!");
        }

        Debug.Log("Update installed with " + DLLLoader.InstallScript());
        int ver = DLLLoader.GetScriptVersion(scriptPath);
        int a = ver / 100, b = ver % 100 / 10, c = ver % 10;
        Debug.Log("Now Version: " + a + "." + b + "." + c);
        File.Delete(tempPath);
    }
}

public class Downloader
{
    public static string HttpDownloadFile(string url, string path)
    {
        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        Stream responseStream = response.GetResponseStream();
        Stream stream = new FileStream(path, FileMode.Create);
        byte[] bArr = new byte[1024];
        int size = responseStream.Read(bArr, 0, (int)bArr.Length);
        while (size > 0)
        {
            stream.Write(bArr, 0, size);
            size = responseStream.Read(bArr, 0, (int)bArr.Length);
        }
        stream.Close();
        responseStream.Close();
        return path;
    }
}

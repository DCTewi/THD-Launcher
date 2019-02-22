using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public Canvas optionsCanvas;
    public Text oldDataPath;
    public Text newDotaPath;
    public Toggle useBot;
    public Toggle useTest;
    public Toggle waitInGame;
    public DotaFinder dotaFinder;

    public Text newInternalPort;
    public Text newExternalPort;
    public Text oldInternalPort;
    public Text oldExternalPort;

    public static bool UseTest = true;
    public static int WaitInGame = 0;
    public static int UseBot = 1;

    public void ShowOptions()
    {
        optionsCanvas.sortingOrder = 2;
        oldDataPath.text = DotaFinder.DotaPath;
        oldExternalPort.text = UPDP.eport.ToString();
        oldInternalPort.text = UPDP.iport.ToString();
    }

    public void HideOptions()
    {
        optionsCanvas.sortingOrder = -1;
    }

    public void FreshPathShow()
    {
        oldDataPath.text = DotaFinder.DotaPath;
    }

    public void SaveOptions()
    {
        if (File.Exists(newDotaPath.text) || File.Exists(oldDataPath.text))
        {
            DotaFinder.DotaPath = newDotaPath.text;
            dotaFinder.CreateINI(0);
            FreshPathShow();
        }
        else
        {
            Debug.Log("dota2.exe not found!");
        }

        UseTest = useTest.isOn ? true : false;
        UseBot = useBot.isOn? 1: 0;
        WaitInGame = waitInGame.isOn ? 1: 0;
        UPDP.eport = (newExternalPort.text != "")? Convert.ToInt32(newExternalPort.text): Convert.ToInt32(oldExternalPort.text);
        UPDP.iport = (newInternalPort.text != "") ? Convert.ToInt32(newInternalPort.text) : Convert.ToInt32(oldInternalPort.text);

        INIParser config = new INIParser();
        config.Open(Application.dataPath + "/../config.ini");
        config.WriteValue("thd-launcher", "UseTest", UseTest ? "1" : "0");
        config.WriteValue("thd-launcher", "WaitInGame", WaitInGame);
        config.WriteValue("thd-launcher", "ExPort", UPDP.eport.ToString());
        config.WriteValue("thd-launcher", "InPort", UPDP.iport.ToString());
        config.WriteValue("thd-launcher", "UseBot", UseBot);
        config.Close();
    }

    private void Start()
    {
        HideOptions();
        useTest.isOn = UseTest;
        useBot.isOn = UseBot == 1? true: false;

        INIParser config = new INIParser();
        string configPath = Application.dataPath + "/../config.ini";

        if (!config.IsKeyExists("thd-launcher", "UseBot"))
        {
            config.WriteValue("thd-launcher", "UseBot", UseBot);
        }

        if (!File.Exists(configPath))
        {
            File.Create(configPath).Dispose();
            config.Open(configPath);
            config.WriteValue("thd-launcher", "UseTest", UseTest ? "1" : "0");
            config.WriteValue("thd-launcher", "WaitInGame", WaitInGame);
            config.WriteValue("thd-launcher", "ExPort", UPDP.eport);
            config.WriteValue("thd-launcher", "InPort", UPDP.iport);
            config.WriteValue("thd-launcher", "UseBot", UseBot);
        }
        else
        {
            config.Open(configPath);
            UseTest = config.ReadValue("thd-launcher", "UseTest", "1") == "1" ? true: false;
            WaitInGame = config.ReadValue("thd-launcher", "WaitInGame", "0") == "1" ? 1 : 0;
            UPDP.eport = config.ReadValue("thd-launcher", "ExPort", UPDP.eport);
            UPDP.iport = config.ReadValue("thd-launcher", "InPort", UPDP.iport);
            UseBot = config.ReadValue("thd-launcher", "UseBot", "1") == "1" ? 1 : 0;
        }
        config.Close();
    }
}

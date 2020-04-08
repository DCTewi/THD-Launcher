using System;
using System.Text;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public static Launcher Instance;

    public void LaunchProxy()
    {
        StringBuilder addrSv = new StringBuilder(256);
        StringBuilder addrOb;

        bool isObs = Options.Get<bool>(Options.Keys.ProxyObserve);
        if (!isObs)
        {
            addrOb = new StringBuilder("@", 256);
        }
        else
        {
            addrOb = new StringBuilder(256);
        }

        var rtn = LibLoader.LaunchProxy(addrSv, addrOb, 500);

        Debug.Log($"proxy launched with {rtn}");

        Debug.Log($"Server:{addrSv}");

        if (isObs)
        {
            Debug.Log($"Observe:{addrOb}");
        }
    }

    public void LaunchServer(bool isTest, bool useBot)
    {
        IntPtr hWnd = (IntPtr)null;
        bool isWaitInGame = Options.Get<bool>(Options.Keys.WaitInGame);

        if (isWaitInGame)
        {
            hWnd = LibLoader.GetForegroundWindow();
            LibLoader.ShowWindow(hWnd, 0);
        }

        var rtn = LibLoader.LaunchServer(isTest ? 1 : 0, isWaitInGame ? 1 : 0, useBot ? 1 : 0);

        Debug.Log($"server launched with {rtn}");

        if (isWaitInGame)
        {
            LibLoader.ShowWindow(hWnd, 5);
        }
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
}
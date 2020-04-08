using System;
using UnityEngine;
using UnityEngine.UI;

public class Connector : MonoBehaviour
{
    public static Connector Instance;

    private Text IPText;

    public void ConnectToServer()
    {
        bool isWaitInGame = Options.Get<bool>(Options.Keys.WaitInGame);
        IntPtr hWnd = (IntPtr)null;

        if (isWaitInGame)
        {
            hWnd = LibLoader.GetForegroundWindow();
            LibLoader.ShowWindow(hWnd, 0);
        }

        Debug.Log($"connecting to {IPText.text} with {LibLoader.Connect(IPText.text, isWaitInGame ? 1 : 0)}");

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

    private void Start()
    {
        IPText = AppManager.Instance.IPAddressText;
    }
}

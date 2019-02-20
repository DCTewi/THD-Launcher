using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectServer : MonoBehaviour
{
    public Canvas IPCanvas;
    public Text IPText;

    public void ShowIPCanvas()
    {
        IPCanvas.sortingOrder = 1;
    }

    public void HideIPCanvas()
    {
        IPCanvas.sortingOrder = -1;
    }

    public void ConnectToServer()
    {
        Debug.Log("Connecting to " + IPText.text + " with " + DLLLoader.Connect(IPText.text, OptionManager.WaitInGame));
    }

    private void Awake()
    {
        HideIPCanvas();
    }
}

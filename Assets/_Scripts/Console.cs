using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    public Text console;
    public int maxLines = 21;

    private int lines;

    private void Awake()
    {
        Application.logMessageReceived += HandleLog;
        console.text = "";
        lines = 0;
    }

    private void HandleLog(string msg, string stackTrace, LogType type)
    {
        string message = string.Format("[{0}]:{1}\n", type, msg);
        SendLog(message);
    }

    private void Update()
    {
        if (lines > maxLines)
        {
            string s = console.text;
            console.text = s.Substring(s.IndexOf('\n') + 1);
            lines--;
        }
    }

    private void SendLog(string message)
    {
        console.text += message;
        lines++;
;    }
}

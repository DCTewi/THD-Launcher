using UnityEngine;
using UnityEngine.UI;

public class LogRedirector : MonoBehaviour
{
    public Button ClearButton;
    public Text Console;
    public int MaxLines;

    private int lines;

    private void ConsoleHandler(string condition, string stackTrace, LogType type)
    {
        var msg = $"[{type}]: {condition}";

        SendLog(msg);
    }

    private void SendLog(string msg)
    {
        Console.text += $"{msg}\n";
        lines++;
    }

    private void Awake()
    {
        Application.logMessageReceived += ConsoleHandler;

        Console.text = "";
        MaxLines = 21;

        lines = 0;

        ClearButton.onClick.AddListener(() =>
        {
            Console.text = "";
            lines = 0;
        });
    }

    private void Update()
    {
        if (lines > MaxLines)
        {
            var s = Console.text;
            Console.text = s.Substring(s.IndexOf('\n') + 1);
            lines--;
        }
    }
}

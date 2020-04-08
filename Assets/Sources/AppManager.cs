using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;

    [Header("Canvas")]
    [SerializeField] private Canvas OptionCanvas;
    [SerializeField] private Canvas ConnectCanvas;

    [Header("Main Canvas")]
    public Button CheckUpdateButton;
    public Button OpenProxyButton;
    public Button LaunchServerButton;
    public Button ConnectServerButton;
    public Button ConfigButton;

    [Header("Option Canvas")]
    public Text DotaPathPlaceHolder;
    public Text DotaPathText;
    public Button SaveDotaPathButton;
    public Button AutoDetectDotaButton;

    public Button OpenConfigINIButton;
    public Button OpenProxyINIButton;
    public Button UpdateConfigButton;

    [Header("Connect Canvas")]
    public Text IPAddressPlaceHolder;
    public Text IPAddressText;
    public Button ConnectButton;

    private void TriggerCanvas(Canvas canvas)
    {
        canvas.enabled = !canvas.enabled;
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
        DontDestroyOnLoad(this);

        OptionCanvas.enabled = false;
        ConnectCanvas.enabled = false;

        Options.Read();

        LibLoader.FileLogInit(Path.Combine(Application.dataPath, "launcher.log"));
    }

    private void Start()
    {
        // Main canvas
        CheckUpdateButton.onClick.AddListener(() =>
        {
            Updater.Instance.CheckUpdate();
        });

        OpenProxyButton.onClick.AddListener(() =>
        {
            Launcher.Instance.LaunchProxy();
        });

        LaunchServerButton.onClick.AddListener(() =>
        {
            Launcher.Instance.LaunchServer(Options.Get<bool>(Options.Keys.UseTest), Options.Get<bool>(Options.Keys.UseBot));
        });

        ConnectServerButton.onClick.AddListener(() =>
        {
            DotaPathPlaceHolder.text = DotaSeeker.DotaPath;
            TriggerCanvas(ConnectCanvas);
        });

        ConfigButton.onClick.AddListener(() =>
        {
            DotaPathPlaceHolder.text = DotaSeeker.DotaPath;
            TriggerCanvas(OptionCanvas);
        });

        // Config Canvas
        SaveDotaPathButton.onClick.AddListener(() =>
        {
            var newpath = DotaPathText.text;

            DotaSeeker.DotaPath = newpath;

            DotaPathPlaceHolder.text = newpath;

            DotaPathText.text = "";
        });

        AutoDetectDotaButton.onClick.AddListener(() =>
        {
            DotaSeeker.Instance.SeekDotaRegistry();

            DotaPathPlaceHolder.text = DotaSeeker.DotaPath;
            DotaPathText.text = DotaSeeker.DotaPath;
        });

        UpdateConfigButton.onClick.AddListener(() =>
        {
            Options.Read();
        });

        OpenConfigINIButton.onClick.AddListener(() =>
        {
            string path = Path.Combine(Application.dataPath, "..", Options.INIPath);
            if (!File.Exists(path))
            {
                Options.Init();
            }

            System.Diagnostics.Process.Start("notepad.exe", path);
        });

        OpenProxyINIButton.onClick.AddListener(() =>
        {
            string path = Path.Combine(Application.dataPath, "..", "proxy.ini");

            if (!File.Exists(path))
            {
                File.WriteAllText(path, Constance.DefaultProxySetting);
            }

            System.Diagnostics.Process.Start("notepad.exe", path);
        });

        // Connect canvas
        ConnectButton.onClick.AddListener(() =>
        {
            string path = Path.Combine(Application.dataPath, "..", "proxy.ini");

            if (!File.Exists(path))
            {
                File.WriteAllText(path, Constance.DefaultProxySetting);
            }

            Connector.Instance.ConnectToServer();
        });
    }
}


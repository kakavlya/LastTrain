using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    [SerializeField] bool overrideControl = false;
    [SerializeField] ControlScheme overrideScheme;

    public static PlatformDetector Instance { get; private set; }

    public enum ControlScheme
    {
        Mouse,
        Joystick
    }

    public ControlScheme CurrentControlScheme { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (overrideControl)
        {
            CurrentControlScheme = overrideScheme;
        }
        else
        {
            DetectPlatform();
        }
    }

    private void DetectPlatform()
    {
        bool isWebGL = Application.platform == RuntimePlatform.WebGLPlayer;
        bool isMobile = Application.isMobilePlatform;

        if (isWebGL && isMobile)
        {
            CurrentControlScheme = ControlScheme.Joystick;
        }
        else
        {
            CurrentControlScheme = ControlScheme.Mouse;
        }
    }
}

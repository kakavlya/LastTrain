using UnityEngine;

namespace LastTrain.Core
{
    public class PlatformDetector : MonoBehaviour
    {
        public static PlatformDetector Instance { get; private set; }

        [SerializeField] bool _overrideControl = false;
        [SerializeField] ControlScheme _overrideScheme;

        public enum ControlScheme
        {
            Computer,
            Mobile
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
            if (_overrideControl)
            {
                CurrentControlScheme = _overrideScheme;
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
                CurrentControlScheme = ControlScheme.Mobile;
            }
            else
            {
                CurrentControlScheme = ControlScheme.Computer;
            }
        }
    }
}

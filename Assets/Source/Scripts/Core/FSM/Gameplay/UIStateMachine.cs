using System;
using UnityEngine;
using UnityEngine.UI;
using LastTrain.Training;
using LastTrain.UI;

namespace LastTrain.Core.FSM
{
    public class UIStateMachine : TypeStateMachineMono
    {
        [Header("Screens")]
        [SerializeField] private GameObject _startScreen;
        [SerializeField] private GameObject _hudScreen;
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _gameEndScreen;
        [SerializeField] private GameObject _gamePauseScreen;
        [SerializeField] private GameObject _settingsScreen;

        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button[] _restartButtons;
        [SerializeField] private Button[] _pauseButtons;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button[] _menuButtons;
        [SerializeField] private Button _settingsButton;

        [Header("Mobile Platform Control")]
        [SerializeField] private GameObject _joustick;

        private UIScreenRouter _router;

        public event Action StartClicked;
        public event Action PauseClicked;
        public event Action ResumeClicked;
        public event Action RestartClicked;
        public event Action MenuClicked;

        public UIScreenRouter Router => _router;

        public GameObject StartScreen => _startScreen;

        public GameObject HudScreen => _hudScreen;

        public GameObject GameOverScreen => _gameOverScreen;

        public GameObject GameEndScreen => _gameEndScreen;

        public GameObject GamePauseScreen => _gamePauseScreen;

        public GameObject SettingsScreen => _settingsScreen;

        private void Awake()
        {
            _router = new UIScreenRouter(
                _startScreen, _hudScreen, _gameOverScreen, _gameEndScreen, _gamePauseScreen, _settingsScreen
            );

            FSM.Register(new LevelStartState(this));
            FSM.Register(new PlayingState(this));
            FSM.Register(new GameOverState(this));
            FSM.Register(new EndLevelState(this));
            FSM.Register(new PauseState(this));
            FSM.Register(new SettingsState(this));

            _startButton.onClick.AddListener(() =>
            {
                StartClicked?.Invoke();
                FSM.Switch<PlayingState>();
            });

            foreach (var b in _pauseButtons)
                b.onClick.AddListener(() =>
                {
                    PauseClicked?.Invoke();
                    FSM.Switch<PauseState>();
                });

            _resumeButton.onClick.AddListener(() =>
            {
                ResumeClicked?.Invoke();
                FSM.Switch<PlayingState>();
            });

            foreach (var b in _restartButtons)
                b.onClick.AddListener(() =>
                {
                    RestartClicked?.Invoke();
                    FSM.Switch<LevelStartState>();
                });

            foreach (var b in _menuButtons)
                b.onClick.AddListener(() => { MenuClicked?.Invoke(); });

            _settingsButton.onClick.AddListener(() => FSM.Switch<SettingsState>());

            _joustick?.SetActive(
                PlatformDetector.Instance != null &&
                PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Mobile
            );

            if (TrainingHandler.Instance != null && !TrainingHandler.Instance.IsDoneGameplayTraining)
            {
                foreach (var menu in _menuButtons)
                    menu.interactable = false;
            }

            FSM.Switch<LevelStartState>();
        }

        private void OnDestroy()
        {
            foreach (var b in _pauseButtons)
                b.onClick.RemoveAllListeners();

            foreach (var b in _restartButtons)
                b.onClick.RemoveAllListeners();

            foreach (var b in _menuButtons)
                b.onClick.RemoveAllListeners();

            _startButton.onClick.RemoveAllListeners();
            _resumeButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
        }
    }
}
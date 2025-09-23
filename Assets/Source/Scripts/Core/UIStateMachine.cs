using System;
using UnityEngine;
using UnityEngine.UI;
using LastTrain.Core.FSM;
using LastTrain.UI;
using LastTrain.Training;
using LastTrain.UI.FSM;

namespace LastTrain.Core
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

        [Header("Mobile Platorm Control")]
        [SerializeField] private GameObject _joustick;

        public event Action StartClicked;
        public event Action PauseClicked;
        public event Action ResumeClicked;
        public event Action RestartClicked;
        public event Action MenuClicked;

        public sealed class LevelStart { }
        public sealed class Playing { }
        public sealed class GameOver { }
        public sealed class EndLevel { }
        public sealed class Pause { }
        public sealed class Settings { }

        private UIScreenRouter _router;

        private void Awake()
        {
            _router = new UIScreenRouter(_startScreen, _hudScreen, _gameOverScreen, _gameEndScreen, _gamePauseScreen, _settingsScreen);

            Register<LevelStart>(new ScreenState<LevelStart>(_router, _startScreen));
            Register<Playing>(new ScreenState<Playing>(_router, _hudScreen));
            Register<GameOver>(new ScreenState<GameOver>(_router, _gameOverScreen));
            Register<EndLevel>(new ScreenState<EndLevel>(_router, _gameEndScreen));
            Register<Pause>(new ScreenState<Pause>(_router, _gamePauseScreen));
            Register<Settings>(new ScreenState<Settings>(_router, _settingsScreen));

            _startButton.onClick.AddListener(() => { StartClicked?.Invoke(); Switch<Playing>(); });

            foreach (var b in _pauseButtons)
                b.onClick.AddListener(() => { PauseClicked?.Invoke(); Switch<Pause>(); });

            _resumeButton.onClick.AddListener(() => { ResumeClicked?.Invoke(); Switch<Playing>(); });

            foreach (var b in _restartButtons)
                b.onClick.AddListener(() => { RestartClicked?.Invoke(); Switch<LevelStart>(); });

            foreach (var b in _menuButtons)
                b.onClick.AddListener(() => { MenuClicked?.Invoke(); });

            _settingsButton.onClick.AddListener(() => Switch<Settings>());

            _joustick?.SetActive(PlatformDetector.Instance != null &&
                                  PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Mobile);

            if (TrainingHandler.Instance != null && !TrainingHandler.Instance.IsDoneGameplayTraining)
                foreach (var menu in _menuButtons) menu.interactable = false;

            Switch<LevelStart>();
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveAllListeners();
            foreach (var b in _pauseButtons) b.onClick.RemoveAllListeners();
            _resumeButton.onClick.RemoveAllListeners();
            foreach (var b in _restartButtons) b.onClick.RemoveAllListeners();
            foreach (var b in _menuButtons) b.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
        }

        public void ShowLevelStart() => Switch<LevelStart>();
        public void ShowGameOver() => Switch<GameOver>();
        public void ShowEndLevel() => Switch<EndLevel>();
        public void ShowPause() => Switch<Pause>();
        public void ShowHUD() => Switch<Playing>();
        public void ShowSettings() => Switch<Settings>();
    }
}

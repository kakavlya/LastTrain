using System;
using UnityEngine;
using UnityEngine.UI;
using LastTrain.Core.FSM;
using LastTrain.UI;
using LastTrain.Training;

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

        [Header("Mobile Platform Control")]
        [SerializeField] private GameObject _joustick;

        public event Action StartClicked;
        public event Action PauseClicked;
        public event Action ResumeClicked;
        public event Action RestartClicked;
        public event Action MenuClicked;

        private UIScreenRouter _router;

        private void Awake()
        {
            _router = new UIScreenRouter(_startScreen, _hudScreen, _gameOverScreen, _gameEndScreen, _gamePauseScreen, _settingsScreen);

            Register(new LevelStartState(this));
            Register(new PlayingState(this));
            Register(new GameOverState(this));
            Register(new EndLevelState(this));
            Register(new PauseState(this));
            Register(new SettingsState(this));

            _startButton.onClick.AddListener(() => { StartClicked?.Invoke(); Switch<PlayingState>(); });

            foreach (var b in _pauseButtons)
                b.onClick.AddListener(() => { PauseClicked?.Invoke(); Switch<PauseState>(); });

            _resumeButton.onClick.AddListener(() => { ResumeClicked?.Invoke(); Switch<PlayingState>(); });

            foreach (var b in _restartButtons)
                b.onClick.AddListener(() => { RestartClicked?.Invoke(); Switch<LevelStartState>(); });

            foreach (var b in _menuButtons)
                b.onClick.AddListener(() => { MenuClicked?.Invoke(); });

            _settingsButton.onClick.AddListener(() => Switch<SettingsState>());

            _joustick?.SetActive(PlatformDetector.Instance != null &&
                                  PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Mobile);

            if (TrainingHandler.Instance != null && !TrainingHandler.Instance.IsDoneGameplayTraining)
                foreach (var menu in _menuButtons) menu.interactable = false;

            Switch<LevelStartState>();
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

        public void ShowLevelStart() => Switch<LevelStartState>();
        public void ShowHUD() => Switch<PlayingState>();
        public void ShowGameOver() => Switch<GameOverState>();
        public void ShowEndLevel() => Switch<EndLevelState>();
        public void ShowPause() => Switch<PauseState>();
        public void ShowSettings() => Switch<SettingsState>();

        private abstract class UMState : IState
        {
            protected readonly UIStateMachine UI;
            protected UMState(UIStateMachine ui) { UI = ui; }
            public virtual void Enter() { }
            public virtual void Exit() { }
        }

        private sealed class LevelStartState : UMState
        {
            public LevelStartState(UIStateMachine ui) : base(ui) { }
            public override void Enter() => UI._router.ShowOnly(UI._startScreen);
        }

        private sealed class PlayingState : UMState
        {
            public PlayingState(UIStateMachine ui) : base(ui) { }
            public override void Enter() => UI._router.ShowOnly(UI._hudScreen);
        }

        private sealed class GameOverState : UMState
        {
            public GameOverState(UIStateMachine ui) : base(ui) { }
            public override void Enter() => UI._router.ShowOnly(UI._gameOverScreen);
        }

        private sealed class EndLevelState : UMState
        {
            public EndLevelState(UIStateMachine ui) : base(ui) { }
            public override void Enter() => UI._router.ShowOnly(UI._gameEndScreen);
        }

        private sealed class PauseState : UMState
        {
            public PauseState(UIStateMachine ui) : base(ui) { }
            public override void Enter() => UI._router.ShowOnly(UI._gamePauseScreen);
        }

        private sealed class SettingsState : UMState
        {
            public SettingsState(UIStateMachine ui) : base(ui) { }
            public override void Enter() => UI._router.ShowOnly(UI._settingsScreen);
        }
    }
}

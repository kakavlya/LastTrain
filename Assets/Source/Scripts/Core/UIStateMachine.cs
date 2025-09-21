using System;
using UnityEngine;
using UnityEngine.UI;
using LastTrain.Training;

namespace LastTrain.Core
{
    public class UIStateMachine : MonoBehaviour
    {
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

        private UIState _currentState = UIState.None;

        public event Action StartClicked;
        public event Action PauseClicked;
        public event Action ResumeClicked;
        public event Action RestartClicked;
        public event Action MenuClicked;

        public enum UIState
        {
            None,
            LevelStart,
            Playing,
            GameOver,
            EndLevel,
            Pause,
            Start,
            Settings
        }

        private void Start()
        {
            _startButton.onClick.AddListener(OnStartButton);

            foreach (var button in _pauseButtons)
                button.onClick.AddListener(() => { PauseClicked?.Invoke(); SwitchState(UIState.Pause); });

            _resumeButton.onClick.AddListener(() => { ResumeClicked?.Invoke(); SwitchState(UIState.Playing); });

            foreach (var button in _restartButtons)
                button.onClick.AddListener(OnRestartButton);

            foreach (var button in _menuButtons)
                button.onClick.AddListener(OnMenuButton);

            _settingsButton.onClick.AddListener(OnSettingsButton);

            if (PlatformDetector.Instance != null && PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Mobile)
            {
                _joustick.SetActive(true);
            }
            else
            {
                _joustick.SetActive(false);
            }

            DisableMenuButtonsIfTraining();
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartButton);

            foreach (var button in _pauseButtons)
                button.onClick.RemoveAllListeners();

            _resumeButton.onClick.RemoveAllListeners();

            foreach (var button in _restartButtons)
                button.onClick.RemoveListener(OnRestartButton);
        }

        public void SwitchState(UIState state)
        {
            if (_currentState == state)
                return;

            _currentState = state;
            DisableAll();

            switch (state)
            {
                case UIState.LevelStart:
                    _startScreen.SetActive(true);
                    break;
                case UIState.Playing:
                    _hudScreen.SetActive(true);
                    break;
                case UIState.GameOver:
                    _gameOverScreen.SetActive(true);
                    break;
                case UIState.EndLevel:
                    _gameEndScreen.SetActive(true);
                    break;
                case UIState.Pause:
                    _gamePauseScreen.SetActive(true);
                    break;
                case UIState.Settings:
                    _settingsScreen.SetActive(true);
                    break;
            }
        }

        public void OnStartButton()
        {
            StartClicked?.Invoke();
            SwitchState(UIState.Playing);
        }

        public void OnRestartButton()
        {
            RestartClicked?.Invoke();
            SwitchState(UIState.LevelStart);
        }

        public void OnMenuButton()
        {
            MenuClicked?.Invoke();
        }

        public void OnSettingsButton()
        {
            SwitchState(UIState.Settings);
        }

        private void DisableAll()
        {
            _startScreen.SetActive(false);
            _hudScreen.SetActive(false);
            _gameOverScreen.SetActive(false);
            _gameEndScreen.SetActive(false);
            _gamePauseScreen.SetActive(false);
            _settingsScreen.SetActive(false);
        }

        private void DisableMenuButtonsIfTraining()
        {
            if (TrainingHandler.Instance.IsDoneGameplayTraining == false)
            {
                foreach (var menu in _menuButtons)
                {
                    menu.interactable = false;
                }
            }
        }
    }
}

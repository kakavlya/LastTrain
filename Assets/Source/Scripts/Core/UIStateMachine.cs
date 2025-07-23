using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStateMachine : MonoBehaviour
{
    public enum UIState
    {
        None,
        LevelStart,
        Playing,
        GameOver,
        EndLevel,
        Pause,
        Start
    }

    [SerializeField] private GameObject _startScreen;
    [SerializeField] private GameObject _hudScreen;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _gameEndScreen;
    [SerializeField] private GameObject _gamePauseScreen;

    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _menuButton;

    [Header("Mobile Platorm Control")]
    [SerializeField] private GameObject _joustick;

    public event Action StartClicked;
    public event Action PauseClicked;
    public event Action ResumeClicked;
    public event Action RestartClicked;
    public event Action MenuClicked;

    private UIState _currentState = UIState.None;
    private LevelStateMachine _levelStateMachine;

    private void Awake()
    {
        _startButton.onClick.AddListener(OnStartButton);
        _pauseButton.onClick.AddListener(() => { PauseClicked?.Invoke(); SwitchState(UIState.Pause); });
        _resumeButton.onClick.AddListener(() => { ResumeClicked?.Invoke(); SwitchState(UIState.Playing); });
        _restartButton.onClick.AddListener(OnRestartButton);
        _menuButton.onClick.AddListener(OnMenuButton);

        if (PlatformDetector.Instance != null && PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Joystick)
        {
            _joustick.SetActive(true);

        }
        else
        {
            _joustick.SetActive(false);
        }
    }

    public void Construct(LevelStateMachine levelStateMachine)
    {
        _levelStateMachine = levelStateMachine;
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

    private void DisableAll()
    {
        _startScreen.SetActive(false);
        _hudScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        _gameEndScreen.SetActive(false);
        _gamePauseScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveListener(OnStartButton);
        _pauseButton.onClick.RemoveAllListeners();
        _resumeButton.onClick.RemoveAllListeners();
        _restartButton.onClick.RemoveListener(OnRestartButton);
    }
}

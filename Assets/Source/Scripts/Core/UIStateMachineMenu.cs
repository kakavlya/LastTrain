using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStateMachineMenu : MonoBehaviour
{
    public enum UIState
    {
        Settings,
        Level
    }

    [SerializeField] private PlayHandler _playHandler;

    [Header("Screens")]
    [SerializeField] private GameObject _settingsScreen;
    [SerializeField] private GameObject _choseLevelScreen;

    [Header("Buttons")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _choseLevelButton;
    [SerializeField] private Button[] _returnOnMainButtons;

    private void Awake()
    {
        foreach (var button in _returnOnMainButtons)
            button.onClick.AddListener(DisableAll);

        _settingsButton.onClick.AddListener(OnSettingsButton);
        _choseLevelButton.onClick.AddListener(OnLevelChose);
    }

    public void SwitchState(UIState state)
    {
        DisableAll();

        switch (state)
        {
            case UIState.Settings:
                _settingsScreen.SetActive(true);
                break;

            case UIState.Level:
                _choseLevelScreen.SetActive(true); 
                break;

        }
    }

    private void OnSettingsButton()
    {
        SwitchState(UIState.Settings);
    }

    private void OnLevelChose()
    {
        SwitchState(UIState.Level);
    }

    private void DisableAll()
    {
        _settingsScreen.SetActive(false);
        _choseLevelScreen.SetActive(false);
    }
}

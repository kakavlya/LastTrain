using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStateMachineMenu : MonoBehaviour
{
    public enum UIState
    {
        Settings,
        Level,
        Weapon
    }

    [SerializeField] private PlayHandler _playHandler;

    [Header("Screens")]
    [SerializeField] private GameObject _settingsScreen;
    [SerializeField] private GameObject _choseLevelScreen;
    [SerializeField] private GameObject _choseWeaponScreen;

    [Header("Buttons")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _choseLevelButton;
    [SerializeField] private Button _choseWeaponButton;
    [SerializeField] private Button[] _returnOnMainButtons;

    private void Awake()
    {
        foreach (var button in _returnOnMainButtons)
            button.onClick.AddListener(OnReturnButton);

        _settingsButton.onClick.AddListener(OnSettingsButton);
        _choseLevelButton.onClick.AddListener(OnLevelChoseButton);
        _choseWeaponButton.onClick.AddListener(OnWeaponChoseButton);
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

            case UIState.Weapon:
                _choseWeaponScreen.SetActive(true);
                break;
        }
    }

    private void OnSettingsButton()
    {
        SwitchState(UIState.Settings);
    }

    private void OnLevelChoseButton()
    {
        SwitchState(UIState.Level);
    }

    private void OnWeaponChoseButton()
    {
        SwitchState(UIState.Weapon);
    }

    private void OnReturnButton()
    {
        _settingsScreen.SetActive(false);
        _choseLevelScreen.SetActive(false);
        _choseWeaponScreen.SetActive(false);
        _settingsButton.gameObject.SetActive(true);
        _choseLevelButton.gameObject.SetActive(true);
        _choseWeaponButton.gameObject.SetActive(true);
    }

    private void DisableAll()
    {
        _settingsScreen.SetActive(false);
        _choseLevelScreen.SetActive(false);
        _choseWeaponScreen.SetActive(false);
        _settingsButton.gameObject.SetActive(false);
        _choseLevelButton.gameObject.SetActive(false);
        _choseWeaponButton.gameObject.SetActive(false);
    }
}

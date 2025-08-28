using UnityEngine;
using UnityEngine.UI;
using LastTrain.UI.MainMenu;

namespace LastTrain.Core
{
    public class UIStateMachineMenu : MonoBehaviour
    {
        [SerializeField] private PlayHandler _playHandler;

        [Header("Screens")]
        [SerializeField] private GameObject _settingsScreen;
        [SerializeField] private GameObject _choseLevelScreen;
        [SerializeField] private GameObject _choseWeaponScreen;
        [SerializeField] private GameObject _shopScreen;
        [SerializeField] private GameObject _leadeboardScreen;

        [Header("Buttons")]
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _choseLevelButton;
        [SerializeField] private Button _choseWeaponButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _leaderBoardButton;
        [SerializeField] private Button[] _returnOnMainButtons;

        public enum UIState
        {
            Settings,
            Level,
            Weapon,
            Shop,
            Leaderboard
        }

        private void Awake()
        {
            foreach (var button in _returnOnMainButtons)
                button.onClick.AddListener(OnReturnButton);

            _settingsButton.onClick.AddListener(OnSettingsButton);
            _choseLevelButton.onClick.AddListener(OnLevelChoseButton);
            _choseWeaponButton.onClick.AddListener(OnWeaponChoseButton);
            _shopButton.onClick.AddListener(OnShopButton);
            _leaderBoardButton.onClick.AddListener(OnLeaderboardButton);
        }

        private void SwitchState(UIState state)
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

                case UIState.Shop:
                    _shopScreen.SetActive(true);
                    break;

                case UIState.Leaderboard:
                    _leadeboardScreen.SetActive(true);
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

        private void OnShopButton()
        {
            SwitchState(UIState.Shop);
        }

        private void OnLeaderboardButton()
        {
            SwitchState(UIState.Leaderboard);
        }

        private void OnReturnButton()
        {
            _settingsScreen.SetActive(false);
            _choseLevelScreen.SetActive(false);
            _choseWeaponScreen.SetActive(false);
            _shopScreen.SetActive(false);
            _leadeboardScreen.SetActive(false);
            _settingsButton.gameObject.SetActive(true);
            _choseLevelButton.gameObject.SetActive(true);
            _choseWeaponButton.gameObject.SetActive(true);
            _shopButton.gameObject.SetActive(true);
            _leaderBoardButton.gameObject.SetActive(true);
        }

        private void DisableAll()
        {
            _settingsScreen.SetActive(false);
            _choseLevelScreen.SetActive(false);
            _choseWeaponScreen.SetActive(false);
            _shopScreen.SetActive(false);
            _leadeboardScreen.SetActive(false);
            _settingsButton.gameObject.SetActive(false);
            _choseLevelButton.gameObject.SetActive(false);
            _choseWeaponButton.gameObject.SetActive(false);
            _shopButton.gameObject.SetActive(false);
            _leaderBoardButton.gameObject.SetActive(false);
        }
    }
}

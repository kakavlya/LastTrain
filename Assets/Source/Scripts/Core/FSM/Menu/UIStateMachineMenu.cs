using UnityEngine;
using UnityEngine.UI;
using LastTrain.UI;
using LastTrain.UI.MainMenu;

namespace LastTrain.Core.FSM
{
    public class UIStateMachineMenu : TypeStateMachineMono
    {
        [SerializeField] private PlayHandler _playHandler;

        [Header("Screens")]
        [SerializeField] private GameObject _settingsScreen;
        [SerializeField] private GameObject _choseLevelScreen;
        [SerializeField] private GameObject _choseWeaponScreen;
        [SerializeField] private GameObject _shopScreen;
        [SerializeField] private GameObject _leaderboardScreen;

        [Header("Buttons")]
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _choseLevelButton;
        [SerializeField] private Button _choseWeaponButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _leaderBoardButton;
        [SerializeField] private Button[] _returnOnMainButtons;

        private UIScreenRouter _router;
        private GameObject[] _mainButtons;

        public UIScreenRouter Router => _router;

        public GameObject[] MainButtons => _mainButtons;

        public GameObject SettingsScreen => _settingsScreen;

        public GameObject ChoseLevelScreen => _choseLevelScreen;

        public GameObject ChoseWeaponScreen => _choseWeaponScreen;

        public GameObject ShopScreen => _shopScreen;

        public GameObject LeaderboardScreen => _leaderboardScreen;

        private void Awake()
        {
            _router = new UIScreenRouter(
                _settingsScreen, _choseLevelScreen, _choseWeaponScreen, _shopScreen, _leaderboardScreen
            );

            _mainButtons = new[]
            {
                _settingsButton.gameObject,
                _choseLevelButton.gameObject,
                _choseWeaponButton.gameObject,
                _shopButton.gameObject,
                _leaderBoardButton.gameObject
            };

            FSM.Register(new RootState(this));
            FSM.Register(new SettingsMenuState(this));
            FSM.Register(new LevelState(this));
            FSM.Register(new WeaponState(this));
            FSM.Register(new ShopState(this));
            FSM.Register(new LeaderboardState(this));

            _settingsButton.onClick.AddListener(() => FSM.Switch<SettingsMenuState>());
            _choseLevelButton.onClick.AddListener(() => FSM.Switch<LevelState>());
            _choseWeaponButton.onClick.AddListener(() => FSM.Switch<WeaponState>());
            _shopButton.onClick.AddListener(() => FSM.Switch<ShopState>());
            _leaderBoardButton.onClick.AddListener(() => FSM.Switch<LeaderboardState>());

            foreach (var b in _returnOnMainButtons)
                b.onClick.AddListener(() => FSM.Switch<RootState>());

            FSM.Switch<RootState>();
        }

        private void OnDestroy()
        {
            _settingsButton.onClick.RemoveAllListeners();
            _choseLevelButton.onClick.RemoveAllListeners();
            _choseWeaponButton.onClick.RemoveAllListeners();
            _shopButton.onClick.RemoveAllListeners();
            _leaderBoardButton.onClick.RemoveAllListeners();

            foreach (var b in _returnOnMainButtons)
                b.onClick.RemoveAllListeners();
        }
    }
}
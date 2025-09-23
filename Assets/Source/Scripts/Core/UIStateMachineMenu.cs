using UnityEngine;
using UnityEngine.UI;
using LastTrain.Core.FSM;
using LastTrain.UI;
using LastTrain.UI.MainMenu;

namespace LastTrain.Core
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

        public sealed class Root { }
        public sealed class Settings { }
        public sealed class Level { }
        public sealed class Weapon { }
        public sealed class Shop { }
        public sealed class Leaderboard { }

        private UIScreenRouter _router;
        private GameObject[] _mainButtons;

        private void Awake()
        {
            _router = new UIScreenRouter(_settingsScreen, _choseLevelScreen, _choseWeaponScreen, _shopScreen, _leaderboardScreen);
            _mainButtons = new[]
            {
                _settingsButton.gameObject,
                _choseLevelButton.gameObject,
                _choseWeaponButton.gameObject,
                _shopButton.gameObject,
                _leaderBoardButton.gameObject
            };

            Register<Root>(new RootState(_router, _mainButtons));
            Register<Settings>(new ChildScreenState(_router, _settingsScreen, _mainButtons));
            Register<Level>(new ChildScreenState(_router, _choseLevelScreen, _mainButtons));
            Register<Weapon>(new ChildScreenState(_router, _choseWeaponScreen, _mainButtons));
            Register<Shop>(new ChildScreenState(_router, _shopScreen, _mainButtons));
            Register<Leaderboard>(new ChildScreenState(_router, _leaderboardScreen, _mainButtons));

            _settingsButton.onClick.AddListener(() => Switch<Settings>());
            _choseLevelButton.onClick.AddListener(() => Switch<Level>());
            _choseWeaponButton.onClick.AddListener(() => Switch<Weapon>());
            _shopButton.onClick.AddListener(() => Switch<Shop>());
            _leaderBoardButton.onClick.AddListener(() => Switch<Leaderboard>());

            foreach (var b in _returnOnMainButtons) b.onClick.AddListener(() => Switch<Root>());

            Switch<Root>();
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

        private sealed class RootState : IState
        {
            private readonly UIScreenRouter _router;
            private readonly GameObject[] _mainButtons;

            public RootState(UIScreenRouter router, GameObject[] mainButtons)
            {
                _router = router; _mainButtons = mainButtons;
            }

            public void Enter()
            {
                _router.HideAll(); SetMain(true);
            }

            public void Exit()
            {
                SetMain(false);
            }

            private void SetMain(bool v)
            {
                foreach (var go in _mainButtons) if (go) go.SetActive(v);
            }
        }

        private sealed class ChildScreenState : IState
        {
            private readonly UIScreenRouter _router;
            private readonly GameObject _screen;
            private readonly GameObject[] _mainButtons;
            public ChildScreenState(UIScreenRouter router, GameObject screen, GameObject[] mainButtons)
            {
                _router = router; _screen = screen; _mainButtons = mainButtons;
            }
            public void Enter()
            {
                SetMain(false);
                _router.ShowOnly(_screen);
            }
            public void Exit()
            {
                _router.HideAll();
            }
            private void SetMain(bool v)
            {
                foreach (var go in _mainButtons)
                    if (go) go.SetActive(v);
            }
        }
    }
}

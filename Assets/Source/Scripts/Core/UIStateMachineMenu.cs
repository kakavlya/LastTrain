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

            Register(new RootState(this));
            Register(new SettingsState(this));
            Register(new LevelState(this));
            Register(new WeaponState(this));
            Register(new ShopState(this));
            Register(new LeaderboardState(this));

            _settingsButton.onClick.AddListener(() => Switch<SettingsState>());
            _choseLevelButton.onClick.AddListener(() => Switch<LevelState>());
            _choseWeaponButton.onClick.AddListener(() => Switch<WeaponState>());
            _shopButton.onClick.AddListener(() => Switch<ShopState>());
            _leaderBoardButton.onClick.AddListener(() => Switch<LeaderboardState>());

            foreach (var b in _returnOnMainButtons)
                b.onClick.AddListener(() => Switch<RootState>());

            Switch<RootState>();
        }

        private void OnDestroy()
        {
            _settingsButton.onClick.RemoveAllListeners();
            _choseLevelButton.onClick.RemoveAllListeners();
            _choseWeaponButton.onClick.RemoveAllListeners();
            _shopButton.onClick.RemoveAllListeners();
            _leaderBoardButton.onClick.RemoveAllListeners();
            foreach (var b in _returnOnMainButtons) b.onClick.RemoveAllListeners();
        }

        private abstract class MMState : IState
        {
            protected readonly UIStateMachineMenu UI;
            protected MMState(UIStateMachineMenu ui) { UI = ui; }
            public virtual void Enter() { }
            public virtual void Exit() { }

            protected void SetMain(bool visible)
            {
                foreach (var go in UI._mainButtons) if (go) go.SetActive(visible);
            }
        }

        private sealed class RootState : MMState
        {
            public RootState(UIStateMachineMenu ui) : base(ui) { }
            public override void Enter() { UI._router.HideAll(); SetMain(true); }
            public override void Exit() { SetMain(false); }
        }

        private sealed class SettingsState : MMState
        {
            public SettingsState(UIStateMachineMenu ui) : base(ui) { }
            public override void Enter() { SetMain(false); UI._router.ShowOnly(UI._settingsScreen); }
            public override void Exit() { UI._router.HideAll(); }
        }

        private sealed class LevelState : MMState
        {
            public LevelState(UIStateMachineMenu ui) : base(ui) { }
            public override void Enter() { SetMain(false); UI._router.ShowOnly(UI._choseLevelScreen); }
            public override void Exit() { UI._router.HideAll(); }
        }

        private sealed class WeaponState : MMState
        {
            public WeaponState(UIStateMachineMenu ui) : base(ui) { }
            public override void Enter() { SetMain(false); UI._router.ShowOnly(UI._choseWeaponScreen); }
            public override void Exit() { UI._router.HideAll(); }
        }

        private sealed class ShopState : MMState
        {
            public ShopState(UIStateMachineMenu ui) : base(ui) { }
            public override void Enter() { SetMain(false); UI._router.ShowOnly(UI._shopScreen); }
            public override void Exit() { UI._router.HideAll(); }
        }

        private sealed class LeaderboardState : MMState
        {
            public LeaderboardState(UIStateMachineMenu ui) : base(ui) { }
            public override void Enter() { SetMain(false); UI._router.ShowOnly(UI._leaderboardScreen); }
            public override void Exit() { UI._router.HideAll(); }
        }
    }
}
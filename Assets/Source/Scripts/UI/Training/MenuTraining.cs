using System;
using UnityEngine;
using UnityEngine.UI;
using YG;
using LastTrain.Core.FSM;
using LastTrain.Inventory;
using LastTrain.Persistence;
using LastTrain.ShopSystem;

namespace LastTrain.Training
{
    public class MenuTraining : TypeStateMachineMono
    {
        [Header("Training Screens")]
        [SerializeField] private GameObject _startTrainingScreen;
        [SerializeField] private GameObject _shopOpenTrainingScreen;
        [SerializeField] private GameObject _shopInfoTrainingScreen;
        [SerializeField] private GameObject _shopUnlockTrainingScreen;
        [SerializeField] private GameObject _shopBackTrainingScreen;
        [SerializeField] private GameObject _inventoryOpenTrainingScreen;
        [SerializeField] private GameObject _inventoryDragTrainingScreen;
        [SerializeField] private GameObject _inventoryCloseTrainingScreen;
        [SerializeField] private GameObject _choseLevelOpenScreen;
        [SerializeField] private GameObject _choseLevelCloseScreen;

        [Header("Menu Screens")]
        [SerializeField] private GameObject _shopScreen;
        [SerializeField] private GameObject _inventoryScreen;

        [Header("Menu Buttons")]
        [SerializeField] private Button _startLevelButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _choseLevelButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _backFromShopButton;
        [SerializeField] private Button _backFromInventoryButton;
        [SerializeField] private Button _backFromChoseLevelButton;
        [SerializeField] private Button _rewardButton;

        [Header("Training Buttons")]
        [SerializeField] private Button _startTrainingOkButton;
        [SerializeField] private Button _shopInfoTrainingOkButton;
        [SerializeField] private Button _choseLevelCloseButton;

        [Header("Training Locks")]
        [SerializeField] private GameObject _shopLockerScreen;
        [SerializeField] private GameObject _inventoryLockerScreen;

        [Header("Content")]
        [SerializeField] private GameObject _shopContent;
        [SerializeField] private GameObject _playerInventory;

        private ShopItemUI[] _shopItems;
        private WeaponSlotUI[] _weaponSlots;

        private void Start()
        {
            if (TrainingHandler.Instance.IsDoneGameplayTraining &&
                !TrainingHandler.Instance.IsDoneMenuTraining)
            {
                FSM.Register(new StartState(this));
                FSM.Register(new ShopOpenState(this));
                FSM.Register(new ShopInfoState(this));
                FSM.Register(new ShopUnlockState(this));
                FSM.Register(new ShopCloseState(this));
                FSM.Register(new InventoryOpenState(this));
                FSM.Register(new InventoryDragState(this));
                FSM.Register(new InventoryCloseState(this));
                FSM.Register(new ChoseLevelOpenState(this));
                FSM.Register(new ChoseLevelCloseState(this));
                FSM.Register(new EndState(this));

                DisableAllTrainingScreens();
                SwitchToSavedOrStart();
            }
        }

        private void SwitchToSavedOrStart()
        {
            var s = YG2.saves.TrainingState;
            switch (s)
            {
                case MenuTrainingState.Start: FSM.Switch<StartState>(); break;
                case MenuTrainingState.ShopOpen: FSM.Switch<ShopOpenState>(); break;
                case MenuTrainingState.ShopInfo: FSM.Switch<ShopInfoState>(); break;
                case MenuTrainingState.ShopUnlock: FSM.Switch<ShopUnlockState>(); break;
                case MenuTrainingState.ShopClose: FSM.Switch<ShopCloseState>(); break;
                case MenuTrainingState.InventoryOpen: FSM.Switch<InventoryOpenState>(); break;
                case MenuTrainingState.InventoryDrag: FSM.Switch<InventoryDragState>(); break;
                case MenuTrainingState.InventoryClose: FSM.Switch<InventoryCloseState>(); break;
                case MenuTrainingState.ChoseLevelOpen: FSM.Switch<ChoseLevelOpenState>(); break;
                case MenuTrainingState.ChoseLevelClose: FSM.Switch<ChoseLevelCloseState>(); break;
                case MenuTrainingState.End: FSM.Switch<EndState>(); break;
                default: FSM.Switch<StartState>(); break;
            }
        }

        internal void DisableAllTrainingScreens()
        {
            _startTrainingScreen.SetActive(false);
            _shopOpenTrainingScreen.SetActive(false);
            _shopInfoTrainingScreen.SetActive(false);
            _shopUnlockTrainingScreen.SetActive(false);
            _shopBackTrainingScreen.SetActive(false);
            _inventoryOpenTrainingScreen.SetActive(false);
            _inventoryDragTrainingScreen.SetActive(false);
            _inventoryCloseTrainingScreen.SetActive(false);
            _choseLevelOpenScreen.SetActive(false);
            _choseLevelCloseScreen.SetActive(false);
        }

        internal void SetAllMainInteractable(bool interactable)
        {
            _inventoryButton.interactable = interactable;
            _choseLevelButton.interactable = interactable;
            _shopButton.interactable = interactable;
            _startLevelButton.interactable = interactable;
            _rewardButton.interactable = interactable;
        }

        internal static void SaveStep(MenuTrainingState state)
        {
            YG2.saves.TrainingState = state;
            YG2.SaveProgress();
        }

        internal void SignUpShopItemsUILock(Action<WeaponProgress, WeaponUpgradeConfig> onUnlocked)
        {
            _shopItems = _shopContent.GetComponentsInChildren<ShopItemUI>(true);
            foreach (var item in _shopItems)
            {
                item.WeaponUnlocked += onUnlocked;
                var upg = item.GetComponentInChildren<Button>(true);
                if (upg) upg.interactable = false;
            }
        }

        internal void UnsubscribeShopItems(Action<WeaponProgress, WeaponUpgradeConfig> onUnlocked)
        {
            if (_shopItems == null) return;
            foreach (var item in _shopItems)
                item.WeaponUnlocked -= onUnlocked;
        }

        internal void UnlockAllUpgradeButtons()
        {
            _shopItems = _shopContent.GetComponentsInChildren<ShopItemUI>(true);
            foreach (var item in _shopItems)
            {
                var upg = item.GetComponentInChildren<Button>(true);
                if (upg) upg.interactable = true;
            }
        }

        internal void SignUpPlayerInventorySlots(Action onFilled)
        {
            _weaponSlots = _playerInventory.GetComponentsInChildren<WeaponSlotUI>(true);
            foreach (var slot in _weaponSlots)
                slot.Filled += onFilled;
        }

        internal void UnsubscribeInventorySlots(Action onFilled)
        {
            if (_weaponSlots == null) return;
            foreach (var slot in _weaponSlots)
                slot.Filled -= onFilled;
        }

        private sealed class StartState : IState
        {
            private readonly MenuTraining _menuTraining;
            public StartState(MenuTraining c) { this._menuTraining = c; }

            public void Enter()
            {
                _menuTraining.DisableAllTrainingScreens();
                _menuTraining.SetAllMainInteractable(false);
                _menuTraining._startTrainingScreen.SetActive(true);
                SaveStep(MenuTrainingState.Start);
                _menuTraining._startTrainingOkButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                _menuTraining._startTrainingOkButton.onClick.RemoveListener(OnNext);
                _menuTraining._startTrainingScreen.SetActive(false);
            }

            private void OnNext() => _menuTraining.FSM.Switch<ShopOpenState>();
        }

        private sealed class ShopOpenState : IState
        {
            private readonly MenuTraining _menuTraining;
            public ShopOpenState(MenuTraining c) { this._menuTraining = c; }

            public void Enter()
            {
                _menuTraining.DisableAllTrainingScreens();
                _menuTraining._shopOpenTrainingScreen.SetActive(true);
                _menuTraining._shopButton.interactable = true;
                _menuTraining._startLevelButton.interactable = false;
                _menuTraining._inventoryButton.interactable = false;
                _menuTraining._choseLevelButton.interactable = false;
                _menuTraining._rewardButton.interactable = false;
                SaveStep(MenuTrainingState.ShopOpen);
                _menuTraining._shopButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                _menuTraining._shopButton.onClick.RemoveListener(OnNext);
                _menuTraining._shopOpenTrainingScreen.SetActive(false);
            }

            private void OnNext() => _menuTraining.FSM.Switch<ShopInfoState>();
        }

        private sealed class ShopInfoState : IState
        {
            private readonly MenuTraining _menuTraining;
            public ShopInfoState(MenuTraining c) { this._menuTraining = c; }

            public void Enter()
            {
                _menuTraining.DisableAllTrainingScreens();
                _menuTraining._shopScreen.SetActive(true);
                _menuTraining._shopLockerScreen.SetActive(true);
                _menuTraining._shopInfoTrainingScreen.SetActive(true);
                _menuTraining._backFromShopButton.interactable = false;
                SaveStep(MenuTrainingState.ShopInfo);
                _menuTraining._shopInfoTrainingOkButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                _menuTraining._shopInfoTrainingOkButton.onClick.RemoveListener(OnNext);
                _menuTraining._shopInfoTrainingScreen.SetActive(false);
            }

            private void OnNext() => _menuTraining.FSM.Switch<ShopUnlockState>();
        }

        private sealed class ShopUnlockState : IState
        {
            private readonly MenuTraining _menuTraining;
            public ShopUnlockState(MenuTraining c) { this._menuTraining = c; }

            public void Enter()
            {
                _menuTraining.DisableAllTrainingScreens();
                _menuTraining._shopScreen.SetActive(true);
                _menuTraining._shopLockerScreen.SetActive(false);
                _menuTraining._shopUnlockTrainingScreen.SetActive(true);
                SaveStep(MenuTrainingState.ShopUnlock);
                _menuTraining.SignUpShopItemsUILock(OnUnlocked);
            }

            public void Exit()
            {
                _menuTraining.UnsubscribeShopItems(OnUnlocked);
                _menuTraining._shopUnlockTrainingScreen.SetActive(false);
            }

            private void OnUnlocked(WeaponProgress progress, WeaponUpgradeConfig config)
            {
                _menuTraining.FSM.Switch<ShopCloseState>();
                _menuTraining.UnsubscribeShopItems(OnUnlocked);
            }
        }

        private sealed class ShopCloseState : IState
        {
            private readonly MenuTraining _menu;
            public ShopCloseState(MenuTraining c) { this._menu = c; }

            public void Enter()
            {
                _menu.DisableAllTrainingScreens();
                _menu._shopScreen.SetActive(true);
                _menu._shopBackTrainingScreen.SetActive(true);
                _menu._backFromShopButton.interactable = true;
                SaveStep(MenuTrainingState.ShopClose);
                _menu._backFromShopButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                _menu._backFromShopButton.onClick.RemoveListener(OnNext);
                _menu._shopBackTrainingScreen.SetActive(false);
            }

            private void OnNext() => _menu.FSM.Switch<InventoryOpenState>();
        }

        private sealed class InventoryOpenState : IState
        {
            private readonly MenuTraining _menuTraining;
            public InventoryOpenState(MenuTraining c) { this._menuTraining = c; }

            public void Enter()
            {
                _menuTraining.DisableAllTrainingScreens();
                _menuTraining._shopScreen.SetActive(false);
                _menuTraining._inventoryOpenTrainingScreen.SetActive(true);
                _menuTraining._inventoryButton.interactable = true;
                _menuTraining._shopButton.interactable = false;
                SaveStep(MenuTrainingState.InventoryOpen);
                _menuTraining._inventoryButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                _menuTraining._inventoryButton.onClick.RemoveListener(OnNext);
                _menuTraining._inventoryOpenTrainingScreen.SetActive(false);
            }

            private void OnNext() => _menuTraining.FSM.Switch<InventoryDragState>();
        }

        private sealed class InventoryDragState : IState
        {
            private readonly MenuTraining c;
            public InventoryDragState(MenuTraining c) { this.c = c; }

            public void Enter()
            {
                c.DisableAllTrainingScreens();
                c._inventoryScreen.SetActive(true);
                c._inventoryDragTrainingScreen.SetActive(true);
                c._backFromInventoryButton.interactable = false;
                SaveStep(MenuTrainingState.InventoryDrag);
                c.SignUpPlayerInventorySlots(OnFilled);
            }

            public void Exit()
            {
                c.UnsubscribeInventorySlots(OnFilled);
                c._inventoryDragTrainingScreen.SetActive(false);
            }

            private void OnFilled()
            {
                c.FSM.Switch<InventoryCloseState>();
                c.UnsubscribeInventorySlots(OnFilled);
            }
        }

        private sealed class InventoryCloseState : IState
        {
            private readonly MenuTraining _menuTraining;
            public InventoryCloseState(MenuTraining c) { this._menuTraining = c; }

            public void Enter()
            {
                _menuTraining.DisableAllTrainingScreens();
                _menuTraining._inventoryScreen.SetActive(true);
                _menuTraining._inventoryLockerScreen.SetActive(true);
                _menuTraining._inventoryCloseTrainingScreen.SetActive(true);
                _menuTraining._backFromInventoryButton.interactable = true;
                SaveStep(MenuTrainingState.InventoryClose);
                _menuTraining._backFromInventoryButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                _menuTraining._backFromInventoryButton.onClick.RemoveListener(OnNext);
                _menuTraining._inventoryCloseTrainingScreen.SetActive(false);
            }

            private void OnNext() => _menuTraining.FSM.Switch<ChoseLevelOpenState>();
        }

        private sealed class ChoseLevelOpenState : IState
        {
            private readonly MenuTraining _menuTraining;
            public ChoseLevelOpenState(MenuTraining c) { this._menuTraining = c; }

            public void Enter()
            {
                _menuTraining.DisableAllTrainingScreens();
                _menuTraining._inventoryScreen.SetActive(false);
                _menuTraining._inventoryLockerScreen.SetActive(false);
                _menuTraining._inventoryCloseTrainingScreen.SetActive(false);
                _menuTraining._inventoryButton.interactable = false;
                _menuTraining._shopButton.interactable = false;
                _menuTraining._choseLevelButton.interactable = true;
                SaveStep(MenuTrainingState.ChoseLevelOpen);
                _menuTraining._backFromChoseLevelButton.onClick.AddListener(OnNext);
                _menuTraining._choseLevelOpenScreen.SetActive(true);
            }

            public void Exit()
            {
                _menuTraining._backFromChoseLevelButton.onClick.RemoveListener(OnNext);
                _menuTraining._choseLevelOpenScreen.SetActive(false);
            }

            private void OnNext() => _menuTraining.FSM.Switch<ChoseLevelCloseState>();
        }

        private sealed class ChoseLevelCloseState : IState
        {
            private readonly MenuTraining _menuTraining;
            public ChoseLevelCloseState(MenuTraining _menuTraining) { this._menuTraining = _menuTraining; }

            public void Enter()
            {
                _menuTraining.DisableAllTrainingScreens();
                _menuTraining._choseLevelCloseScreen.SetActive(true);
                SaveStep(MenuTrainingState.ChoseLevelClose);
                _menuTraining._choseLevelCloseButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                _menuTraining._choseLevelCloseButton.onClick.RemoveAllListeners();
                _menuTraining._choseLevelCloseScreen.SetActive(false);
            }

            private void OnNext() => _menuTraining.FSM.Switch<EndState>();
        }

        private sealed class EndState : IState
        {
            private readonly MenuTraining _menuTraining;
            public EndState(MenuTraining c) { this._menuTraining = c; }

            public void Enter()
            {
                _menuTraining.DisableAllTrainingScreens();
                _menuTraining.SetAllMainInteractable(true);
                _menuTraining.UnlockAllUpgradeButtons();
                SaveStep(MenuTrainingState.End);
                YG2.saves.IsDoneMenuTraining = true;
                YG2.SaveProgress();
            }

            public void Exit() { }
        }
    }

    public enum MenuTrainingState
    {
        Start,
        ShopOpen,
        ShopInfo,
        ShopUnlock,
        ShopClose,
        InventoryOpen,
        InventoryDrag,
        InventoryClose,
        ChoseLevelOpen,
        ChoseLevelClose,
        End
    }
}
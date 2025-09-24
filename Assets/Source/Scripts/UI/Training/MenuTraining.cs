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
                Register(new StartState(this));
                Register(new ShopOpenState(this));
                Register(new ShopInfoState(this));
                Register(new ShopUnlockState(this));
                Register(new ShopCloseState(this));
                Register(new InventoryOpenState(this));
                Register(new InventoryDragState(this));
                Register(new InventoryCloseState(this));
                Register(new ChoseLevelOpenState(this));
                Register(new ChoseLevelCloseState(this));
                Register(new EndState(this));

                Switch<StartState>();
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

        internal void SetAllMainInteractable(bool v)
        {
            _inventoryButton.interactable = v;
            _choseLevelButton.interactable = v;
            _shopButton.interactable = v;
            _startLevelButton.interactable = v;
            _rewardButton.interactable = v;
        }

        internal static void SaveStep(MenuTrainingState s)
        {
            YG2.saves.TrainingState = s;
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
            private readonly MenuTraining c;
            public StartState(MenuTraining c) { this.c = c; }

            public void Enter()
            {
                c.DisableAllTrainingScreens();
                c.SetAllMainInteractable(false);
                c._startTrainingScreen.SetActive(true);
                SaveStep(MenuTrainingState.Start);

                c._startTrainingOkButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                c._startTrainingOkButton.onClick.RemoveListener(OnNext);
                c._startTrainingScreen.SetActive(false);
            }

            private void OnNext() => c.Switch<ShopOpenState>();
        }

        private sealed class ShopOpenState : IState
        {
            private readonly MenuTraining _menu;
            public ShopOpenState(MenuTraining c) { this._menu = c; }

            public void Enter()
            {
                _menu.DisableAllTrainingScreens();
                _menu._shopOpenTrainingScreen.SetActive(true);

                _menu._shopButton.interactable = true;
                _menu._startLevelButton.interactable = false;
                _menu._inventoryButton.interactable = false;
                _menu._choseLevelButton.interactable = false;
                _menu._rewardButton.interactable = false;

                SaveStep(MenuTrainingState.ShopOpen);
                _menu._shopButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                _menu._shopButton.onClick.RemoveListener(OnNext);
                _menu._shopOpenTrainingScreen.SetActive(false);
            }

            private void OnNext() => _menu.Switch<ShopInfoState>();
        }

        private sealed class ShopInfoState : IState
        {
            private readonly MenuTraining _menu;
            public ShopInfoState(MenuTraining c) { this._menu = c; }

            public void Enter()
            {
                _menu.DisableAllTrainingScreens();
                _menu._shopScreen.SetActive(true);
                _menu._shopLockerScreen.SetActive(true);
                _menu._shopInfoTrainingScreen.SetActive(true);
                _menu._backFromShopButton.interactable = false;

                SaveStep(MenuTrainingState.ShopInfo);
                _menu._shopInfoTrainingOkButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                _menu._shopInfoTrainingOkButton.onClick.RemoveListener(OnNext);
                _menu._shopInfoTrainingScreen.SetActive(false);
            }

            private void OnNext() => _menu.Switch<ShopUnlockState>();
        }

        private sealed class ShopUnlockState : IState
        {
            private readonly MenuTraining _menu;
            public ShopUnlockState(MenuTraining c) { this._menu = c; }

            public void Enter()
            {
                _menu.DisableAllTrainingScreens();
                _menu._shopScreen.SetActive(true);
                _menu._shopLockerScreen.SetActive(false);
                _menu._shopUnlockTrainingScreen.SetActive(true);

                SaveStep(MenuTrainingState.ShopUnlock);
                _menu.SignUpShopItemsUILock(OnUnlocked);
            }

            public void Exit()
            {
                _menu.UnsubscribeShopItems(OnUnlocked);
                _menu._shopUnlockTrainingScreen.SetActive(false);
            }

            private void OnUnlocked(WeaponProgress progress, WeaponUpgradeConfig config)
            {
                _menu.Switch<ShopCloseState>();
                _menu.UnsubscribeShopItems(OnUnlocked);
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

            private void OnNext() => _menu.Switch<InventoryOpenState>();
        }

        private sealed class InventoryOpenState : IState
        {
            private readonly MenuTraining c;
            public InventoryOpenState(MenuTraining c) { this.c = c; }

            public void Enter()
            {
                c.DisableAllTrainingScreens();
                c._shopScreen.SetActive(false);
                c._inventoryOpenTrainingScreen.SetActive(true);

                c._inventoryButton.interactable = true;
                c._shopButton.interactable = false;

                SaveStep(MenuTrainingState.InventoryOpen);
                c._inventoryButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                c._inventoryButton.onClick.RemoveListener(OnNext);
                c._inventoryOpenTrainingScreen.SetActive(false);
            }

            private void OnNext() => c.Switch<InventoryDragState>();
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
                c.Switch<InventoryCloseState>();
                c.UnsubscribeInventorySlots(OnFilled);
            }
        }

        private sealed class InventoryCloseState : IState
        {
            private readonly MenuTraining c;
            public InventoryCloseState(MenuTraining c) { this.c = c; }

            public void Enter()
            {
                c.DisableAllTrainingScreens();
                c._inventoryScreen.SetActive(true);
                c._inventoryLockerScreen.SetActive(true);
                c._inventoryCloseTrainingScreen.SetActive(true);
                c._backFromInventoryButton.interactable = true;

                SaveStep(MenuTrainingState.InventoryClose);
                c._backFromInventoryButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                c._backFromInventoryButton.onClick.RemoveListener(OnNext);
                c._inventoryCloseTrainingScreen.SetActive(false);
            }

            private void OnNext() => c.Switch<ChoseLevelOpenState>();
        }

        private sealed class ChoseLevelOpenState : IState
        {
            private readonly MenuTraining c;
            public ChoseLevelOpenState(MenuTraining c) { this.c = c; }

            public void Enter()
            {
                c.DisableAllTrainingScreens();

                c._inventoryScreen.SetActive(false);
                c._inventoryLockerScreen.SetActive(false);
                c._inventoryCloseTrainingScreen.SetActive(false);

                c._inventoryButton.interactable = false;
                c._shopButton.interactable = false;
                c._choseLevelButton.interactable = true;

                SaveStep(MenuTrainingState.ChoseLevelOpen);
                c._backFromChoseLevelButton.onClick.AddListener(OnNext);
                c._choseLevelOpenScreen.SetActive(true);
            }

            public void Exit()
            {
                c._backFromChoseLevelButton.onClick.RemoveListener(OnNext);
                c._choseLevelOpenScreen.SetActive(false);
            }

            private void OnNext() => c.Switch<ChoseLevelCloseState>();
        }

        private sealed class ChoseLevelCloseState : IState
        {
            private readonly MenuTraining c;
            public ChoseLevelCloseState(MenuTraining c) { this.c = c; }

            public void Enter()
            {
                c.DisableAllTrainingScreens();
                c._choseLevelCloseScreen.SetActive(true);

                SaveStep(MenuTrainingState.ChoseLevelClose);
                c._choseLevelCloseButton.onClick.AddListener(OnNext);
            }

            public void Exit()
            {
                c._choseLevelCloseButton.onClick.RemoveAllListeners();
                c._choseLevelCloseScreen.SetActive(false);
            }

            private void OnNext() => c.Switch<EndState>();
        }

        private sealed class EndState : IState
        {
            private readonly MenuTraining c;
            public EndState(MenuTraining c) { this.c = c; }

            public void Enter()
            {
                c.DisableAllTrainingScreens();
                c.SetAllMainInteractable(true);
                c.UnlockAllUpgradeButtons();

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

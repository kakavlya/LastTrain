using System;
using System.Collections.Generic;
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
        private readonly Dictionary<MenuTrainingState, Action> _switchers = new();
        
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

        public GameObject StartTrainingScreen => _startTrainingScreen;

        public GameObject ShopOpenTrainingScreen => _shopOpenTrainingScreen;

        public GameObject ShopInfoTrainingScreen => _shopInfoTrainingScreen;

        public GameObject ShopUnlockTrainingScreen => _shopUnlockTrainingScreen;

        public GameObject ShopBackTrainingScreen => _shopBackTrainingScreen;

        public GameObject InventoryOpenTrainingScreen => _inventoryOpenTrainingScreen;

        public GameObject InventoryDragTrainingScreen => _inventoryDragTrainingScreen;

        public GameObject InventoryCloseTrainingScreen => _inventoryCloseTrainingScreen;

        public GameObject ChoseLevelOpenScreen => _choseLevelOpenScreen;

        public GameObject ChoseLevelCloseScreen => _choseLevelCloseScreen;

        public GameObject ShopScreen => _shopScreen;

        public GameObject InventoryScreen => _inventoryScreen;

        public Button StartLevelButton => _startLevelButton;

        public Button InventoryButton => _inventoryButton;

        public Button ChoseLevelButton => _choseLevelButton;

        public Button ShopButton => _shopButton;

        public Button BackFromShopButton => _backFromShopButton;

        public Button BackFromInventoryButton => _backFromInventoryButton;

        public Button BackFromChoseLevelButton => _backFromChoseLevelButton;

        public Button RewardButton => _rewardButton;

        public Button StartTrainingOkButton => _startTrainingOkButton;

        public Button ShopInfoTrainingOkButton => _shopInfoTrainingOkButton;

        public Button ChoseLevelCloseButton => _choseLevelCloseButton;

        public GameObject ShopLockerScreen => _shopLockerScreen;

        public GameObject InventoryLockerScreen => _inventoryLockerScreen;

        private void Start()
        {
            if (TrainingHandler.Instance.IsDoneGameplayTraining &&
                !TrainingHandler.Instance.IsDoneMenuTraining)
            {
                Register(MenuTrainingState.Start, new StartStateMenu(this));
                Register(MenuTrainingState.ShopOpen, new ShopOpenState(this));
                Register(MenuTrainingState.ShopInfo, new ShopInfoState(this));
                Register(MenuTrainingState.ShopUnlock, new ShopUnlockState(this));
                Register(MenuTrainingState.ShopClose, new ShopCloseState(this));
                Register(MenuTrainingState.InventoryOpen, new InventoryOpenState(this));
                Register(MenuTrainingState.InventoryDrag, new InventoryDragState(this));
                Register(MenuTrainingState.InventoryClose, new InventoryCloseState(this));
                Register(MenuTrainingState.ChoseLevelOpen, new ChoseLevelOpenState(this));
                Register(MenuTrainingState.ChoseLevelClose, new ChoseLevelCloseState(this));
                Register(MenuTrainingState.End, new EndState(this));

                DisableAllTrainingScreens();
                SwitchToSavedOrStart();
            }
        }

        private void Register<T>(MenuTrainingState key, T state) where T : class, IState
        {
            FSM.Register(state);
            _switchers[key] = () => FSM.Switch<T>();
        }

        private void SwitchToSavedOrStart()
        {
            var key = YG2.saves.TrainingState;
            if (!_switchers.TryGetValue(key, out var go))
                go = () => FSM.Switch<StartStateMenu>();

            go();
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

        public void SaveStep(MenuTrainingState state)
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
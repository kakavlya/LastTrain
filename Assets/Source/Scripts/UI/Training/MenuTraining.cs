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

        public sealed class Intro { }
        public sealed class ShopOpen { }
        public sealed class ShopInfo { }
        public sealed class ShopUnlock { }
        public sealed class ShopClose { }
        public sealed class InventoryOpen { }
        public sealed class InventoryDrag { }
        public sealed class InventoryClose { }
        public sealed class ChoseLevelOpen { }
        public sealed class ChoseLevelClose { }
        public sealed class End { }

        private void Start()
        {
            if (TrainingHandler.Instance.IsDoneGameplayTraining == true &&
                TrainingHandler.Instance.IsDoneMenuTraining == false)
            {
                RegisterStates();
                Switch<Intro>();
            }
        }

        private void RegisterStates()
        {
            Register<Intro>(new ActionState(
                enter: () =>
                {
                    DisableAllTrainingScreens();
                    SetAllMainInteractable(false);
                    _startTrainingScreen.SetActive(true);

                    SaveState(MenuTrainingState.Start);

                    _startTrainingOkButton.onClick.AddListener(OnStartTrainingButton);
                },
                exit: () =>
                {
                    _startTrainingOkButton.onClick.RemoveListener(OnStartTrainingButton);
                    _startTrainingScreen.SetActive(false);
                }));

            Register<ShopOpen>(new ActionState(
                enter: () =>
                {
                    DisableAllTrainingScreens();
                    _shopOpenTrainingScreen.SetActive(true);

                    _shopButton.interactable = true;
                    _startLevelButton.interactable = false;
                    _inventoryButton.interactable = false;
                    _choseLevelButton.interactable = false;
                    _rewardButton.interactable = false;

                    SaveState(MenuTrainingState.ShopOpen);

                    _shopButton.onClick.AddListener(OnShopOpenTrainingButton);
                },
                exit: () =>
                {
                    _shopButton.onClick.RemoveListener(OnShopOpenTrainingButton);
                    _shopOpenTrainingScreen.SetActive(false);
                }));

            Register<ShopInfo>(new ActionState(
                enter: () =>
                {
                    DisableAllTrainingScreens();
                    _shopScreen.SetActive(true);
                    _shopLockerScreen.SetActive(true);
                    _shopInfoTrainingScreen.SetActive(true);
                    _backFromShopButton.interactable = false;

                    SaveState(MenuTrainingState.ShopInfo);

                    _shopInfoTrainingOkButton.onClick.AddListener(OnShopInfoTrainingButton);
                },
                exit: () =>
                {
                    _shopInfoTrainingOkButton.onClick.RemoveListener(OnShopInfoTrainingButton);
                    _shopInfoTrainingScreen.SetActive(false);
                }));

            Register<ShopUnlock>(new ActionState(
                enter: () =>
                {
                    DisableAllTrainingScreens();
                    _shopScreen.SetActive(true);
                    _shopLockerScreen.SetActive(false);
                    _shopUnlockTrainingScreen.SetActive(true);

                    SaveState(MenuTrainingState.ShopUnlock);

                    SignUpShopItemsUILock(); 
                },
                exit: () =>
                {
                    UnsubscribeShopItems();
                    _shopUnlockTrainingScreen.SetActive(false);
                }));

            Register<ShopClose>(new ActionState(
                enter: () =>
                {
                    DisableAllTrainingScreens();
                    _shopScreen.SetActive(true);
                    _shopBackTrainingScreen.SetActive(true);
                    _backFromShopButton.interactable = true;

                    SaveState(MenuTrainingState.ShopClose);

                    _backFromShopButton.onClick.AddListener(OnShopBackButton);
                },
                exit: () =>
                {
                    _backFromShopButton.onClick.RemoveListener(OnShopBackButton);
                    _shopBackTrainingScreen.SetActive(false);
                }));

            Register<InventoryOpen>(new ActionState(
                enter: () =>
                {
                    DisableAllTrainingScreens();
                    _shopScreen.SetActive(false);

                    _inventoryOpenTrainingScreen.SetActive(true);
                    _inventoryButton.interactable = true;
                    _shopButton.interactable = false;

                    SaveState(MenuTrainingState.InventoryOpen);

                    _inventoryButton.onClick.AddListener(OnInventoryOpen);
                },
                exit: () =>
                {
                    _inventoryButton.onClick.RemoveListener(OnInventoryOpen);
                    _inventoryOpenTrainingScreen.SetActive(false);
                }));

            Register<InventoryDrag>(new ActionState(
                enter: () =>
                {
                    DisableAllTrainingScreens();
                    _inventoryScreen.SetActive(true);
                    _inventoryDragTrainingScreen.SetActive(true);
                    _backFromInventoryButton.interactable = false;

                    SaveState(MenuTrainingState.InventoryDrag);

                    SignUpPlayerInventorySlots();
                },
                exit: () =>
                {
                    UnsubscribeInventorySlots();
                    _inventoryDragTrainingScreen.SetActive(false);
                }));

            Register<InventoryClose>(new ActionState(
                enter: () =>
                {
                    DisableAllTrainingScreens();
                    _inventoryScreen.SetActive(true);
                    _inventoryLockerScreen.SetActive(true);
                    _inventoryCloseTrainingScreen.SetActive(true);
                    _backFromInventoryButton.interactable = true;

                    SaveState(MenuTrainingState.InventoryClose);

                    _backFromInventoryButton.onClick.AddListener(OnInventoryClose);
                },
                exit: () =>
                {
                    _backFromInventoryButton.onClick.RemoveListener(OnInventoryClose);
                    _inventoryCloseTrainingScreen.SetActive(false);
                }));

            Register<ChoseLevelOpen>(new ActionState(
                enter: () =>
                {
                    DisableAllTrainingScreens();

                    _inventoryScreen.SetActive(false);
                    _inventoryLockerScreen.SetActive(false);
                    _inventoryCloseTrainingScreen.SetActive(false);

                    _inventoryButton.interactable = false;
                    _shopButton.interactable = false;
                    _choseLevelButton.interactable = true;

                    SaveState(MenuTrainingState.ChoseLevelOpen);

                    _backFromChoseLevelButton.onClick.AddListener(OnChoseLevelOpen);
                    _choseLevelOpenScreen.SetActive(true);
                },
                exit: () =>
                {
                    _backFromChoseLevelButton.onClick.RemoveListener(OnChoseLevelOpen);
                    _choseLevelOpenScreen.SetActive(false);
                }));


            Register<ChoseLevelClose>(new ActionState(
                enter: () =>
                {
                    DisableAllTrainingScreens();

                    SaveState(MenuTrainingState.ChoseLevelClose);

                    _choseLevelCloseScreen.SetActive(true);
                    _choseLevelCloseButton.onClick.AddListener(OnChoseLevelClose);
                },
                exit: () =>
                {
                    _choseLevelCloseButton.onClick.RemoveListener(OnChoseLevelClose);
                    _choseLevelCloseScreen.SetActive(false);
                }));

            Register<End>(new ActionState(
                enter: () =>
                {
                    DisableAllTrainingScreens();
                    SetAllMainInteractable(true);
                    UnlockAllUpgradeButtons();

                    SaveState(MenuTrainingState.End);

                    YG2.saves.IsDoneMenuTraining = true;
                    YG2.SaveProgress();
                },
                exit: () => { }));
        }

        private void OnStartTrainingButton() => Switch<ShopOpen>();
        private void OnShopOpenTrainingButton() => Switch<ShopInfo>();
        private void OnShopInfoTrainingButton() => Switch<ShopUnlock>();
        private void OnShopBackButton() => Switch<InventoryOpen>();
        private void OnInventoryOpen() => Switch<InventoryDrag>();
        private void OnInventoryClose() => Switch<ChoseLevelOpen>();
        private void OnChoseLevelOpen() => Switch<ChoseLevelClose>();
        private void OnChoseLevelClose() => Switch<End>();

        private void OnShopUnlockTraining(WeaponProgress _, WeaponUpgradeConfig __)
        {
            Switch<ShopClose>();
            UnsubscribeShopItems();
        }

        private void OnInventoryDrag()
        {
            Switch<InventoryClose>();
            UnsubscribeInventorySlots();
        }

        private void DisableAllTrainingScreens()
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

        private void SetAllMainInteractable(bool value)
        {
            _inventoryButton.interactable = value;
            _choseLevelButton.interactable = value;
            _shopButton.interactable = value;
            _startLevelButton.interactable = value;
            _rewardButton.interactable = value;
        }

        private void SignUpShopItemsUILock()
        {
            _shopItems = _shopContent.GetComponentsInChildren<ShopItemUI>(true);
            foreach (var item in _shopItems)
            {
                item.WeaponUnlocked += OnShopUnlockTraining;

                var upgradeButton = item.GetComponentInChildren<Button>(true);
                if (upgradeButton != null)
                    upgradeButton.interactable = false;
            }
        }

        private void UnsubscribeShopItems()
        {
            if (_shopItems == null)
                return;

            foreach (var item in _shopItems)
                item.WeaponUnlocked -= OnShopUnlockTraining;
        }

        private void UnlockAllUpgradeButtons()
        {
            _shopItems = _shopContent.GetComponentsInChildren<ShopItemUI>(true);

            foreach (var item in _shopItems)
            {
                var upgradeButton = item.GetComponentInChildren<Button>(true);
                if (upgradeButton != null)
                    upgradeButton.interactable = true;
            }
        }

        private void SignUpPlayerInventorySlots()
        {
            _weaponSlots = _playerInventory.GetComponentsInChildren<WeaponSlotUI>(true);

            foreach (var slot in _weaponSlots)
                slot.Filled += OnInventoryDrag;
        }

        private void UnsubscribeInventorySlots()
        {
            if (_weaponSlots == null) 
                return;

            foreach (var slot in _weaponSlots)
                slot.Filled -= OnInventoryDrag;
        }

        private static void SaveState(MenuTrainingState state)
        {
            YG2.saves.TrainingState = state;
            YG2.SaveProgress();
        }

        private sealed class ActionState : IState
        {
            private readonly Action _enter, _exit;
            public ActionState(Action enter, Action exit) { _enter = enter; _exit = exit; }
            public void Enter() => _enter?.Invoke();
            public void Exit() => _exit?.Invoke();
        }
    }

    //enum for YG2 saves
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
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MenuTraining : MonoBehaviour
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
        if (TrainingHandler.Instance.IsDoneGameplayTraining == true && TrainingHandler.Instance.IsDoneMenuTraining == false)
        {
            SwitchTrainingWindows(YG2.saves.TrainingState);
            _startTrainingOkButton.onClick.AddListener(OnStartTrainingButton);
            _shopInfoTrainingOkButton.onClick.AddListener(OnShopInfoTrainingButton);
        }
    }

    private void SwitchTrainingWindows(MenuTrainingState state)
    {
        DisableAllTrainingScreens();
        _startLevelButton.interactable = false;

        switch (state)
        {
            case MenuTrainingState.Start:
                _inventoryButton.interactable = false;
                _choseLevelButton.interactable = false;
                _shopButton.interactable = false;
                _rewardButton.interactable = false;
                _startTrainingScreen.SetActive(true);
                break;

            case MenuTrainingState.ShopOpen:
                _shopButton.interactable = true;
                _shopOpenTrainingScreen.SetActive(true);
                _shopButton.onClick.AddListener(OnShopOpenTrainingButton);
                break;

            case MenuTrainingState.ShopInfo:
                _shopScreen.SetActive(true);
                _shopLockerScreen.SetActive(true);
                _shopInfoTrainingScreen.SetActive(true);
                _backFromShopButton.interactable = false;
                _shopButton.onClick.RemoveListener(OnShopOpenTrainingButton);
                break;

            case MenuTrainingState.ShopUnlock:
                _shopScreen.SetActive(true);
                _shopLockerScreen.SetActive(false);
                _shopUnlockTrainingScreen.SetActive(true);
                SignUpShopItemsUILock();
                break;

            case MenuTrainingState.ShopClose:
                _shopScreen.SetActive(true);
                _shopBackTrainingScreen.SetActive(true);
                _backFromShopButton.interactable = true;
                _backFromShopButton.onClick.AddListener(OnShopBackButton);
                break;

            case MenuTrainingState.InventoryOpen:
                _shopScreen.SetActive(false);
                _inventoryOpenTrainingScreen.SetActive(true);
                _inventoryButton.interactable = true;
                _inventoryButton.onClick.AddListener(OnInventoryOpen);
                _backFromShopButton.onClick.RemoveListener(OnShopBackButton);
                _shopButton.interactable = false;
                break;

            case MenuTrainingState.InventoryDrag:
                _inventoryScreen.SetActive(true);
                _backFromInventoryButton.interactable = false;
                _inventoryDragTrainingScreen.SetActive(true);
                _inventoryButton.onClick.RemoveListener(OnInventoryOpen);
                SignUpPlayerInventorySlots();
                break;

            case MenuTrainingState.InventoryClose:
                _inventoryScreen.SetActive(true);
                _inventoryLockerScreen.SetActive(true);
                _inventoryCloseTrainingScreen.SetActive(true);
                _backFromInventoryButton.interactable = true;
                _backFromInventoryButton.onClick.AddListener(OnInventoryClose);
                break;

            case MenuTrainingState.ChoseLevelOpen:
                _inventoryScreen.SetActive(false);
                _inventoryLockerScreen.SetActive(false);
                _inventoryCloseTrainingScreen.SetActive(false);
                _inventoryButton.interactable = false;
                _choseLevelButton.interactable = true;
                _shopButton.interactable = false;
                _backFromChoseLevelButton.onClick.AddListener(OnChoseLevelOpen);
                _choseLevelOpenScreen.SetActive(true);
                break;

            case MenuTrainingState.ChoseLevelClose:
                _choseLevelButton.onClick.RemoveListener(OnChoseLevelOpen);
                _choseLevelCloseScreen.SetActive(true);
                _choseLevelCloseButton.onClick.AddListener(OnChoseLevelClose);
                break;

            case MenuTrainingState.End:
                _inventoryButton.interactable = true;
                _shopButton.interactable = true;
                _startLevelButton.interactable = true;
                _rewardButton.interactable = true;
                _choseLevelCloseButton.onClick.RemoveListener(OnChoseLevelClose);
                UnlockAllUpgradeButtons();
                YG2.saves.IsDoneMenuTraining = true;
                break;
        }

        YG2.saves.TrainingState = state;
        YG2.SaveProgress();
    }

    private void OnStartTrainingButton()
    {
        SwitchTrainingWindows(MenuTrainingState.ShopOpen);
    }

    private void OnShopOpenTrainingButton()
    {
        SwitchTrainingWindows(MenuTrainingState.ShopInfo);
    }

    private void OnShopInfoTrainingButton()
    {
        SwitchTrainingWindows(MenuTrainingState.ShopUnlock);
    }

    private void OnShopUnlockTraining(WeaponProgress weaponProgress, WeaponUpgradeConfig weaponUpgradeConfig)
    {
        SwitchTrainingWindows(MenuTrainingState.ShopClose);

        foreach (var shopItem in _shopItems)
        {
            shopItem.WeaponUnlocked -= OnShopUnlockTraining;
        }
    }

    private void OnShopBackButton()
    {
        SwitchTrainingWindows(MenuTrainingState.InventoryOpen);
    }

    private void OnInventoryOpen()
    {
        SwitchTrainingWindows(MenuTrainingState.InventoryDrag);
    }

    private void OnInventoryDrag()
    {
        SwitchTrainingWindows(MenuTrainingState.InventoryClose);

        foreach (var weaponSlot in _weaponSlots)
        {
            weaponSlot.Filled -= OnInventoryDrag;
        }
    }

    private void OnInventoryClose()
    {
        SwitchTrainingWindows(MenuTrainingState.ChoseLevelOpen);
    }

    private void OnChoseLevelOpen()
    {
        SwitchTrainingWindows(MenuTrainingState.ChoseLevelClose);
    }

    private void OnChoseLevelClose()
    {
        SwitchTrainingWindows(MenuTrainingState.End);
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

    private void SignUpShopItemsUILock()
    {
        _shopItems = _shopContent.GetComponentsInChildren<ShopItemUI>();

        foreach (var shopItem in _shopItems)
        {
            shopItem.WeaponUnlocked += OnShopUnlockTraining;

            var upgradeButton = shopItem.GetComponentInChildren<Button>(true);

            if (upgradeButton != null)
                upgradeButton.interactable = false;
        }
    }

    private void UnlockAllUpgradeButtons()
    {
        _shopItems = _shopContent.GetComponentsInChildren<ShopItemUI>();

        foreach (var shopItem in _shopItems)
        {
            var upgradeButton = shopItem.GetComponentInChildren<Button>(true);

            if (upgradeButton != null)
                upgradeButton.interactable = true;
        }
    }

    private void SignUpPlayerInventorySlots()
    {
        _weaponSlots = _playerInventory.GetComponentsInChildren<WeaponSlotUI>();

        foreach (var weaponSlot in _weaponSlots)
        {
            weaponSlot.Filled += OnInventoryDrag;
        }
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

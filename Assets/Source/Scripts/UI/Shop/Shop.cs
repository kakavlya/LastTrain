using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    [Header("Weapons List")]
    [SerializeField] private Transform _contentParent;
    [SerializeField] private ShopItemUI _shopItemPrefab;
    [SerializeField] private UpgradeConfig[] _intemConfigs;

    [Header("General shop UI")]
    [SerializeField] private Image _blocker;
    [SerializeField] private DetailsPanel _detailsPrefab;
    [SerializeField] private Transform _detailsParent;
    [SerializeField] private ScrollRect _scrollRect;

    [Header("Inventory items")]
    [SerializeField] private InventoryWeapon _inventoryWeaponPrefab;
    [SerializeField] private InventoryHandler _inventoryHandler;

    private DetailsPanel _detailsPanel;
    private ProgressData _data;
    private List<ShopItemUI> _uiItems = new List<ShopItemUI>();

    public event Action SlotIncremented;

    private void Start()
    {
        _blocker.gameObject.SetActive(false);
        _data = SaveManager.Instance.Data;
        BuildShop();
    }

    private void BuildShop()
    {
        foreach (var item in _uiItems)
        {
            item.WeaponUnlocked -= InitialNewInventoryWeapon;
        }

        _uiItems.Clear();

        foreach (Transform child in _contentParent)
            Destroy(child.gameObject);

        var data = SaveManager.Instance.Data;

        foreach (var upgradeConfig in _intemConfigs)
        {
            BaseProgress progress = null;

            if (upgradeConfig is WeaponUpgradeConfig weaponUprgadeCfg)
            {
                string id = weaponUprgadeCfg.WeaponId;
                progress = _data.WeaponsProgress.Find(w => w.WeaponId == id);

                if (progress == null)
                {
                    progress = new WeaponProgress(id);
                    data.WeaponsProgress.Add((WeaponProgress)progress);
                }
            }
            else if (upgradeConfig is TrainUpgradeConfig trainUpgradeConfig)
            {
                progress = _data.TrainProgress;
            }


            var itemUi = Instantiate(_shopItemPrefab, _contentParent);
            _uiItems.Add(itemUi);
            itemUi.Init(upgradeConfig, progress, OnItemSelected);
            itemUi.WeaponUnlocked += InitialNewInventoryWeapon;
        }

        SaveManager.Instance.Save();
        StartCoroutine(ResizeAndScrollToTop());
    }

    private void OnDisable()
    {
        foreach (var item in _uiItems)
        {
            item.WeaponUnlocked -= InitialNewInventoryWeapon;
        }
    }


    private void OnItemSelected(UpgradeConfig cfg, BaseProgress prog)
    {
        if (_detailsPanel == null)
        {
            _detailsPanel = Instantiate(_detailsPrefab, _detailsParent);
            _detailsPanel.Incremented += OnStatIncremented;
        }

        _blocker.gameObject.SetActive(true);
        _detailsPanel.Show(cfg, prog, OnDetailsClosed);
    }

    private void OnDetailsClosed()
    {
        _detailsPanel.Incremented -= OnStatIncremented;
        _blocker.gameObject.SetActive(false);
        BuildShop();
    }

    private void OnStatIncremented(StatType stat)
    {
        if (stat == StatType.Slots)
        {
            SlotIncremented?.Invoke();
        }
    }

    private void InitialNewInventoryWeapon(WeaponProgress progress, WeaponUpgradeConfig weaponConfig)
    {
        progress.IsAvailable = true;
        SaveManager.Instance.Data.InventorySlots.Add(weaponConfig.WeaponId);
        _inventoryHandler.SubmitActiveSlots();

        WeaponSlotUI lastSlot = _inventoryHandler.GetLastActiveSlotUIs();

        if (lastSlot != null && lastSlot.GetComponentInChildren<InventoryWeapon>() == null)
        {
            InventoryWeapon inventoryWeapon = Instantiate(_inventoryWeaponPrefab, lastSlot.transform);
            inventoryWeapon.Init(weaponConfig);
            lastSlot.SetSlotFilled();
        }

        SaveManager.Instance.Save();
    }

    private void ResizeContentForGrid()
    {
        var layout = _contentParent.GetComponent<GridLayoutGroup>();
        var contentRect = _contentParent.GetComponent<RectTransform>();

        int totalItems = _contentParent.childCount;
        int columns = Mathf.Max(1, Mathf.FloorToInt((contentRect.rect.width + layout.spacing.x) / (layout.cellSize.x + layout.spacing.x)));

        int rows = Mathf.CeilToInt((float)totalItems / columns);

        float height = rows * layout.cellSize.y + layout.spacing.y * (rows - 1) + layout.padding.top + layout.padding.bottom;

        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    private IEnumerator ResizeAndScrollToTop()
    {
        yield return null;
        ResizeContentForGrid();
        yield return null;
        _scrollRect.verticalNormalizedPosition = 1f;
    }
}

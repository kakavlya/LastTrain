using LastTrain.Inventory;
using LastTrain.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace LastTrain.ShopSystem
{
    public class Shop : MonoBehaviour
    {
        [Header("Weapons List")]
        [SerializeField] private Transform _contentParent;
        [SerializeField] private ShopItemUI _shopItemPrefab;
        [SerializeField] private UpgradeConfig[] _itemConfigs;

        [Header("General shop UI")]
        [SerializeField] private Image _blocker;
        [SerializeField] private DetailsPanel _detailsPrefab;
        [SerializeField] private Transform _detailsParent;
        [SerializeField] private ScrollRect _scrollRect;

        [Header("Inventory items")]
        [SerializeField] private InventoryWeapon _inventoryWeaponPrefab;
        [SerializeField] private InventoryHandler _inventoryHandler;

        private DetailsPanel _detailsPanel;
        private SavesYG _data;
        private List<ShopItemUI> _uiItems = new List<ShopItemUI>();

        public event Action SlotIncremented;

        private void Start()
        {
            _blocker.gameObject.SetActive(false);
            _data = YG2.saves;
            BuildShop();
        }

        private void OnDisable()
        {
            foreach (var item in _uiItems)
            {
                item.WeaponUnlocked -= InitialNewInventoryWeapon;
                item.TurretUnlocked -= InitialNewTurret;
            }
        }

        private void OnDestroy()
        {
            if (_detailsPanel != null)
                _detailsPanel.Incremented -= OnStatIncremented;
        }

        private void BuildShop()
        {
            foreach (var item in _uiItems)
            {
                item.WeaponUnlocked -= InitialNewInventoryWeapon;
                item.TurretUnlocked -= InitialNewTurret;
            }

            _uiItems.Clear();

            foreach (Transform child in _contentParent)
                Destroy(child.gameObject);

            var data = YG2.saves;

            foreach (var upgradeConfig in _itemConfigs)
            {
                // This panel is Weapons-only.
                // Train is handled by TrainShopPanel.
                // Turrets are handled by HardpointsShopPanel (coming in Step 2).
                if (upgradeConfig is not WeaponUpgradeConfig weaponUpgradeCfg)
                    continue;

                string id = weaponUpgradeCfg.WeaponId;
                BaseProgress progress = _data.WeaponsProgress.Find(w => w.WeaponId == id);

                if (progress == null)
                {
                    progress = new WeaponProgress(id);
                    data.WeaponsProgress.Add((WeaponProgress)progress);
                }

                var itemUi = Instantiate(_shopItemPrefab, _contentParent);
                _uiItems.Add(itemUi);
                itemUi.Init(upgradeConfig, progress, OnItemSelected);
                itemUi.WeaponUnlocked += InitialNewInventoryWeapon;
            }

            YG2.SaveProgress();
            StartCoroutine(ResizeAndScrollToTop());
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
            progress.SetAvailable(true);
            YG2.saves.InventorySlots.Add(weaponConfig.WeaponId);
            _inventoryHandler.SubmitActiveSlots();

            WeaponSlotUI lastSlot = _inventoryHandler.GetLastActiveSlotUIs();

            if (lastSlot != null && lastSlot.GetComponentInChildren<InventoryWeapon>() == null)
            {
                InventoryWeapon inventoryWeapon = Instantiate(_inventoryWeaponPrefab, lastSlot.transform);
                inventoryWeapon.Init(weaponConfig);
                lastSlot.SetSlotFilled(inventoryWeapon);
            }

            YG2.SaveProgress();
        }

        private void InitialNewTurret(TurretProgress progress, TurretUpgradeConfig turretConfig)
        {
            progress.SetUnlocked(true);
            YG2.SaveProgress();
            
            // Note: Unlike weapons, turrets don't have inventory slots.
            // They are automatically equipped to the train's hardpoints based on progress.IsUnlocked.
        }

        private void ResizeContentForGrid()
        {
            var layout = _contentParent.GetComponent<GridLayoutGroup>();
            var contentRect = _contentParent.GetComponent<RectTransform>();
            int totalItems = _contentParent.childCount;
            int columns = Mathf.Max(1, Mathf.FloorToInt((contentRect.rect.width + layout.spacing.x) / (layout.cellSize.x + layout.spacing.x)));
            int rows = Mathf.CeilToInt((float)totalItems / columns);
            float height = (rows * layout.cellSize.y) + (layout.spacing.y * (rows - 1)) + layout.padding.top + layout.padding.bottom;
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
}

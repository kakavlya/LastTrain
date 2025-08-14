using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Weapons List")]
    [SerializeField] private Transform _contentParent;
    [SerializeField] private ShopItemUI _shopItemPrefab;
    [SerializeField] private WeaponUpgradeConfig[] _weaponConfigs;

    [Header("General shop UI")]
    [SerializeField] private Image _blocker;              
    [SerializeField] private WeaponDetailsPanel _detailsPrefab;
    [SerializeField] private Transform _detailsParent;        // spawn position

    [Header("Inventory items")]
    [SerializeField] private InventoryWeapon _inventoryWeaponPrefab;

    // Кэш, чтобы не пересоздавать окно каждый раз
    private WeaponDetailsPanel _detailsPanel;
    ProgressData _data;

    private void Start()
    {
        _blocker.gameObject.SetActive(false);
        _data = SaveManager.Instance.Data;
        BuildShop();
    }

    private void BuildShop()
    {
        foreach (Transform child in _contentParent)
            Destroy(child.gameObject);

        var data = SaveManager.Instance.Data;

        foreach (var upgradeConfig in _weaponConfigs)
        {

            string id = upgradeConfig.WeaponId;                           
            var progress = _data.Weapons.Find(w => w.WeaponId == id);
            if (progress == null)
            {
                progress = new WeaponProgress(id);
                data.Weapons.Add(progress);
            }

            var itemUi = Instantiate(_shopItemPrefab, _contentParent);
            itemUi.Init(upgradeConfig, progress, OnItemSelected);
            itemUi.Unlocked += InitialNewInventoryWeapon;
        }

        SaveManager.Instance.Save();
    }


    private void OnItemSelected(WeaponUpgradeConfig cfg, WeaponProgress prog)
    {
        if (_detailsPanel == null)
            _detailsPanel = Instantiate(_detailsPrefab, _detailsParent);

        _blocker.gameObject.SetActive(true);       

        _detailsPanel.Show(cfg, prog, OnDetailsClosed);
    }

    private void OnDetailsClosed()
    {
        _blocker.gameObject.SetActive(false);
        BuildShop();
    }

    private void InitialNewInventoryWeapon()
    {

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private Transform _contentParent;
    [SerializeField] private ShopItemUI _shopItemPrefab;
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private WeaponUpgradeConfig[] _weaponConfigs;

    private void Start()
    {
        BuildShop();
    }
    private void OnEnable()
    {
        RefreshCoins();
    }

    private void BuildShop()
    {
        foreach(Transform t in _contentParent)
        {
            Destroy(t.gameObject);
        }

        var data = SaveManager.Instance.Data;
        foreach (var config in _weaponConfigs)
        {
            var weapnData = data.Weapons.Find(w => w.WeaponId == config.WeaponId);

            int savedLevel = weapnData != null ? weapnData.UpgradeLevel : 0;

            var itemUI = Instantiate(_shopItemPrefab, _contentParent);
            itemUI.Init(config, savedLevel);
        }
    }


    public void RefreshCoins()
    {
        _coinsText.text = $"Coins: {SaveManager.Instance.Data.Coins}";
    }
}

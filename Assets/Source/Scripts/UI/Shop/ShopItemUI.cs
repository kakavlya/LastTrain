using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _weaponName;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _unlockCostText;
    [SerializeField] private Button _unlockButton;
    [SerializeField] private GameObject _lockPanel;
    [SerializeField] private Button _upgradeButton;

    private int _unlockingCost;
    private WeaponUpgradeConfig _upgradeConfig;
    private WeaponProgress _progress;
    private Action<WeaponUpgradeConfig, WeaponProgress> _onSelected;
    private bool _isAvailable;

    public event Action<WeaponProgress, WeaponUpgradeConfig> Unlocked;

    public void Init(WeaponUpgradeConfig cfg,
                     WeaponProgress progress,
                     Action<WeaponUpgradeConfig, WeaponProgress> onSelected)
    {
        _upgradeConfig = cfg;
        _progress = progress;
        _onSelected = onSelected;
        _unlockingCost = cfg.UnblockingCost;
        _unlockCostText.text = _unlockingCost.ToString();

        _icon.sprite = cfg.Icon;
        _isAvailable = progress.IsAvailable;

        _upgradeButton.onClick.AddListener(OnUpgradeButtonClick);

        if (!_isAvailable)
        {
            _unlockButton.gameObject.SetActive(true);
            _unlockButton.onClick.AddListener(BuyItem);
            _lockPanel.SetActive(true);
            _upgradeButton.gameObject.SetActive(false);
            
        }
        else
        {
            _unlockButton.gameObject.SetActive(false);
            _lockPanel.SetActive(false);
            _upgradeButton.gameObject.SetActive(true);
        }

        Refresh();
    }

    private void OnUpgradeButtonClick()
    {
        if (_lockPanel.activeSelf)
        {
            return;
        }

        _onSelected?.Invoke(_upgradeConfig, _progress);
    }

    public void Refresh()
    {
        UpdateTextLabels();
    }

    private void UpdateTextLabels()
    {
        int sumLevel = _progress.DamageLevel + _progress.RangeLevel;

        _levelText.text = sumLevel.ToString();
        _weaponName.text = _upgradeConfig.WeaponName;
    }

    private void BuyItem()
    {
        if (CoinsHandler.Instance.CoinsCount >= _unlockingCost)
        {
            _unlockButton.onClick.RemoveListener(BuyItem);
            CoinsHandler.Instance.RemoveCoins(_unlockingCost);
            _unlockButton.gameObject.SetActive(false);
            _lockPanel.SetActive(false);
            _upgradeButton.gameObject.SetActive(true);
            Unlocked?.Invoke(_progress,_upgradeConfig);
        }
    }
}

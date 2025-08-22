using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    private UpgradeConfig _upgradeConfig;
    private BaseProgress _progress;
    private Action<UpgradeConfig, BaseProgress> _onSelected;
    private bool _isAvailable;

    public event Action<WeaponProgress, WeaponUpgradeConfig> WeaponUnlocked;

    public void Init(UpgradeConfig cfg,
                     BaseProgress progress,
                     Action<UpgradeConfig, BaseProgress> onSelected)
    {
        _upgradeConfig = cfg;
        _progress = progress;
        _onSelected = onSelected;

        _icon.sprite = cfg.Icon;
        _weaponName.text = cfg.Name;
        _upgradeButton.onClick.AddListener(OnUpgradeButtonClick);

        if (cfg is WeaponUpgradeConfig weaponUpgradeConfig &&
            progress is WeaponProgress weaponProgress)
        {
            InitWeaponUnlockingSystem(weaponUpgradeConfig, weaponProgress);
        }
        else if (cfg is TrainUpgradeConfig trainUpgradeConfig 
            && progress is TrainProgress trainProgress)
        {
            _unlockCostText.text = "";
            _unlockButton.gameObject.SetActive(false);
            _lockPanel.SetActive(false);
            _upgradeButton.gameObject.SetActive(true);
        }

        Refresh();
    }

    private void InitWeaponUnlockingSystem(WeaponUpgradeConfig weaponUpgradeConfig, WeaponProgress weaponProgress)
    {
        _unlockingCost = weaponUpgradeConfig.UnblockingCost;
        _unlockCostText.text = _unlockingCost.ToString();

        _isAvailable = weaponProgress.IsAvailable;

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
        _levelText.text = _progress.GetSumLevels().ToString();
        _weaponName.text = _upgradeConfig.Name;
    }

    private void BuyItem()
    {
        if (CoinsHandler.Instance.CoinsCount >= _unlockingCost &&
            _progress is WeaponProgress progress &&
            _upgradeConfig is WeaponUpgradeConfig _weaponUpgrade)
        {
            _unlockButton.onClick.RemoveListener(BuyItem);
            CoinsHandler.Instance.RemoveCoins(_unlockingCost);
            _unlockButton.gameObject.SetActive(false);
            _lockPanel.SetActive(false);
            _upgradeButton.gameObject.SetActive(true);
            WeaponUnlocked?.Invoke(progress, _weaponUpgrade);
            ProgressHandler.Instance.RefreshSumLevels();
        }
    }
}

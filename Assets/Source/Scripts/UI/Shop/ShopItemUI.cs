using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting.FullSerializer;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private TMP_Text _rangeText;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Image _itemIcon;

    private WeaponUpgradeConfig _config;
    private int _currentLevel;

    public void Init(WeaponUpgradeConfig config, int savedLevel)
    {
        _config = config;
        _currentLevel = savedLevel;
        _currentLevel = Mathf.Clamp(savedLevel, 0, config.MaxLevel);

        _upgradeButton.onClick.RemoveAllListeners();
        _upgradeButton.onClick.AddListener(OnUpgradePressed);
        RefreshUI();
    }

    private void RefreshUI()
    {
        int nextLevel = _currentLevel+1;
        bool canUpgrade = nextLevel <= _config.MaxLevel;

        _levelText.text = $"Level: {_currentLevel}/{_config.MaxLevel}";
        _costText.text = canUpgrade ? $"Cost: {_config.GetCost(nextLevel)}" : "MAX";
        _damageText.text = $"Damage: {_config.GetDamage(_currentLevel)}";
        _rangeText.text = $"Range:  {_config.GetRange(_currentLevel):0.#}";
        _itemIcon.sprite = _config.Icon;

        _upgradeButton.interactable = canUpgrade;
    }

    private void OnUpgradePressed()
    {
        int nextLevel = _currentLevel + 1;
        int cost = _config.GetCost(nextLevel);

        var data = SaveManager.Instance.Data;

        if (data.Coins < cost || nextLevel > _config.MaxLevel)
        {
            Debug.Log($"Not enough coins (have {data.Coins}, need {cost})");
            return;
        }

        data.Coins -= cost;
        _currentLevel = nextLevel;

        var wp = data.Weapons.Find(w => w.WeaponId == _config.WeaponId);
        if (wp != null) wp.UpgradeLevel = _currentLevel;

        SaveManager.Instance.Save();

        RefreshUI();
        FindObjectOfType<Shop>()?.RefreshCoins();
    }

    private void SaveProgress(ProgressData data)
    {
        var wp = data.Weapons.Find(w => w.WeaponId == _config.WeaponId);
        if (wp != null) { 
            wp.UpgradeLevel = _currentLevel;
        }
        SaveManager.Instance.Save();
    }
}

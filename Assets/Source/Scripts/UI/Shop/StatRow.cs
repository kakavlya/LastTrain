using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _statName;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _amount;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private Button _upgradeButton;

    private StatType _statType;
    private UpgradeConfig _upgradeConfig;
    private BaseProgress _progress;

    public void Init(StatType statType, UpgradeConfig upgradeConfig, BaseProgress progress, Action<StatType> onUpgrade)
    {
        _statType = statType;
        _upgradeConfig = upgradeConfig;
        _progress = progress;

        _statName.text = statType.ToString();
        _upgradeButton.onClick.AddListener(() => onUpgrade?.Invoke(statType));

        Refresh();
    }

    public void Refresh()
    {
        var maxLevel = _upgradeConfig.GetMaxLevel(_statType);
        var currentLevel = _progress.GetLevel(_statType);

        float ratio = currentLevel / (float)maxLevel;

        _slider.value = ratio;

        _level.text = $"{currentLevel}/{maxLevel}";
        _amount.text = _upgradeConfig.GetStat(_statType, currentLevel).ToString("F1");

        bool canUpgrade = currentLevel < maxLevel;
        _cost.text = canUpgrade ? _upgradeConfig.GetCost(_statType, currentLevel).ToString() : "-";
        _upgradeButton.interactable = canUpgrade;
    }
}

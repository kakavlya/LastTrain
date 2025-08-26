using System;
using Assets.SimpleLocalization.Scripts;
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
    private bool _isShowFractionalValue;

    public void Init(StatConfig statConfig, UpgradeConfig upgradeConfig, BaseProgress progress, Action<StatType> onUpgrade)
    {
        _isShowFractionalValue = statConfig.IsShowFractionalValue;
        _statType = statConfig.StatType;
        _upgradeConfig = upgradeConfig;
        _progress = progress;

        _statName.text = statConfig.Name;
        
        _upgradeButton.onClick.AddListener(() => onUpgrade?.Invoke(statConfig.StatType));

        Refresh();
    }

    public void Refresh()
    {
        var maxLevel = _upgradeConfig.GetMaxLevel(_statType);
        var currentLevel = _progress.GetLevel(_statType);

        float ratio = currentLevel / (float)maxLevel;

        _slider.value = ratio;

        _level.text = $"{currentLevel}/{maxLevel}";

        var statValue = _upgradeConfig.GetStat(_statType, currentLevel);
        
        if (_isShowFractionalValue)
        {
            _amount.text = statValue.ToString("F1");
        }
        else
        {
            _amount.text = statValue.ToString("F0");
        }

        bool canUpgrade = currentLevel < maxLevel;
        _cost.text = canUpgrade ? _upgradeConfig.GetCost(_statType, currentLevel).ToString() : "-";
        _upgradeButton.interactable = canUpgrade;
    }
}

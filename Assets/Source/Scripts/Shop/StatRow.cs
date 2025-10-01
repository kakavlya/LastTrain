using LastTrain.Persistence;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LastTrain.ShopSystem
{
    public class StatRow : MonoBehaviour
    {
        private const string FractionalFormat = "F1";
        private const string IntegerFormat = "F0";
        private const string MaxLevelText = "-";

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

        public void Init(
            StatConfig statConfig, UpgradeConfig upgradeConfig, BaseProgress progress, Action<StatType> onUpgrade)
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
                _amount.text = statValue.ToString(FractionalFormat);
            }
            else
            {
                _amount.text = statValue.ToString(IntegerFormat);
            }

            bool canUpgrade = currentLevel < maxLevel;
            _cost.text = canUpgrade ? _upgradeConfig.GetCost(_statType, currentLevel).ToString() : MaxLevelText;
            _upgradeButton.interactable = canUpgrade;
        }
    }
}
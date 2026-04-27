using LastTrain.Core;
using LastTrain.Persistence;
using LastTrain.Training;
using LastTrain.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace LastTrain.Player
{
    public class PlayerHealth : HealthBase
    {
        private const string DisplayFormat = "F0";

        [SerializeField] private Slider _healthSlider;
        [SerializeField] private TextMeshProUGUI _healthText;

        private float _maxHealth;

        public event Action Died;

        public float MaxHealth => _maxHealth;

        protected override void Awake()
        {
            base.Awake();
            OnDeath.AddListener(OnPlayerDeath);
            _maxHealth = GetMaxHealthValue();
            SetCurrentHealth(_maxHealth);
            _healthText.text = MaxHealth.ToString(DisplayFormat);
            _healthSlider.maxValue = MaxHealth;
            _healthSlider.value = MaxHealth;
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            _healthText.text = CurrentHealth.ToString(DisplayFormat);
            _healthSlider.value = CurrentHealth;
        }

        private void OnPlayerDeath()
        {
            Died?.Invoke();
            TrainingHandler.Instance.TryEndGameplayTrainingAndLoadMenu();
        }

        private float GetMaxHealthValue()
        {
            var trainConfigs = TransferData.Instance.TrainUpgradeConfig.StatConfigs;
            var healthLevel = YG2.saves.TrainProgress.HealthLevel;
            StatConfig healthConfig = null;

            foreach (var config in trainConfigs)
            {
                if (config.StatType == StatType.Health)
                {
                    healthConfig = config;
                    break;
                }
            }

            float maxHealth = healthConfig != null ? healthConfig.GetValue(healthLevel) : 100f;

            // Add Hardpoint HP bonuses
            var hardpointConfigs = TransferData.Instance.HardpointConfigs;
            if (hardpointConfigs != null && YG2.saves.HardpointsProgress != null)
            {
                foreach (var hpProgress in YG2.saves.HardpointsProgress)
                {
                    if (!hpProgress.IsUnlocked) continue;

                    // Find the matching config for this hardpoint index
                    foreach (var hpConfig in hardpointConfigs)
                    {
                        if (hpConfig != null && hpConfig.HardpointIndex == hpProgress.HardpointIndex)
                        {
                            maxHealth += hpProgress.Level * hpConfig.HpBonusPerLevel;
                            break;
                        }
                    }
                }
            }

            return maxHealth;
        }
    }
}

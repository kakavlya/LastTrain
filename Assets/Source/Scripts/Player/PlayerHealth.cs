using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using LastTrain.Core;
using LastTrain.Data;
using LastTrain.Persistence;
using LastTrain.Training;

namespace LastTrain.Player
{
    public class PlayerHealth : HealthBase
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private SharedData _sharedData;

        private float _maxHealth;

        public event Action Died;

        public float MaxHealth => _maxHealth;

        protected override void Awake()
        {
            base.Awake();
            OnDeath.AddListener(OnPlayerDeath);
            _maxHealth = GetMaxHealthValue();
            CurrentHealth = _maxHealth;
            _healthText.text = MaxHealth.ToString("F0");
            _healthSlider.maxValue = MaxHealth;
            _healthSlider.value = MaxHealth;
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            _healthText.text = GetCurrentHealth.ToString("F0");
            _healthSlider.value = GetCurrentHealth;
        }

        private void OnPlayerDeath()
        {
            Died?.Invoke();
            TrainingHandler.Instance.TryEndGameplayTrainingAndLoadMenu();
        }

        private float GetMaxHealthValue()
        {
            var trainConfigs = _sharedData.TrainUpgradeConfig.StatConfigs;
            var healthLevel = YG2.saves.TrainProgress.HealthLevel;
            StatConfig healthConfig = null;

            foreach (var config in trainConfigs)
            {
                if (config.StatType == StatType.Health)
                {
                    healthConfig = config;
                }
            }

            return healthConfig.GetValue(healthLevel);
        }
    }
}

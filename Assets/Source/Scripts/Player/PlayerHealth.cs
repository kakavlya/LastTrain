using Assets.Source.Scripts.Core;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBase
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private float _respawnDelay = 3f;
    [SerializeField] private SharedData _sharedData;

    public event Action Died;

    protected override void Awake()
    {
        base.Awake();
        OnDeath.AddListener(OnPlayerDeath);
        _healthText.text = MaxHealth.ToString();
        _healthSlider.maxValue = MaxHealth;
        _healthSlider.value = MaxHealth;
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        _healthText.text = GetCurrentHealth.ToString();
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
        var healthLevel = SaveManager.Instance.Data.TrainProgress.HealthLevel;
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

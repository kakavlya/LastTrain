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

    public event Action Died;

    protected override void Awake()
    {
        base.Awake();
        OnDeath.AddListener(OnPlayerDeath);
        _healthText.text = MaxHealth.ToString();
        _healthSlider.maxValue = MaxHealth;
        _healthSlider.value = MaxHealth;
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        _healthText.text = CurrentHealth.ToString();
        _healthSlider.value = CurrentHealth;
        Debug.Log($"Player took damage: {amount}. Current health: {CurrentHealth}.");
    }

    private void OnPlayerDeath()
    {
        Debug.Log("[PlayerHealth] Player died");
        Died?.Invoke();
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(_respawnDelay);
    }
}

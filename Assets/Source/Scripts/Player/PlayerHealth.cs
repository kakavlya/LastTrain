using Assets.Source.Scripts.Core;
using System.Collections;
using UnityEngine;

public class PlayerHealth : HealthBase
{
    // Todo : добавить UI для отображения здоровья
    //[SerializeField] private PlayerUI _ui;      
    [SerializeField] private float _respawnDelay = 3f;

    protected override void Awake()
    {
        base.Awake();
        OnDeath.AddListener(OnPlayerDeath);
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        //_ui?.UpdateHealth(CurrentHealth, _maxHealth);
    }

    private void OnPlayerDeath()
    {
        // например, обнулить управление, показать меню
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(_respawnDelay);
    }
}

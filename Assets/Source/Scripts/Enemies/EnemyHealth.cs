using Assets.Source.Scripts.Core;
using Assets.Source.Scripts.Enemies;
using UnityEngine;

public class EnemyHealth : HealthBase
{
    [Header("Health Settings")]
    [SerializeField] private float _maxHealth = 100;

    private EnemyDeathHandler _deathHandler;
    private int _rewardForKill;

    protected override void Awake()
    {
        base.Awake();
        _deathHandler = GetComponent<EnemyDeathHandler>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        CurrentHealth = _maxHealth;
    }

    public void SetRewardForKill(int reward)
    {
        _rewardForKill = reward;
    }

    protected override void Die()
    {
        base.Die();
        _deathHandler?.HandleDeath();
        CoinsHandler.Instance.AddCoins(_rewardForKill);
    }

    public void HandleDie()
    {
        Die();
    }
}

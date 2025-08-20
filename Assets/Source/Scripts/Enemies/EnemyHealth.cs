using Assets.Source.Scripts.Core;
using Assets.Source.Scripts.Enemies;

public class EnemyHealth : HealthBase
{
    private EnemyDeathHandler _deathHandler;
    private int _rewardForKill;

    protected override void Awake()
    {
        base.Awake();
        _deathHandler = GetComponent<EnemyDeathHandler>();
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

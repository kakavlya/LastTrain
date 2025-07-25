using Assets.Source.Scripts.Core;
using Assets.Source.Scripts.Enemies;

public class EnemyHealth : HealthBase
{
    private EnemyView _view;
    private EnemyDeathHandler _deathHandler;
    private int _rewardForKill;

    protected override void Awake()
    {
        base.Awake();
        _view = GetComponent<EnemyView>();
        _deathHandler = GetComponent<EnemyDeathHandler>();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        _view?.PlayHitFX();
    }

    public void SetRewardForKill(int reward)
    {
        _rewardForKill = reward;
    }

    protected override void Die()
    {
        base.Die();
        _view?.PlayDeathFX();
        _deathHandler?.HandleDeath();
        CoinsHandler.Instance.AddCoins(_rewardForKill);
    }
}

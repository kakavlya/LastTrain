using Assets.Source.Scripts.Core;
using Assets.Source.Scripts.Enemies;

public class EnemyHealth : HealthBase
{
    private EnemyView _view;
    private EnemyDeathHandler _deathHandler;

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

    protected override void Die()
    {
        base.Die();
        _view?.PlayDeathFX();
        _deathHandler?.HandleDeath();
    }
}

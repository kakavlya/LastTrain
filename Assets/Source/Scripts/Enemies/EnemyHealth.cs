using LastTrain.Coins;
using LastTrain.Core;

namespace LastTrain.Enemies
{
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

        public void HandleDie()
        {
            Die();
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            CombatEvents.RaiseHit();
        }

        protected override void Die()
        {
            base.Die();
            _deathHandler?.HandleDeath();
            CoinsHandler.Instance.AddCoins(_rewardForKill);
        }
    }
}

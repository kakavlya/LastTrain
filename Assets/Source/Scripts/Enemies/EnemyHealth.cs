using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Scripts.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 100;

        public UnityEvent OnDeath; // для внешних подписок (например, GameManager, UI)
        public int CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }

        private EnemyDeathHandler _deathHandler;
        private EnemyView _enemyView;

        private void Awake()
        {
            _deathHandler = GetComponent<EnemyDeathHandler>();
            _enemyView = GetComponent<EnemyView>();
        }

        private void OnEnable()
        {
            ResetHealth(); // for pool using
        }

        public void TakeDamage(int amount)
        {
            if (IsDead) return;

            CurrentHealth -= amount;
            _enemyView?.PlayHitFX();

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }
        public void Die()
        {
            IsDead = true;
            OnDeath?.Invoke();

            _deathHandler?.HandleDeath();
        }

        public void ResetHealth()
        {
            CurrentHealth = _maxHealth;
            IsDead = false;
        }
    }
}
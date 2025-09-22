using UnityEngine;
using UnityEngine.Events;
using LastTrain.Effects;
using LastTrain.Enemies;

namespace LastTrain.Core
{
    public class HealthBase : MonoBehaviour, IDamageable
    {      
        private ModelEffects _view;
        private float _currentHealth;

        public UnityEvent OnDeath;

        public bool IsDead { get; private set; }

        public float CurrentHealth => _currentHealth;

        protected virtual void Awake()
        {
            _view = GetComponent<ModelEffects>();
        }

        protected virtual void OnEnable()
        {
            IsDead = false;
        }

        public virtual void TakeDamage(float damage)
        {
            if (IsDead)
                return;

            _currentHealth -= damage;
            _view?.PlayHitFX();

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        public void SetCurrentHealth(float health)
        {
            _currentHealth = health;
        }

        protected virtual void Die()
        {
            _view?.PlayDeathFX();
            IsDead = true;
            OnDeath?.Invoke();
        }
    }
}
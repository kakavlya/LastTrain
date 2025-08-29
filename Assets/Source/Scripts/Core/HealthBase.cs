using UnityEngine;
using UnityEngine.Events;
using LastTrain.Effects;
using LastTrain.Enemies;

namespace LastTrain.Core
{
    public class HealthBase : MonoBehaviour, IDamageable
    {      
        private ModelEffects _view;

        protected float CurrentHealth;

        public UnityEvent OnDeath;

        public bool IsDead { get; private set; }

        public float GetCurrentHealth => CurrentHealth;

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

            CurrentHealth -= damage;
            _view?.PlayHitFX();

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            _view?.PlayDeathFX();
            IsDead = true;
            OnDeath?.Invoke();
        }
    }
}
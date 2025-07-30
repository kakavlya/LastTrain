using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Scripts.Core
{
    public class HealthBase : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [SerializeField] private int _maxHealth = 100;
        
        private int _currentHealth;
        private ModelEffects _view;

        public UnityEvent OnDeath;

        public bool IsDead { get; private set; }

        public int CurrentHealth => _currentHealth;

        public int MaxHealth => _maxHealth;

        protected virtual void Awake()
        {
            _view = GetComponent<ModelEffects>();
        }

        protected virtual void OnEnable()
        {
            _currentHealth = _maxHealth;
            IsDead = false;
        }

        public virtual void TakeDamage(int damage)
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
        protected virtual void Die()
        {
            _view?.PlayDeathFX();
            IsDead = true;
            OnDeath?.Invoke();
        }
    }
}
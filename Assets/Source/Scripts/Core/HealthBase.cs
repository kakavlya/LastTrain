using UnityEngine;
using UnityEngine.Events;

namespace Assets.Source.Scripts.Core
{
    public class HealthBase : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private int _currentHealth;

        public UnityEvent OnDeath;

        public bool IsDead { get; private set; }

        public int CurrentHealth => _currentHealth;

        protected virtual void Awake()
        {
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
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
        protected virtual void Die()
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
    }
}
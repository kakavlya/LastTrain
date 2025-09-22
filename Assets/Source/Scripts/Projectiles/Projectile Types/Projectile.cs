using System;
using UnityEngine;
using LastTrain.Enemies;
using LastTrain.Particles;
using LastTrain.Projectiles.Effects;

namespace LastTrain.Projectiles.Types
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private TrailHandler _trail;
        [SerializeField] protected ParticleSystem _impactPrefab;
        [field: SerializeField] public float Lifetime { get; private set; } = 3f;
        [field: SerializeField] public bool UsePooling { get; private set; } = false;

        protected Rigidbody ProjectileRigidbody;
        protected float SpawnTime;

        public event Action<Projectile> OnReturnToPool;

        public float Speed { get; private set; } = 100f;
        public float Damage { get; private set; } = 50;
        public float MaxAttackDistance { get; private set; } = 100;

        public GameObject Owner { get; private set; }

        private void Awake()
        {
            ProjectileRigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            SpawnTime = Time.time;
        }

        public virtual void SetVelocity()
        {
            if (ProjectileRigidbody != null)
                ProjectileRigidbody.velocity = transform.forward * Speed;
        }

        protected virtual void Update()
        {
            if (Time.time - SpawnTime >= Lifetime)
                Despawn();

            if (Owner != null)
            {
                float currentDistance = Vector3.Distance(transform.position, Owner.transform.position);

                if (currentDistance > MaxAttackDistance)
                    Despawn();
            }
        }

        protected virtual void BeforeDespawn() { }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (_impactPrefab != null)
                    ParticlePool.Instance.Spawn(_impactPrefab, transform.position);

                Despawn();
            }

            if (Owner != null && collider.transform.IsChildOf(Owner.transform))
                return;

            if (IsFriendlyFire(collider))
                return;

            if (collider.TryGetComponent<IDamageable>(out var dmg))
            {
                dmg.TakeDamage(Damage);
            }

            if (_impactPrefab != null)
                ParticlePool.Instance.Spawn(_impactPrefab, transform.position);

            Despawn();
        }

        public virtual void Initial(
               Vector3 position,
            Quaternion rotation,
            GameObject owner,
            float speed,
            float damage,
            float maxAttackDistance,
            bool usePooling,
            float aoeDamage = 0,
            float aoeRange = 0)
        {
            transform.SetPositionAndRotation(position, rotation);
            Owner = owner;
            Speed = speed;
            Damage = damage;
            MaxAttackDistance = maxAttackDistance;
            UsePooling = usePooling;

            if (owner != null)
                gameObject.layer = owner.layer;

            if (ProjectileRigidbody != null)
            {
                ProjectileRigidbody.angularVelocity = Vector3.zero;
                ProjectileRigidbody.velocity = transform.forward * Speed;
            }

            _trail?.Play(Speed);
        }

        protected void Despawn()
        {
            _trail?.BeginDetachFade();
            BeforeDespawn();

            if (UsePooling)
            {
                OnReturnToPool?.Invoke(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private bool IsFriendlyFire(Collider other)
        {
            if (Owner == null)
                return false;

            bool ownerIsEnemy = Owner.GetComponentInParent<EnemyController>() != null;
            bool targetIsEnemy = other.GetComponentInParent<EnemyController>() != null;
            return ownerIsEnemy && targetIsEnemy;
        }
    }
}
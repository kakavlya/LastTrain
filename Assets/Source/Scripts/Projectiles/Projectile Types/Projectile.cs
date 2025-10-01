using LastTrain.Enemies;
using LastTrain.Particles;
using LastTrain.Projectiles.Effects;
using System;
using UnityEngine;

namespace LastTrain.Projectiles.Types
{
    public class Projectile : MonoBehaviour
    {
        public const string GroundLayerName = "Ground";

        [SerializeField] private TrailHandler _trail;
        [SerializeField] private ParticleSystem _impactPrefab;

        [field: SerializeField] public float Lifetime { get; private set; } = 3f;

        [field: SerializeField] public bool UsePooling { get; private set; } = false;

        private Rigidbody _projectileRigidbody;
        private float _spawnTime;

        public event Action<Projectile> OnReturnToPool;

        public float Speed { get; private set; } = 100f;

        public float Damage { get; private set; } = 50;

        public float MaxAttackDistance { get; private set; } = 100;

        public ParticleSystem ImpactPrefab => _impactPrefab;

        public GameObject Owner { get; private set; }

        private void Awake()
        {
            _projectileRigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _spawnTime = Time.time;
        }

        protected virtual void Update()
        {
            if (Time.time - _spawnTime >= Lifetime)
                Despawn();

            if (Owner != null)
            {
                float currentDistance = Vector3.Distance(transform.position, Owner.transform.position);

                if (currentDistance > MaxAttackDistance)
                    Despawn();
            }
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer(GroundLayerName))
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

        public virtual void SetVelocity()
        {
            if (_projectileRigidbody != null)
                _projectileRigidbody.velocity = transform.forward * Speed;
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

            if (_projectileRigidbody != null)
            {
                _projectileRigidbody.angularVelocity = Vector3.zero;
                _projectileRigidbody.velocity = transform.forward * Speed;
            }

            _trail?.Play(Speed);
        }

        protected virtual void BeforeDespawn()
        {
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
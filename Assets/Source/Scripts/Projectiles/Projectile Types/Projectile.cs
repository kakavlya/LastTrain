using System;
using Assets.Source.Scripts.Core;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem _impactPrefab;
    [field: SerializeField] public float Lifetime { get; private set; } = 3f;
    [field: SerializeField] public bool UsePooling { get; private set; } = false;

    protected Rigidbody ProjectileRigidbody;

    private float _spawnTime;

    public event Action<Projectile> OnReturnToPool;

    public float Speed { get; private set; } = 100f;
    public int Damage { get; private set; } = 50;
    public float MaxAttackDistance { get; private set; } = 100;

    public GameObject Owner { get; private set; }

    private void Awake()
    {
        ProjectileRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _spawnTime = Time.time;
        SetVelocity();
    }

    public virtual void SetVelocity()
    {
        if (ProjectileRigidbody != null)
            ProjectileRigidbody.velocity = transform.forward * Speed;
    }

    private void Update()
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
        if (Owner != null && collider.transform.IsChildOf(Owner.transform))
            return;

        if(IsFriendlyFire(collider))
            return;

        if (collider.TryGetComponent<IDamageable>(out var dmg))
        {
            dmg.TakeDamage(Damage);
        }

        if (_impactPrefab != null)
            ParticlePool.Instance.Spawn(_impactPrefab, transform.position);

        Despawn();
    }

    private bool IsFriendlyFire(Collider other)
    {
        if (Owner == null) return false;

        bool ownerIsEnemy = Owner.GetComponentInParent<EnemyController>() != null;
        bool targetIsEnemy = other.GetComponentInParent<EnemyController>() != null;

        return ownerIsEnemy && targetIsEnemy;
    }

    public virtual void Initial(
        Vector3 position,
        Quaternion rotation,
        GameObject owner,
        float speed,
        int damage,
        float maxAttackDistance,
        bool usePooling,
        int aoeDamage = 0,
        float aoeRange = 0)
    {
        transform.position = position;
        transform.rotation = rotation;
        Owner = owner;
        Speed = speed;
        Damage = damage;
        MaxAttackDistance = maxAttackDistance;
        UsePooling = usePooling;

        if (owner != null)
        {
            gameObject.layer = owner.layer;
        }
    }

    private void Despawn()
    {
        if (UsePooling)
        {
            OnReturnToPool?.Invoke(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

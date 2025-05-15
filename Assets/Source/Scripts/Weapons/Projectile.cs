using System;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public event Action<Projectile> OnReturnToPool;

    [field: SerializeField] public float Speed { get; private set; } = 100f;
    [field: SerializeField] public int Damage { get; set; } = 50;
    [field: SerializeField] public float Lifetime { get; private set; } = 3f;
    [field: SerializeField] public bool UsePooling { get; private set; } = false;
    [field: SerializeField] public float MaxDistance { get; private set; } = 100;

    public GameObject Owner { get; set; }

    private Rigidbody _rb;
    private float _spawnTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _spawnTime = Time.time;
        if (_rb != null)
            _rb.velocity = transform.forward * Speed;
    }

    private void Update()
    {
        if (Time.time - _spawnTime >= Lifetime)
            Despawn();

        float currentDistance = Vector3.Distance(transform.position, Owner.transform.position);

        if (currentDistance > MaxDistance)
            Despawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Owner != null && other.transform.IsChildOf(Owner.transform))
            return;

        if (other.TryGetComponent<IDamageable>(out var dmg))
            dmg.TakeDamage(Damage);

        Despawn();
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

    public void Configure(
        GameObject owner,
        bool usePooling
    )
    {
        Owner = owner;
        UsePooling = usePooling;
    }
}

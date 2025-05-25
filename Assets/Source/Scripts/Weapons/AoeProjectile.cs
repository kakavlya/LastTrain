using UnityEngine;

public class AoeProjectile : Projectile
{
    [SerializeField] private GameObject _particleExplotionPrefab;

    public int AoeDamage { get; private set; } = 0;
    public float AoeRange { get; private set; } = 0;

    public override void SetVelocity()
    {
        if (ProjectileRigidbody != null)
            ProjectileRigidbody.velocity = transform.up * Speed;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        AoeExplode(other.gameObject.layer);
    }

    private void AoeExplode(LayerMask layer)
    {
        if (AoeRange <= 0) return;

        Debug.Log("Work");
        LayerMask targetsLayer = LayerMask.GetMask("Enemy", "Ground");
        
        if ((targetsLayer.value & (1 << layer)) != 0)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, AoeRange);

            foreach (Collider target in targets)
            {
                if (target.TryGetComponent<IDamageable>(out IDamageable aoeDmg))
                {
                    aoeDmg.TakeDamage(AoeDamage);
                }
            }

            Instantiate(_particleExplotionPrefab, transform.position, Quaternion.identity);
        }
    }

    public override void Initial(
        Vector3 position, Quaternion rotation, GameObject owner, float speed,
        int damage, float maxAttackDistance, bool usePooling, int aoeDamage = 0, float aoeRange = 0)
    {
        base.Initial(position, rotation, owner, speed, damage, maxAttackDistance, usePooling, aoeDamage, aoeRange);

        AoeDamage = aoeDamage;
        AoeRange = aoeRange;
    }
}

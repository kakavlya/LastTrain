using UnityEngine;

public class AoeProjectile : Projectile
{
    [Header("AoE Settings")]
    [SerializeField] private LayerMask _hitResponsiveMasks;
    private float _aoeRange;
    private int _aoeDamage;

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
        if (_aoeRange <= 0) return;
        
        if ((_hitResponsiveMasks.value & (1 << layer)) != 0)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, _aoeRange, _hitResponsiveMasks);

            foreach (Collider target in targets)
            {
                if (target.TryGetComponent(out IDamageable aoeDmg))
                {
                    aoeDmg.TakeDamage(_aoeDamage);
                    Debug.Log("yes");
                }
            }
        }
    }

    public override void Initial(
        Vector3 position, Quaternion rotation, GameObject owner, float speed,
        int damage, float maxAttackDistance, bool usePooling, int aoeDamage = 0, float aoeRange = 0)
    {
        base.Initial(position, rotation, owner, speed, damage, maxAttackDistance, usePooling, aoeDamage, aoeRange);

        _aoeDamage = aoeDamage;
        _aoeRange = aoeRange;
    }
}

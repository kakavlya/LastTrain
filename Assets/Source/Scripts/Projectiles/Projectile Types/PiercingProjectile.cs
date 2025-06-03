using System;
using UnityEngine;

public class PiercingProjectile : Projectile
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (Owner != null && other.transform.IsChildOf(Owner.transform))
            return;

        if (other.TryGetComponent<IDamageable>(out var dmg))
            dmg.TakeDamage(Damage);
    }
}

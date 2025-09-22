using UnityEngine;
using LastTrain.Enemies;

namespace LastTrain.Projectiles.Types
{
    public class PiercingProjectile : Projectile
    {
        protected override void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Despawn();
            }

            if (Owner != null && collider.transform.IsChildOf(Owner.transform))
                return;

            if (collider.TryGetComponent<IDamageable>(out var dmg))
                dmg.TakeDamage(Damage);
        }
    }
}

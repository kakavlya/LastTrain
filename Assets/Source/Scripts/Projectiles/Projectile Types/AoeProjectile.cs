using UnityEngine;
using LastTrain.Enemies;
using LastTrain.Particles;

namespace LastTrain.Projectiles.Types
{
    public class AoeProjectile : Projectile
    {
        private float _aoeRange;
        private float _aoeDamage;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }

        protected override void BeforeDespawn()
        {
            AoeExplode();
        }

        public override void Initial(
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
            base.Initial(position, rotation, owner, speed, damage, maxAttackDistance, usePooling, aoeDamage, aoeRange);

            _aoeDamage = aoeDamage;
            _aoeRange = aoeRange;

            if (owner != null)
            {
                gameObject.layer = owner.layer;
            }
        }

        private void AoeExplode()
        {
            if (ImpactPrefab != null)
                ParticlePool.Instance.Spawn(ImpactPrefab, transform.position);

            if (_aoeRange <= 0)
                return;

            Collider[] targets = Physics.OverlapSphere(transform.position, _aoeRange);

            foreach (Collider target in targets)
            {
                if (target.TryGetComponent(out IDamageable aoeDmg) && gameObject.layer != target.gameObject.layer)
                {
                    aoeDmg.TakeDamage(_aoeDamage);
                }
            }
        }
    }
}

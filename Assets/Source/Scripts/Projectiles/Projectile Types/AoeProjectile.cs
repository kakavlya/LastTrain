using LastTrain.Enemies;
using LastTrain.Particles;
using UnityEngine;

namespace LastTrain.Projectiles.Types
{
    public class AoeProjectile : Projectile
    {
        private float _aoeRange;
        private float _aoeDamage;
        private Collider[] _resultsCache = new Collider[32];

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
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

        protected override void BeforeDespawn()
        {
            AoeExplode();
        }

        private void AoeExplode()
        {
            if (ImpactPrefab != null)
                ParticlePool.Instance.Spawn(ImpactPrefab, transform.position);

            if (_aoeRange <= 0)
                return;

            int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            _aoeRange,
            _resultsCache);

            for (int i = 0; i < count; i++)
            {
                Collider target = _resultsCache[i];

                if (target.TryGetComponent(out IDamageable aoeDmg) && gameObject.layer != target.gameObject.layer)
                {
                    aoeDmg.TakeDamage(_aoeDamage);
                }
            }
        }
    }
}

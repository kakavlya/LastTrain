using System.Collections.Generic;
using UnityEngine;

namespace LastTrain.Projectiles
{
    public class ProjectilePool : MonoBehaviour
    {
        public static ProjectilePool Instance { get; private set; }

        private readonly Dictionary<Projectile, Queue<Projectile>> _pools = new Dictionary<Projectile, Queue<Projectile>>();

        public void Init()
        {
            Instance = this;
        }

        public Projectile Spawn(
            Projectile projectilePrefab, Vector3 position, Quaternion rotation,
            GameObject owner, float speed, float damage, float maxDistance,
            float aoeDamage = 0, float aoeRange = 0)
        {
            if (!_pools.TryGetValue(projectilePrefab, out var pool))
            {
                pool = new Queue<Projectile>();
                _pools[projectilePrefab] = pool;
            }

            Projectile proj = null;

            // достаЄм неактивный экземпл€р
            while (pool.Count > 0 && proj == null)
            {
                var candidate = pool.Dequeue();
                if (candidate && !candidate.gameObject.activeSelf)
                    proj = candidate;
                else if (candidate && candidate.gameObject.activeSelf)
                    Debug.LogWarning("ProjectilePool: dequeued active projectile Ч creating a new one.");
            }

            if (proj == null)
                proj = CreateNew(projectilePrefab); 

            proj.Initial(position, rotation, owner, speed, damage, maxDistance, true, aoeDamage, aoeRange);
            proj.gameObject.SetActive(true);

            return proj;
        }

        private Projectile CreateNew(Projectile projectilePrefab, Queue<Projectile> pool)
        {
            var projectile = Instantiate(projectilePrefab, transform);
            projectile.gameObject.SetActive(false);
            projectile.OnReturnToPool += (proj) => ReturnToPool(projectilePrefab, proj);
            pool.Enqueue(projectile);
            return projectile;
        }

        private Projectile CreateNew(Projectile projectilePrefab)
        {
            var projectile = Instantiate(projectilePrefab, transform);
            projectile.gameObject.SetActive(false);

            projectile.OnReturnToPool += (proj) => ReturnToPool(projectilePrefab, proj);
            return projectile;
        }

        private void ReturnToPool(Projectile projectilePrefab, Projectile proj)
        {
            if (!proj) return;

            proj.gameObject.SetActive(false);
            proj.transform.SetParent(transform);

            if (proj.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            if (!_pools.TryGetValue(projectilePrefab, out var pool))
            {
                pool = new Queue<Projectile>();
                _pools[projectilePrefab] = pool;
            }

            pool.Enqueue(proj);
        }
    }
}

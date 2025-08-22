using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    private readonly Dictionary<Projectile, Queue<Projectile>> _pools = new Dictionary<Projectile, Queue<Projectile>>();

    public void Init()
    {
        Instance = this;
    }

    public Projectile Spawn(Projectile projectilePrefab, Vector3 position, Quaternion rotation,
        GameObject owner, float speed, float damage, float maxDistance, int aoeDamage = 0, float aoeRange = 0)
    {
        if (!_pools.TryGetValue(projectilePrefab, out var pool))
        {
            pool = new Queue<Projectile>();
            _pools[projectilePrefab] = pool;
        }

        Projectile proj = pool.Count > 0
            ? pool.Dequeue()
            : CreateNew(projectilePrefab, pool);

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

    private void ReturnToPool(Projectile projectilePrefab, Projectile proj)
    {
        proj.gameObject.SetActive(false);
        proj.transform.SetParent(transform);

        if (proj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (_pools.TryGetValue(projectilePrefab, out var pool))
        {
            pool.Enqueue(proj);
        }
        else
        {
            var newPool = new Queue<Projectile>();
            newPool.Enqueue(proj);
            _pools[projectilePrefab] = newPool;
        }
    }
}

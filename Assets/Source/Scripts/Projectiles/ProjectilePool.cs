using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    private readonly Queue<Projectile> _pool = new Queue<Projectile>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Projectile Spawn(Projectile projectilePrefab, Vector3 position, Quaternion rotation,
        GameObject owner, float speed, int damage, float maxDistance, int aoeDamage = 0, float aoeRange = 0)
    {
        Projectile proj = _pool.Count > 0
            ? _pool.Dequeue()
            : CreateNew(projectilePrefab);

        proj.Initial(position, rotation, owner, speed, damage, maxDistance, true, aoeDamage, aoeRange);
        proj.gameObject.SetActive(true);
        return proj;
    }

    private Projectile CreateNew(Projectile projectilePrefab)
    {
        var projectile = Instantiate(projectilePrefab, transform);
        projectile.gameObject.SetActive(false);
        projectile.OnReturnToPool += ReturnToPool;
        _pool.Enqueue(projectile);
        return projectile;
    }

    private void ReturnToPool(Projectile proj)
    {
        proj.gameObject.SetActive(false);
        proj.transform.SetParent(transform);

        if (proj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        _pool.Enqueue(proj);
    }
}

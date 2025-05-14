using System.Collections.Generic;
using UnityEngine;

public class ProjectileTypesPool : MonoBehaviour
{
    public static ProjectileTypesPool Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private int _initialPoolSize = 20;

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

    public Projectile Spawn(Projectile projectilePrefab, Vector3 position, Quaternion rotation, GameObject owner = null)
    {
        Projectile proj = _pool.Count > 0
            ? _pool.Dequeue()
            : CreateNew(projectilePrefab);

        proj.transform.SetParent(null);
        proj.transform.position = position;
        proj.transform.rotation = rotation;
        proj.Owner = owner;
        proj.gameObject.SetActive(true);
        return proj;
    }

    private Projectile CreateNew(Projectile projectilePrefab)
    {
        var projectile = Instantiate(projectilePrefab, transform);
        projectile.gameObject.SetActive(false);
        projectile.Configure(owner: null, usePooling: true);
        projectile.OnReturnToPool += ReturnToPool;
        _pool.Enqueue(projectile);
        return projectile;
    }

    private void ReturnToPool(Projectile proj)
    {
        proj.gameObject.SetActive(false);
        proj.transform.SetParent(transform);
        _pool.Enqueue(proj);
    }
}

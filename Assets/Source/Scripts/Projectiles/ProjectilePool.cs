using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private Projectile _projectilePrefab;
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

        for (int i = 0; i < _initialPoolSize; i++)
            CreateNew();
    }

    public Projectile Spawn(Vector3 pos, Quaternion rot, GameObject owner = null, int damage = 50)
    {
        Projectile proj = _pool.Count > 0
            ? _pool.Dequeue()
            : CreateNew();

        proj.transform.SetParent(null);
        proj.transform.position = pos;
        proj.transform.rotation = rot;
        proj.Owner = owner;
        proj.Damage = damage;
        proj.gameObject.SetActive(true);
        return proj;
    }

    private Projectile CreateNew()
    {
        var projectile = Instantiate(_projectilePrefab, transform);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private EnemyPrefabKey[] _enemyPrefabs;

    private Dictionary<EnemyPrefabKey, ObjectPool<EnemyPrefabKey>> _pools = new Dictionary<EnemyPrefabKey, ObjectPool<EnemyPrefabKey>>();

    public static EnemyPool Instance { get; private set; }

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var enemyPrefab in _enemyPrefabs)
        {
            CreatePoolForPrefab(enemyPrefab);
        }
    }

    private void CreatePoolForPrefab(EnemyPrefabKey enemyPrefab)
    {
        if (!_pools.ContainsKey(enemyPrefab))
        {
            _pools[enemyPrefab] = new ObjectPool<EnemyPrefabKey>(
                createFunc: () => Instantiate(enemyPrefab),
                actionOnGet: (enemy) => enemy.gameObject.SetActive(true),
                actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
                actionOnDestroy: (enemy) => Destroy(enemy.gameObject)
            );
        }
    }

    public EnemyPrefabKey Spawn(EnemyPrefabKey enemyPrefab, Vector3 position, Quaternion rotation)
    {
        if (!_pools.ContainsKey(enemyPrefab))
        {
            CreatePoolForPrefab(enemyPrefab);
        }

        var enemyInstance = _pools[enemyPrefab].Get();
        enemyInstance.transform.SetPositionAndRotation(position, rotation);
        enemyInstance.SetPrefabKey(enemyPrefab);
        return enemyInstance;
    }

    public void ReleaseEnemy(EnemyPrefabKey enemyInstance, EnemyPrefabKey prefabKey)
    {
        if (_pools.ContainsKey(prefabKey))
        {
            _pools[prefabKey].Release(enemyInstance);
        }
        else
        {
            Destroy(enemyInstance.gameObject);
        }
    }
}

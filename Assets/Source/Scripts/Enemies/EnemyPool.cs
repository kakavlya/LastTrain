using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;

    private Dictionary<GameObject, ObjectPool<GameObject>> _pools = new Dictionary<GameObject, ObjectPool<GameObject>>();

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

    private void CreatePoolForPrefab(GameObject enemyPrefab)
    {
        if (!_pools.ContainsKey(enemyPrefab))
        {
            _pools[enemyPrefab] = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(enemyPrefab),
                actionOnGet: (enemy) => enemy.gameObject.SetActive(true),
                actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
                actionOnDestroy: (enemy) => Destroy(enemy.gameObject)
            );
        }
    }

    public GameObject Spawn(GameObject enemyPrefab, Vector3 position, Quaternion rotation)
    {
        if (!_pools.ContainsKey(enemyPrefab))
        {
            CreatePoolForPrefab(enemyPrefab);
        }

        var enemyInstance = _pools[enemyPrefab].Get();
        enemyInstance.transform.SetPositionAndRotation(position, rotation);
        //enemyInstance.SetPrefabKey(enemyPrefab);  нужен ключ но не знаю как сделать
        return enemyInstance;
    }

    public void ReleaseEnemy(GameObject enemyInstance, GameObject prefabKey)
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

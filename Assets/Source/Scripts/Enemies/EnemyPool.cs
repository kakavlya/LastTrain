using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LastTrain.Enemies
{
    public class EnemyPool : MonoBehaviour
    {
        public static EnemyPool Instance { get; private set; }

        [SerializeField] private GameObject[] _enemyPrefabs;

        private Dictionary<GameObject, ObjectPool<GameObject>> _pools =
            new Dictionary<GameObject, ObjectPool<GameObject>>();

        private void Awake()
        {
            Instance = this;
            InitializePools();
        }

        public GameObject Spawn(GameObject enemyPrefab, Vector3 position, Quaternion rotation)
        {
            if (!_pools.ContainsKey(enemyPrefab))
            {
                CreatePoolForPrefab(enemyPrefab);
            }

            var enemyInstance = _pools[enemyPrefab].Get();
            enemyInstance.transform.SetPositionAndRotation(position, rotation);

            var pooled = enemyInstance.GetComponent<PooledEnemyKey>();
            pooled.SetKey(enemyPrefab);

            return enemyInstance;
        }

        public void ReleaseEnemy(GameObject enemyInstance)
        {
            var key = enemyInstance.GetComponent<PooledEnemyKey>();

            if (key != null && _pools.ContainsKey(key.PrefabKey))
            {
                _pools[key.PrefabKey].Release(enemyInstance);
            }
            else
            {
                Destroy(enemyInstance.gameObject);
            }
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
                    createFunc: () => Instantiate(enemyPrefab, gameObject.transform),
                    actionOnGet: (enemy) => enemy.gameObject.SetActive(true),
                    actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
                    actionOnDestroy: (enemy) => Destroy(enemy.gameObject)
                );
            }
        }
    }
}

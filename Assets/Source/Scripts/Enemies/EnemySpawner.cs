// EnemySpawner.cs
using UnityEngine;
namespace Assets.Source.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform _playerTarget;
        [SerializeField] private Transform[] _spawnPoints;

        private EnemySpawnEntry[] _entries;
        private float[] _timers;

        public void Init(EnemySpawnEntry[] entries, Transform playerTarget, Transform[] spawnPoints)
        {
            _entries = entries;
            _playerTarget = playerTarget;
            _spawnPoints = spawnPoints;

            _timers = new float[_entries.Length];
            for (int i = 0; i < _entries.Length; i++)
                _timers[i] = _entries[i].spawnInterval; 
        }

        private void Update()
        {
            if (_entries == null) return;

            for (int i = 0; i < _entries.Length; i++)
            {
                _timers[i] += Time.deltaTime;
                if (_timers[i] >= _entries[i].spawnInterval)
                {
                    SpawnEnemy(_entries[i]);
                    _timers[i] = 0f;
                }
            }
        }

        private void SpawnEnemy(EnemySpawnEntry entry)
        {
            var sp = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            Vector3 pos = sp.position;
            pos.x += Random.Range(-entry.randRangeXZ.x, entry.randRangeXZ.x);
            pos.z += Random.Range(-entry.randRangeXZ.y, entry.randRangeXZ.y);

            var go = Instantiate(entry.prefab, pos, sp.rotation);
            entry.behaviorSettings?.Initialize(go, _playerTarget);
        }

        [System.Serializable]
        public class EnemySpawnEntry
        {
            public GameObject prefab;
            public float spawnInterval;
            public Vector2 randRangeXZ;
            public EnemyBehaviorSettings behaviorSettings;
        }
    }
}

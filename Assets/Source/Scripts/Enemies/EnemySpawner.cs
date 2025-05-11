using UnityEngine;
using System;

namespace Assets.Source.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Static Config (optional)")]
        [SerializeField] private SpawnerConfig _config;

        private EnemySpawnEntry[] _entries;
        private Transform[] _spawnPoints;
        private Transform _playerTarget;
        private float[] _timers;
        private bool _useRuntime;

        private void Awake()
        {
            if (_useRuntime)
            {
                InitTimers(_entries);
            }
            else if (_config != null && _config.entries != null && _config.entries.Length > 0)
            {
                _entries = _config.entries;
                InitTimers(_entries);
            }
        }

        private void InitTimers(EnemySpawnEntry[] entries)
        {
            _timers = new float[entries.Length];
            for (int i = 0; i < entries.Length; i++)
                _timers[i] = entries[i].spawnInterval;
        }

        private void Update()
        {
            var entriesToUse = _entries;
            var points = _spawnPoints;
            var target = _playerTarget;

            if (entriesToUse == null) return;

            for (int i = 0; i < entriesToUse.Length; i++)
            {
                _timers[i] += Time.deltaTime;
                var e = entriesToUse[i];
                if (_timers[i] >= e.spawnInterval)
                {
                    Spawn(e, points, target);
                    _timers[i] = 0f;
                }
            }
        }

        private void Spawn(EnemySpawnEntry spawnEntry, Transform[] points, Transform player)
        {
            var spawnPoints = (spawnEntry.overrideSpawnPoints != null && spawnEntry.overrideSpawnPoints.Length > 0)
                ? spawnEntry.overrideSpawnPoints
                : points;

            var sp = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            Vector3 pos = sp.position;
            pos.x += UnityEngine.Random.Range(-spawnEntry.randRangeXZ.x, spawnEntry.randRangeXZ.x);
            pos.z += UnityEngine.Random.Range(-spawnEntry.randRangeXZ.y, spawnEntry.randRangeXZ.y);

            var gameObject = Instantiate(spawnEntry.prefab, pos, sp.rotation);
            spawnEntry.behaviorSettings?.Initialize(gameObject, player);
        }

        public void Init(EnemySpawnEntry[] entries, Transform[] spawnPoints, Transform playerTarget)
        {
            _useRuntime = true;
            _entries = entries;
            _spawnPoints = spawnPoints;
            _playerTarget = playerTarget;
            InitTimers(_entries);
        }
    }
}

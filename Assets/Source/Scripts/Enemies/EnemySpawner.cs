using UnityEngine;
using System;

namespace Assets.Source.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Static Config (optional)")]
        [SerializeField] private SpawnerConfig _config;

        [Header("Scene-bound")]
        [SerializeField] private Transform[] _spawnPoints;

        [SerializeField] private bool _useRuntime;

        private EnemySpawnEntry[] _entries;
        private Transform _playerTarget;
        private float[] _timers;

        private bool _paused;
        private bool _stopped;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            _entries = _config.entries;
        }

        public void Init(Transform playerTarget)
        {
            _playerTarget = playerTarget;
            Init();
        }

        public void Begin()
        {
            _stopped = false;
            _paused = false;
            InitTimers();
        }

        private void InitTimers()
        {
            _timers = new float[_entries.Length];
            for (int i = 0; i < _entries.Length; i++)
                _timers[i] = _entries[i].spawnInterval;
        }

        private void Update()
        {
            if (_paused || _stopped) return;
            if (_entries == null || _timers == null) return;

            for (int i = 0; i < _entries.Length; i++)
            {
                _timers[i] += Time.deltaTime;
                var entry = _entries[i];
                if (_timers[i] >= entry.spawnInterval)
                {
                    Spawn(entry, _spawnPoints, _playerTarget);
                    _timers[i] = 0f;
                }
            }
        }

        public void SetSpawnPoint(Transform[] spawnPoints)
        {
            _spawnPoints = spawnPoints;
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

        public void Pause() => _paused = true;
        public void Resume() => _paused = false;

        public void Stop()
        {
            _stopped = true;
            _paused = false;
        }

        //public void Init(EnemySpawnEntry[] entries, Transform[] spawnPoints, Transform playerTarget)
        //{
        //    _useRuntime = true;
        //    _entries = entries;
        //    _spawnPoints = spawnPoints;
        //    _playerTarget = playerTarget;
        //    InitTimers(_entries);
        //}
    }
}

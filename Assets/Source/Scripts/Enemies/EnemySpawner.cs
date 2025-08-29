using UnityEngine;
using System.Collections.Generic;
using LastTrain.Data;
using LastTrain.Level;
using LastTrain.Player;

namespace LastTrain.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private SharedData _sharedData;
        [SerializeField] private TrainMovement _trainMovement;
        [SerializeField] private BoxCollider _trainCollider;

        [Header("Scene-bound")]
        [SerializeField] private float _allowTrainDistance = 200f;

        private SpawnerConfig _spawnerConfig;
        private Transform[] _spawnPoints;
        private EnemySpawnEntry[] _entries;
        private Transform _playerTarget;
        private float[] _timers;
        private bool _paused;
        private bool _stopped;

        private void Update()
        {
            if (_paused || _stopped || _entries == null || _timers == null || _spawnPoints == null) return;

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

        private void OnDisable()
        {
            _levelGenerator.StartedElementDefined -= SetSpawnPoint;
            _levelGenerator.ElementChanged -= SetSpawnPoint;
        }

        public void Init()
        {
            _spawnerConfig = _sharedData.LevelSetting.SpawnerConfig;
            _entries = _spawnerConfig.entries;
            _levelGenerator.StartedElementDefined += SetSpawnPoint;
            _levelGenerator.ElementChanged += SetSpawnPoint;
        }

        public void SetTarget(Transform playerTarget)
        {
            _playerTarget = playerTarget;
        }

        public void Begin()
        {
            _stopped = false;
            _paused = false;
            InitTimers();
        }

        public void Pause() => _paused = true;
        public void Resume() => _paused = false;

        public void Stop()
        {
            _stopped = true;
            _paused = false;
        }

        public void SetSpawnPoint(LevelElement currentElement, LevelElement nextElement)
        {
            _spawnPoints = currentElement.EnemySpawnPoints;
        }

        private void InitTimers()
        {
            _timers = new float[_entries.Length];

            for (int i = 0; i < _entries.Length; i++)
                _timers[i] = _entries[i].spawnInterval;
        }

        private void Spawn(EnemySpawnEntry spawnEntry, Transform[] points, Transform player)
        {
            var spawnPoints = (spawnEntry.overrideSpawnPoints != null && spawnEntry.overrideSpawnPoints.Length > 0)
                ? spawnEntry.overrideSpawnPoints
                : points;

            List<Transform> spawnPointsList = new List<Transform>(spawnPoints);

            foreach (var point in spawnPoints)
            {
                if (point.position.x - _trainMovement.transform.position.x <= _allowTrainDistance)
                {
                    spawnPointsList.Add(point);
                }
            }

            var sp = spawnPointsList[UnityEngine.Random.Range(0, spawnPointsList.Count)];
            Vector3 pos = sp.position;
            pos.x += UnityEngine.Random.Range(-spawnEntry.randRangeXZ.x, spawnEntry.randRangeXZ.x);
            pos.z += UnityEngine.Random.Range(-spawnEntry.randRangeXZ.y, spawnEntry.randRangeXZ.y);
            var enemy = EnemyPool.Instance.Spawn(spawnEntry.prefab, pos, sp.rotation);
            enemy.GetComponent<EnemyHealth>().SetRewardForKill(spawnEntry.behaviorSettings.Reward);
            enemy.GetComponent<EnemyHealth>().SetCurrentHealth(spawnEntry.behaviorSettings.Health);
            spawnEntry.behaviorSettings?.Initialize(enemy, player, _trainCollider);
        }
    }
}

using System;
using UnityEngine;

namespace LastTrain.Enemies
{
    [Serializable]
    public struct EnemySpawnEntry
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _spawnInterval;
        [SerializeField] private Vector2 _randRangeXZ;
        [SerializeField] private EnemyBehaviorSettings _behaviorSettings;

        [Tooltip("Take spawnPoints from SpawnerConfig if nothing set")]
        [SerializeField] private Transform[] _overrideSpawnPoints;

        public GameObject Prefab => _prefab;
        public float SpawnInterval => _spawnInterval;
        public Vector2 RandRangeXZ => _randRangeXZ;
        public EnemyBehaviorSettings BehaviorSettings => _behaviorSettings;
        public Transform[] OverrideSpawnPoints => _overrideSpawnPoints;

    }
}
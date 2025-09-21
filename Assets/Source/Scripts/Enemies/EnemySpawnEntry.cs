using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LastTrain.Enemies
{
    [Serializable]
    public struct EnemySpawnEntry
    {
        public GameObject Prefab;
        public float SpawnInterval;
        public Vector2 RandRangeXZ;
        public EnemyBehaviorSettings BehaviorSettings;

        [Tooltip("Take spawnPoints from SpawnerConfig if nothing set")]
        public Transform[] OverrideSpawnPoints;
    }
}
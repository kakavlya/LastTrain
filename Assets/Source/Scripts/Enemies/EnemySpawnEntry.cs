using System;
using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    [Serializable]
    public struct EnemySpawnEntry
    {
        public GameObject prefab;
        public float spawnInterval;
        public Vector2 randRangeXZ;
        public EnemyBehaviorSettings behaviorSettings;

        [Tooltip("Take spawnPoints from SpawnerConfig if nothing set")]
        public Transform[] overrideSpawnPoints;
    }
}
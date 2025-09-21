using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LastTrain.Enemies
{
    [Serializable]
    public struct EnemySpawnEntry
    {
        [FormerlySerializedAs("prefab")]
        public GameObject Prefab;
        [FormerlySerializedAs("spawnInterval")]
        public float SpawnInterval;
        [FormerlySerializedAs("randRangeXZ")]
        public Vector2 RandRangeXZ;
        [FormerlySerializedAs("behaviorSettings")]
        public EnemyBehaviorSettings BehaviorSettings;

        [Tooltip("Take spawnPoints from SpawnerConfig if nothing set")]
        [FormerlySerializedAs("overrideSpawnPoints")]
        public Transform[] OverrideSpawnPoints;
    }
}
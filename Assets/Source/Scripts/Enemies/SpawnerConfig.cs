using UnityEngine;

namespace LastTrain.Enemies
{
    [CreateAssetMenu(menuName = "Spawners/Spawner Config")]
    public class SpawnerConfig : ScriptableObject
    {
        [Header("What to spawn and how often")]
        public EnemySpawnEntry[] entries;
    }
}
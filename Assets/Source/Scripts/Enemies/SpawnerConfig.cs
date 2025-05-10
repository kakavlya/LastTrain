using Assets.Source.Scripts.Enemies;
using System.Collections;
using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    [CreateAssetMenu(menuName = "Spawners/Spawner Config")]
    public class SpawnerConfig : ScriptableObject
    {
        [Header("What to spawn and how often")]
        public EnemySpawnEntry[] entries;
    }
}
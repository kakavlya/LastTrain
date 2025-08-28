using System;
using UnityEngine;
using LastTrain.Enemies;

namespace LastTrain.Level
{
    [CreateAssetMenu(menuName = "Levels/Level Setting")]
    public class LevelSetting : ScriptableObject
    {
        public int LevelNumber;
        public bool IsAvailable;
        public LevelElement[] LevelElements;
        public int LevelDurationSec;
        public int LevelReward;

        [Range(0, 100)]
        public int AmmunitionGeneratePercent;
        public SpawnerConfig SpawnerConfig;
    }
}

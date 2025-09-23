using System;
using UnityEngine;
using LastTrain.Enemies;

namespace LastTrain.Level
{
    [CreateAssetMenu(menuName = "Levels/Level Setting")]
    public class LevelSetting : ScriptableObject
    {
        [SerializeField] private int _levelNumber;
        [SerializeField] private bool _isAvailable;
        [SerializeField] private LevelElement[] _levelElements;
        [SerializeField] private int _levelDurationSec;
        [SerializeField] private int _levelReward;

        [Range(0, 100)]
        [SerializeField] private int _ammunitionGeneratePercent;
        [SerializeField] private SpawnerConfig _spawnerConfig;

        public int LevelNumber => _levelNumber;
        public bool IsAvailable => _isAvailable;
        public LevelElement[] LevelElements => _levelElements;
        public int LevelDurationSec => _levelDurationSec;
        public int LevelReward => _levelReward;
        public int AmmunitionGeneratePercent => _ammunitionGeneratePercent;
        public SpawnerConfig SpawnerConfig => _spawnerConfig;

        public void SetAvailable(bool available)
        {
            _isAvailable = available;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace LastTrain.Data
{
    [CreateAssetMenu(menuName = "Data/Shared Data")]
    public class SharedData : ScriptableObject
    {
        private LevelSetting[] _allLevels;

        public LevelSetting LevelSetting;
        public List<WeaponUpgradeConfig> WeaponConfigs = new List<WeaponUpgradeConfig>();
        public TrainUpgradeConfig TrainUpgradeConfig;

        public LevelSetting[] AllLevels => _allLevels;

        public void SetAllLevels(LevelSetting[] levelSettings)
        {
            _allLevels = levelSettings;
        }
    }
}

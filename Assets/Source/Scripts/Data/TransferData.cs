using LastTrain.Level;
using LastTrain.Persistence;
using System.Collections.Generic;
using UnityEngine;

namespace LastTrain.Data
{
    public class TransferData : MonoBehaviour
    {
        public static TransferData Instance { get; private set; }

        [SerializeField] private TrainUpgradeConfig _trainUpgradeConfig;

        private LevelSetting _levelSetting;
        private List<WeaponUpgradeConfig> _weaponConfigs = new List<WeaponUpgradeConfig>();
        private LevelSetting[] _allLevelSettings;

        public LevelSetting LevelSetting => _levelSetting;
        public List<WeaponUpgradeConfig> WeaponConfigs => _weaponConfigs;
        public TrainUpgradeConfig TrainUpgradeConfig => _trainUpgradeConfig;
        public LevelSetting[] AllLevels => _allLevelSettings;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetAllLevels(LevelSetting[] levelSettings)
        {
            _allLevelSettings = levelSettings;
        }

        public void SetLevelSetting(LevelSetting levelSetting)
        {
            _levelSetting = levelSetting;
        }

        public void SetWeaponConfigs(List<WeaponUpgradeConfig> weaponConfigs)
        {
            _weaponConfigs = weaponConfigs;
        }
    }
}

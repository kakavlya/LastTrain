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
        [SerializeField] private HardpointUpgradeConfig[] _hardpointConfigs;

        private LevelSetting _levelSetting;
        private List<WeaponUpgradeConfig> _weaponConfigs = new List<WeaponUpgradeConfig>();
        private List<TurretUpgradeConfig> _turretConfigs = new List<TurretUpgradeConfig>();
        private LevelSetting[] _allLevelSettings;

        public LevelSetting LevelSetting => _levelSetting;
        public List<WeaponUpgradeConfig> WeaponConfigs => _weaponConfigs;
        public List<TurretUpgradeConfig> TurretConfigs => _turretConfigs;
        public TrainUpgradeConfig TrainUpgradeConfig => _trainUpgradeConfig;
        public HardpointUpgradeConfig[] HardpointConfigs => _hardpointConfigs;
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

        public void SetTurretConfigs(List<TurretUpgradeConfig> turretConfigs)
        {
            _turretConfigs = turretConfigs;
        }
    }
}

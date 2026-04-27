using UnityEngine;

namespace LastTrain.Persistence
{
    [CreateAssetMenu(menuName = "Config/TurretUpgrade")]
    public class TurretUpgradeConfig : UpgradeConfig
    {
        [SerializeField] private string _turretId;
        [SerializeField] private GameObject _turretPrefab;
        [SerializeField] private int _unlockCost;

        /// <summary>
        /// Stable string key used to match this config to a saved TurretProgress entry.
        /// Falls back to the SO asset name if left blank.
        /// </summary>
        public string TurretId =>
            string.IsNullOrWhiteSpace(_turretId) ? name : _turretId;

        /// <summary>Prefab that is instantiated on a TurretHardpoint at runtime.</summary>
        public GameObject TurretPrefab => _turretPrefab;

        /// <summary>Coin cost to unlock this turret in the shop.</summary>
        public int UnlockCost => _unlockCost;
    }
}

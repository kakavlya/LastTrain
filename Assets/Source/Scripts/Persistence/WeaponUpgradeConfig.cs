using LastTrain.Weapons.Types;
using UnityEngine;

namespace LastTrain.Persistence
{
    [CreateAssetMenu(menuName = "Config/WeaponUpgrade")]
    public class WeaponUpgradeConfig : UpgradeConfig
    {
        [SerializeField] private string _weaponId;
        [SerializeField] private Weapon _weaponPrefab;
        [SerializeField] private int _unblockingCost;

        public string WeaponId =>
            string.IsNullOrWhiteSpace(_weaponId) ? name : _weaponId;

        public Weapon WeaponPrefab => _weaponPrefab;

        public int UnblockingCost => _unblockingCost;
    }
}

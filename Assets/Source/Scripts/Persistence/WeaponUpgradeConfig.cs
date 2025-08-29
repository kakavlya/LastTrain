using UnityEngine;
using LastTrain.Weapons.Types;

namespace LastTrain.Persistence
{
    [CreateAssetMenu(menuName = "Config/WeaponUpgrade")]
    public class WeaponUpgradeConfig : UpgradeConfig
    {
        [SerializeField] private string _weaponId;

        public Weapon WeaponPrefab;
        public int UnblockingCost;

        public string WeaponId =>
            string.IsNullOrWhiteSpace(_weaponId) ? name : _weaponId;
    }
}

using UnityEngine;

namespace LastTrain.Persistence
{
    [CreateAssetMenu(menuName = "Config/HardpointUpgrade")]
    public class HardpointUpgradeConfig : UpgradeConfig
    {
        [SerializeField] private int _hardpointIndex;
        [SerializeField] private int _unlockCost;
        [SerializeField] private float _hpBonusPerLevel = 50f;

        /// <summary>Which slot this config is for (0-3).</summary>
        public int HardpointIndex => _hardpointIndex;

        /// <summary>Coin cost to unlock this hardpoint initially.</summary>
        public int UnlockCost => _unlockCost;

        /// <summary>Amount of Train HP added per level of this hardpoint.</summary>
        public float HpBonusPerLevel => _hpBonusPerLevel;
    }
}

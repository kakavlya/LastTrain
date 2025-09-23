using System;

namespace LastTrain.Persistence
{
    [Serializable]
    public class AoeDamageUpdatingWeaponProgress : SpecialWeaponProgress
    {
        public AoeDamageUpdatingWeaponProgress(string weaponId, int defaultStatLevel = 0)
            : base(weaponId, StatType.AoeDamage, defaultStatLevel)
        {
        }
    }
}

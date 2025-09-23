using System;

namespace LastTrain.Persistence
{
    [Serializable]
    public class AttackSpeedUpdatingWeaponProgress : SpecialWeaponProgress
    {
        public AttackSpeedUpdatingWeaponProgress(string weaponId, int defaultStatLevel = 0)
            : base(weaponId, StatType.AttackSpeed, defaultStatLevel)
        {
        }
    }
}

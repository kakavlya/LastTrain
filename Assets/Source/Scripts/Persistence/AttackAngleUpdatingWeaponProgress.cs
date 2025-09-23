using System;

namespace LastTrain.Persistence
{
    [Serializable]
    public class AttackAngleUpdatingWeaponProgress : SpecialWeaponProgress
    {
        public AttackAngleUpdatingWeaponProgress(string weaponId, int defaultStatLevel = 0)
            : base(weaponId, StatType.AttackAngle, defaultStatLevel)
        {
        }
    }
}

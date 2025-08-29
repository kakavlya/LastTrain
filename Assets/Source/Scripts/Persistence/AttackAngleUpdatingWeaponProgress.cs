using System;

namespace LastTrain.Persistence
{
    [Serializable]
    public class AttackAngleUpdatingWeaponProgress : WeaponProgress
    {
        public int AttackAngleLevel;

        public AttackAngleUpdatingWeaponProgress(string weaponId, int defaultStatLevel = 0) : base(weaponId, defaultStatLevel)
        {
            AttackAngleLevel = defaultStatLevel;
        }

        public override int GetLevel(StatType stat)
        {
            if (stat == StatType.AttackAngle)
            {
                return AttackAngleLevel;
            }

            return base.GetLevel(stat);
        }

        public override int GetSumLevels()
        {
            return base.GetSumLevels() + AttackAngleLevel;
        }

        public override void Increment(StatType stat)
        {
            if (stat == StatType.AttackAngle)
            {
                AttackAngleLevel++;
            }
            else
            {
                base.Increment(stat);
            }
        }
    }
}

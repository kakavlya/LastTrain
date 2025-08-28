using System;

namespace LastTrain.Persistence
{
    [Serializable]
    public class AoeDamageUpdatingWeaponProgress : WeaponProgress
    {
        public int AoeDamageLevel;

        public AoeDamageUpdatingWeaponProgress(string weaponId, int defaultStatLevel = 0) : base(weaponId, defaultStatLevel)
        {
            AoeDamageLevel = defaultStatLevel;
        }

        public override int GetLevel(StatType stat)
        {
            if (stat == StatType.AoeDamage)
            {
                return AoeDamageLevel;
            }

            return base.GetLevel(stat);
        }

        public override int GetSumLevels()
        {
            return base.GetSumLevels() + AoeDamageLevel;
        }

        public override void Increment(StatType stat)
        {
            if (stat == StatType.AoeDamage)
            {
                AoeDamageLevel++;
            }
            else
            {
                base.Increment(stat);
            }
        }
    }
}

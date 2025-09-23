
namespace LastTrain.Persistence
{
    public class SpecialWeaponProgress : WeaponProgress
    {
        public int SpecialStatLevel;
        public StatType SpecialStatType;

        public SpecialWeaponProgress(string weaponId, StatType specialStatType, int defaultStatLevel = 0) :
            base(weaponId, defaultStatLevel)
        {
            SpecialStatLevel = defaultStatLevel;
            SpecialStatType = specialStatType;
        }

        public override int GetLevel(StatType stat)
        {
            if (stat == SpecialStatType)
            {
                return SpecialStatLevel;
            }

            return base.GetLevel(stat);
        }

        public override int GetSumLevels()
        {
            return base.GetSumLevels() + SpecialStatLevel;
        }

        public override void Increment(StatType stat)
        {
            if (stat == SpecialStatType)
            {
                SpecialStatLevel++;
            }
            else
            {
                base.Increment(stat);
            }
        }
    }
}

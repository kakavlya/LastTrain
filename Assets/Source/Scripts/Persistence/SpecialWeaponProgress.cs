namespace LastTrain.Persistence
{
    public class SpecialWeaponProgress : WeaponProgress
    {
        private int _specialStatLevel;
        private StatType _specialStatType;

        public SpecialWeaponProgress(string weaponId, StatType specialStatType, int defaultStatLevel = 0)
            : base(weaponId, defaultStatLevel)
        {
            _specialStatLevel = defaultStatLevel;
            _specialStatType = specialStatType;
        }

        public StatType SpecialStatType => _specialStatType;

        public override int GetLevel(StatType stat)
        {
            if (stat == SpecialStatType)
            {
                return _specialStatLevel;
            }

            return base.GetLevel(stat);
        }

        public override int GetSumLevels()
        {
            return base.GetSumLevels() + _specialStatLevel;
        }

        public override void Increment(StatType stat)
        {
            if (stat == SpecialStatType)
            {
                _specialStatLevel++;
            }
            else
            {
                base.Increment(stat);
            }
        }
    }
}

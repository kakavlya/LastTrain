namespace LastTrain.Persistence
{
    [System.Serializable]
    public class TrainProgress : BaseProgress
    {
        public int HealthLevel;
        public int SlotsLevel;
        public int AmmoLevel;

        public TrainProgress(int defaultStatLevel = 0)
        {
            HealthLevel = defaultStatLevel;
            SlotsLevel = defaultStatLevel;
            AmmoLevel = defaultStatLevel;
        }

        public override int GetLevel(StatType stat)
        {
            switch (stat)
            {
                case StatType.Health:
                    return HealthLevel;

                case StatType.Slots:
                    return SlotsLevel;

                case StatType.Ammo:
                    return AmmoLevel;
            }

            return 0;
        }

        public override int GetSumLevels()
        {
            return HealthLevel + SlotsLevel + AmmoLevel;
        }

        public override void Increment(StatType stat)
        {
            switch (stat)
            {
                case StatType.Health:
                    HealthLevel++;
                    break;

                case StatType.Slots:
                    SlotsLevel++;
                    break;

                case StatType.Ammo:
                    AmmoLevel++;
                    break;
            }
        }
    }
}

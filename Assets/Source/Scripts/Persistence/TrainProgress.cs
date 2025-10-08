using UnityEngine;

namespace LastTrain.Persistence
{
    [System.Serializable]
    public class TrainProgress : BaseProgress
    {
        [SerializeField] private int _healthLevel;
        [SerializeField] private int _slotsLevel;
        [SerializeField] private int _ammoLevel;

        public TrainProgress(int defaultStatLevel = 0)
        {
            _healthLevel = defaultStatLevel;
            _slotsLevel = defaultStatLevel;
            _ammoLevel = defaultStatLevel;
        }

        public int HealthLevel => _healthLevel;

        public int SlotsLevel => _slotsLevel;

        public int AmmoLevel => _ammoLevel;

        public override int GetLevel(StatType stat)
        {
            switch (stat)
            {
                case StatType.Health:
                    return HealthLevel;

                case StatType.Slots:
                    return SlotsLevel;

                case StatType.Ammo:
                    return _ammoLevel;
            }

            return 0;
        }

        public override int GetSumLevels()
        {
            return _healthLevel + _slotsLevel + _ammoLevel;
        }

        public override void Increment(StatType stat)
        {
            switch (stat)
            {
                case StatType.Health:
                    _healthLevel++;
                    break;

                case StatType.Slots:
                    _slotsLevel++;
                    break;

                case StatType.Ammo:
                    _ammoLevel++;
                    break;
            }
        }
    }
}

using System;
using UnityEngine;

namespace LastTrain.Persistence
{
    [Serializable]
    public class WeaponProgress : BaseProgress
    {
        [SerializeField] private string _weaponId;
        [SerializeField] private int _damageLevel;
        [SerializeField] private int _rangeLevel;
        [SerializeField] private bool _isAvailable;

        public string WeaponId => _weaponId;
        public int DamageLevel => _damageLevel;
        public int RangeLevel => _rangeLevel;
        public bool IsAvailable => _isAvailable;

        public WeaponProgress(string weaponId, int defaultStatLevel = 0)
        {
            _weaponId = weaponId;
            _damageLevel = defaultStatLevel;
            _rangeLevel = defaultStatLevel;
        }

        public void SetAvailable(bool available)
        {
            _isAvailable = available;
        }

        public override int GetLevel(StatType stat)
        {
            if (stat == StatType.Damage)
            {
                return _damageLevel;
            }
            else if
                (stat == StatType.Range)
            {
                return _rangeLevel;
            }

            return 0;
        }

        public override int GetSumLevels()
        {
            return _damageLevel + _rangeLevel;
        }

        public override void Increment(StatType stat)
        {
            if (stat == StatType.Damage)
            {
                _damageLevel++;
            }
            else if (stat == StatType.Range)
            {
                _rangeLevel++;
            }
        }
    }
}
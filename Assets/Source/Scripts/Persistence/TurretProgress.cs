using System;
using UnityEngine;

namespace LastTrain.Persistence
{
    [Serializable]
    public class TurretProgress : BaseProgress
    {
        [SerializeField] private string _turretId;
        [SerializeField] private int _damageLevel;
        [SerializeField] private int _fireRateLevel;
        [SerializeField] private bool _isUnlocked;

        public string TurretId => _turretId;
        public int DamageLevel => _damageLevel;
        public int FireRateLevel => _fireRateLevel;
        public bool IsUnlocked => _isUnlocked;

        public TurretProgress(string turretId, int defaultStatLevel = 0)
        {
            _turretId = turretId;
            _damageLevel = defaultStatLevel;
            _fireRateLevel = defaultStatLevel;
        }

        public void SetUnlocked(bool unlocked) => _isUnlocked = unlocked;

        public override int GetLevel(StatType stat)
        {
            return stat switch
            {
                StatType.Damage   => _damageLevel,
                StatType.FireRate => _fireRateLevel,
                _                 => 0,
            };
        }

        public override int GetSumLevels() => _damageLevel + _fireRateLevel;

        public override void Increment(StatType stat)
        {
            switch (stat)
            {
                case StatType.Damage:   _damageLevel++;   break;
                case StatType.FireRate: _fireRateLevel++; break;
            }
        }
    }
}

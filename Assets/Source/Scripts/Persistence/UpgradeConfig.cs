using System;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;

namespace LastTrain.Persistence
{
    public abstract class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private string _localizationKey;
        [SerializeField] private StatConfig[] _statConfigs;

        public Sprite Icon;

        public string Name
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_localizationKey) && LocalizationManager.HasKey(_localizationKey))
                {
                    return LocalizationManager.Localize(_localizationKey);
                }

                return string.IsNullOrWhiteSpace(_name) ? name : _name;
            }
        }

        public StatConfig[] StatConfigs => _statConfigs;

        public float GetStat(StatType stat, int level)
        {
            StatConfig config = FindStat(stat);

            if (config != null)
            {
                return config.GetValue(level);
            }
            else
            {
                return 0;
            }
        }

        public int GetCost(StatType stat, int level)
        {
            StatConfig config = FindStat(stat);

            if (config != null)
            {
                return config.GetCost(level);
            }
            else
            {
                return 0;
            }
        }

        public int GetMaxLevel(StatType stat)
        {
            StatConfig config = FindStat(stat);

            if (config != null)
            {
                return config.MaxLevel;
            }
            else
            {
                return 0;
            }
        }

        public StatConfig FindStat(StatType type)
        {
            if (_statConfigs == null)
                return null;

            foreach (StatConfig config in _statConfigs)
            {
                if (config != null && config.StatType == type)
                    return config;
            }

            return null;
        }

        public bool TryFindStat(StatType statType)
        {
            return Array.Exists(_statConfigs, s => s.StatType == statType);
        }
    }

    public enum StatType
    {
        Damage,
        Range,
        Ammo,
        AttackSpeed,
        AttackAngle,
        AoeDamage,
        Health,
        Slots
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeConfig : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private StatConfig[] _statConfigs;

    public Sprite Icon;

    public string Name =>
    string.IsNullOrWhiteSpace(_name) ? name : _name;

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
}

public enum StatType
{
    Damage,
    Range,
    Health,
    Slots
}

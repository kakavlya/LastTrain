using UnityEngine;

[CreateAssetMenu(menuName = "Config/WeaponUpgrade")]
public class WeaponUpgradeConfig : ScriptableObject
{
    [SerializeField] private string _weaponId;
    [SerializeField] private string _weaponName;
    [SerializeField] private StatConfig[] _statConfigs;

    public Weapon WeaponPrefab;
    public Sprite Icon;
    public int UnblockingCost;

    public string WeaponId =>
        string.IsNullOrWhiteSpace(_weaponId) ? name : _weaponId;

    public string WeaponName =>
        string.IsNullOrWhiteSpace(_weaponName) ? _weaponName : _weaponName;


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
    Range
}

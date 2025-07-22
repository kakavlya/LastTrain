using UnityEngine;

[CreateAssetMenu(menuName = "Config/WeaponUpgrade")]
public class WeaponUpgradeConfig : ScriptableObject
{
    [SerializeField] string _weaponId;

    public string WeaponId => 
        string.IsNullOrWhiteSpace(_weaponId) ? name : _weaponId;

    public AnimationCurve DamageCurve; // 0 - 1
    public AnimationCurve RangeCurve;  // 0 - 1 

    public int MaxDamageLevel = 10;
    public int MaxRangeLevel = 10;

    public int[] DamageCosts;
    public int[] RangeCosts;  

    public float DamageMin, DamageMax;
    public float RangeMin, RangeMax;

    public Sprite Icon;

    public float GetStat(StatType stat, int level)
    {
        float t = level / (float)GetMaxLevel(stat);
        return Mathf.Lerp(stat == StatType.Damage ? DamageMin : RangeMin,
                          stat == StatType.Damage ? DamageMax : RangeMax,
                          (stat == StatType.Damage ? DamageCurve : RangeCurve).Evaluate(t));
    }

    public int GetCost(StatType stat, int level) =>
        stat == StatType.Damage ? DamageCosts[level] : RangeCosts[level];

    public int GetMaxLevel(StatType stat) =>
        stat == StatType.Damage ? MaxDamageLevel : MaxRangeLevel;
}

public enum StatType { Damage, Range }

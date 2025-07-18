using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Weapon Upgrade Config")]
public class WeaponUpgradeConfig : ScriptableObject
{
    public string WeaponId;

    [Header("Damage Range")]
    public int MinDamage = 10;
    public int MaxDamage = 300;

    [Header("Range (units)")]
    public float MinRange = 1f;
    public float MaxRange = 5f;

    [Header("Cost")]
    public int BaseCost = 100;
    public float CostMultiplier = 1.5f;

    [Header("Stats")]
    public AnimationCurve DamageCurve;
    public AnimationCurve RangeCurve;

    [Header("Limits")]
    public int MaxLevel = 10;

    [Header("Icon")]
    public Sprite Icon;

    public int GetCost(int nextLevel)
    {
        if(nextLevel < 1 || nextLevel > MaxLevel)
            return 0;
        return Mathf.RoundToInt(BaseCost * Mathf.Pow(CostMultiplier, nextLevel - 1));
    }

    public int GetDamage(int level)
    {
        level = Mathf.Clamp(level, 1, MaxLevel);
        float t = (level - 1f) / (MaxLevel - 1f);    // 0.1      
        float pct = DamageCurve.Evaluate(t);         // 0.1    
        return Mathf.RoundToInt(Mathf.Lerp(MinDamage, MaxDamage, pct));
    }

    public float GetRange(int level)
    {
        level = Mathf.Clamp(level, 1, MaxLevel);
        float t = (level - 1f) / (MaxLevel - 1f);
        float pct = RangeCurve.Evaluate(t);
        return Mathf.Lerp(MinRange, MaxRange, pct);
    }
}

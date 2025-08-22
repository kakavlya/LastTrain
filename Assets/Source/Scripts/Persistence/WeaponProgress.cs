using System;

[Serializable]
public class WeaponProgress : BaseProgress
{
    public string WeaponId;
    public int DamageLevel;
    public int RangeLevel;
    public bool IsAvailable;

    public WeaponProgress(string weaponId, int defaultStatLevel = 0)
    {
        WeaponId = weaponId;
        DamageLevel = defaultStatLevel;
        RangeLevel = defaultStatLevel;
    }

    public override int GetLevel(StatType stat) =>
        stat == StatType.Damage ? DamageLevel : RangeLevel;

    public override int GetSumLevels()
    {
        return DamageLevel + RangeLevel;
    }

    public override void Increment(StatType stat)
    {
        if (stat == StatType.Damage) DamageLevel++;
        else RangeLevel++;
    }
}
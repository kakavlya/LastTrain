using System;

[Serializable]
public class AttackSpeedUpdatingWeaponProgress : WeaponProgress
{
    public int AttackSpeedLevel;

    public AttackSpeedUpdatingWeaponProgress(string weaponId, int defaultStatLevel = 0) : base(weaponId, defaultStatLevel)
    {
        AttackSpeedLevel = defaultStatLevel;
    }

    public override int GetLevel(StatType stat)
    {
        if (stat == StatType.AttackSpeed)
        {
            return AttackSpeedLevel;
        }

        return base.GetLevel(stat);
    }
        
    public override int GetSumLevels()
    {
        return base.GetSumLevels() + AttackSpeedLevel;
    }

    public override void Increment(StatType stat)
    {
        if (stat == StatType.AttackSpeed)
        {
            AttackSpeedLevel++;
        }
        else
        {
            base.Increment(stat);
        }
    }
}

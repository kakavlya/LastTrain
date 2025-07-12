using System;

[Serializable]
public class WeaponProgress
{
    public string WeaponId;
    public int UpgradeLevel;

    public WeaponProgress(string weaponId, int initialLevel = 0)
    {
        WeaponId = weaponId;
        UpgradeLevel = initialLevel;
    }
}
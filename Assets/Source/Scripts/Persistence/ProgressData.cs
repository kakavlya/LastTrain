using System.Collections.Generic;

[System.Serializable]
public class ProgressData
{
    public List<WeaponProgress> Weapons = new List<WeaponProgress>();
    public List<int> UnlockedLevels = new List<int>();    
    public int Coins;

    public ProgressData()
    {
        // All weapons start at level 1
        Weapons.Add(new WeaponProgress("Rifle", 1));
        Weapons.Add(new WeaponProgress("MachineGun", 1));
        Weapons.Add(new WeaponProgress("GrenadeLauncher", 1));
        Weapons.Add(new WeaponProgress("Flamethrower", 1));

        // Unlocking 1 level on start
        UnlockedLevels.Add(1);
        Coins = 0;
    }
}

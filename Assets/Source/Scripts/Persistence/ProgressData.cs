using System.Collections.Generic;

[System.Serializable]
public class ProgressData
{
    public List<WeaponProgress> Weapons = new List<WeaponProgress>();
    public List<int> UnlockedLevels = new List<int>();    
    public int Coins;
    public float EffectsVolume;
    public float MusicVolume;
    public int InventorySlotsCount;
    public int PlayerInventorySlotsCount;
    public List<string> InventorySlots = new List<string>();
    public List<string> PlayerInventorySlots = new List<string>();
    public List<LevelAvailability> LevelsAvailability = new List<LevelAvailability>();

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
        EffectsVolume = 0.5f;
        MusicVolume = 0.5f;
        InventorySlotsCount = 5;
        PlayerInventorySlotsCount = 3;

        for (int i = 0; i < InventorySlotsCount; i++)
            InventorySlots.Add("");

        for (int i = 0; i < PlayerInventorySlotsCount; i++)
            PlayerInventorySlots.Add("");
    }
}

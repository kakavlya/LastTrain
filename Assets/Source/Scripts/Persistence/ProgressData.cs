using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressData
{
    public List<WeaponProgress> Weapons = new List<WeaponProgress>();
    public List<int> UnlockedLevels = new List<int>();
    public int Coins;
    public float MusicVolume;
    public float EffectsVolume;
    public int InventorySlotsCount;
    public List<string> InventorySlots = new List<string>();

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
        MusicVolume = 0.5f;
        EffectsVolume = 0.5f;
        InventorySlotsCount = 3;

        for (int i = 0; i < InventorySlotsCount; i++)
            InventorySlots.Add("");
    }
}

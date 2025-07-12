using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressData : MonoBehaviour
{
    public List<WeaponProgress> Weapons = new List<WeaponProgress>();
    public List<int> UnlockedLevels = new List<int>();    
    public int Coins;

    public ProgressData()
    {
        
        Weapons.Add(new WeaponProgress("Rifle"));
        Weapons.Add(new WeaponProgress("MachineGun"));
        Weapons.Add(new WeaponProgress("GrenadeLauncher"));
        Weapons.Add(new WeaponProgress("Flamethrower"));

        // Unlocking 1 level on start
        UnlockedLevels.Add(1);
        Coins = 0;
    }
}

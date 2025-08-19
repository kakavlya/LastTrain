using System.Collections.Generic;

[System.Serializable]
public class ProgressData
{
    public List<WeaponProgress> WeaponsProgress = new List<WeaponProgress>(); 
    public List<WeaponProgress> TrainingWeaponsProgress = new List<WeaponProgress>();
    public List<TrainProgress> HealthProgress = new List<TrainProgress>();
    public int Coins;
    public float EffectsVolume;
    public float MusicVolume;
    public int InventorySlotsCount;
    public int PlayerInventorySlotsCount;
    public List<string> InventorySlots = new List<string>();
    public List<string> PlayerInventorySlots = new List<string>();
    public List<LevelAvailability> LevelsAvailability = new List<LevelAvailability>();
    public bool IsDoneGameplayTraining = true;
    public bool IsDoneMenuTraining;

    public ProgressData()
    {
        WeaponsProgress.Add(new WeaponProgress("Rifle", 0));
        WeaponsProgress.Add(new WeaponProgress("MachineGun", 0));
        WeaponsProgress.Add(new WeaponProgress("GrenadeLauncher", 0));
        WeaponsProgress.Add(new WeaponProgress("Flamethrower", 0));
        WeaponsProgress.Add(new WeaponProgress("Crossbow", 0));
        WeaponsProgress.Add(new WeaponProgress("Shotgun", 0));

        TrainingWeaponsProgress.Add(new WeaponProgress("Rifle", 5));
        TrainingWeaponsProgress.Add(new WeaponProgress("MachineGun", 5));
        TrainingWeaponsProgress.Add(new WeaponProgress("GrenadeLauncher", 5));

        HealthProgress.Add(new TrainProgress(0));

        Coins = 100000;
        EffectsVolume = 0.5f;
        MusicVolume = 0.5f;
        InventorySlotsCount = 0;
        PlayerInventorySlotsCount = 1;

        for (int i = 0; i < InventorySlotsCount; i++)
            InventorySlots.Add("");

        for (int i = 0; i < PlayerInventorySlotsCount; i++)
            PlayerInventorySlots.Add("");
    }
}

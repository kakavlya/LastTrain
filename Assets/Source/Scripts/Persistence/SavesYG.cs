using System.Collections.Generic;
using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        [SerializeReference]
        public List<WeaponProgress> WeaponsProgress = new List<WeaponProgress>();

        public List<WeaponProgress> TrainingWeaponsProgress = new List<WeaponProgress>();
        public TrainProgress TrainProgress = new TrainProgress(0);
        public int Coins;
        public float EffectsVolume;
        public float MusicVolume;
        public int InventorySlotsCount;
        public int PlayerInventorySlotsCount;
        public List<string> InventorySlots = new List<string>();
        public List<string> PlayerInventorySlots = new List<string>();
        public List<LevelAvailability> LevelsAvailability = new List<LevelAvailability>();
        public bool IsDoneGameplayTraining;
        public bool IsDoneMenuTraining;
        public MenuTrainingState TrainingState = MenuTrainingState.Start;

        public SavesYG()
        {
            WeaponsProgress.Add(new AttackSpeedUpdatingWeaponProgress("SubmachineGun", 0));
            WeaponsProgress.Add(new AttackSpeedUpdatingWeaponProgress("MachineGun", 0));
            WeaponsProgress.Add(new AoeDamageUpdatingWeaponProgress("GrenadeLauncher", 0));
            WeaponsProgress.Add(new AttackAngleUpdatingWeaponProgress("Flamethrower", 0));
            WeaponsProgress.Add(new AttackSpeedUpdatingWeaponProgress("Crossbow", 0));
            WeaponsProgress.Add(new AttackAngleUpdatingWeaponProgress("Shotgun", 0));

            TrainingWeaponsProgress.Add(new AttackSpeedUpdatingWeaponProgress("SubmachineGun", 5));
            TrainingWeaponsProgress.Add(new AttackSpeedUpdatingWeaponProgress("MachineGun", 5));
            TrainingWeaponsProgress.Add(new AoeDamageUpdatingWeaponProgress("GrenadeLauncher", 5));

            Coins = 0;
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
}

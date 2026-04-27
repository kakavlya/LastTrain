using LastTrain.Level;
using LastTrain.Persistence;
using LastTrain.Training;
using System.Collections.Generic;
using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        // ── Schema version ──────────────────────────────────────────────────
        // 0 = original save format (no turrets)
        // 1 = turret progress added (global)
        // 2 = turrets moved to per-hardpoint progress
        public int SaveVersion;

        // ── Weapons ─────────────────────────────────────────────────────────
        [SerializeReference]
        public List<WeaponProgress> WeaponsProgress = new List<WeaponProgress>();
        public List<WeaponProgress> TrainingWeaponsProgress = new List<WeaponProgress>();

        // ── Train ────────────────────────────────────────────────────────────
        public TrainProgress TrainProgress = new TrainProgress(0);

        // ── Hardpoints & Turrets ─────────────────────────────────────────────
        public List<HardpointProgress> HardpointsProgress = new List<HardpointProgress>();

        // ── General ──────────────────────────────────────────────────────────
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
            SaveVersion = 1;

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
                InventorySlots.Add(string.Empty);

            for (int i = 0; i < PlayerInventorySlotsCount; i++)
                PlayerInventorySlots.Add(string.Empty);
        }

        // ── Migration ────────────────────────────────────────────────────────
        /// <summary>
        /// Call once after save data is loaded (e.g. from ProgressHandler.Start).
        /// Each version block runs exactly once on saves that pre-date that version,
        /// then bumps SaveVersion and persists the change.
        /// Returns true if any migration was applied (caller should call YG2.SaveProgress).
        /// </summary>
        public static bool MigrateIfNeeded()
        {
            bool dirty = false;
            var saves = YG2.saves;

            // v0 → v1: turret list did not exist
            if (saves.SaveVersion < 1)
            {
                // TurretsProgress will already be an empty list (JsonUtility default).
                // No turrets to pre-populate yet — Phase 2 will add unlock defaults here
                // when the first TurretUpgradeConfig assets are authored.
                saves.SaveVersion = 1;
                dirty = true;
            }

            // v1 → v2: migrated to per-hardpoint turrets.
            // Wipes any global turret data from v1 (which was just prototyping anyway)
            // and initializes the 4 locked hardpoints.
            if (saves.SaveVersion < 2)
            {
                if (saves.HardpointsProgress == null)
                    saves.HardpointsProgress = new List<HardpointProgress>();

                if (saves.HardpointsProgress.Count == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        saves.HardpointsProgress.Add(new HardpointProgress(i, defaultUnlocked: false));
                    }
                }

                saves.SaveVersion = 2;
                dirty = true;
            }

            return dirty;
        }
    }
}

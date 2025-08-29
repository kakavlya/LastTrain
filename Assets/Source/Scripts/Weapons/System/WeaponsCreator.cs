using System.Collections.Generic;
using UnityEngine;
using YG;
using LastTrain.AmmunitionSystem;
using LastTrain.Data;
using LastTrain.Persistence;
using LastTrain.Weapons.Types;

namespace LastTrain.Weapons.System
{
    public class WeaponCreator : MonoBehaviour
    {
        [SerializeField] private SharedData _sharedData;

        private float _ammoPercent;

        public void Init()
        {
            _ammoPercent = GetAmmoPercent();
        }

        public Weapon[] CreateWeapons()
        {
            List<WeaponProgress> weaponProgresses = GetWeaponProgressType();
            var weaponConfigs = _sharedData.WeaponConfigs;
            Weapon[] weapons = new Weapon[weaponConfigs.Count];

            for (int i = 0; i < weaponConfigs.Count; i++)
            {
                var config = weaponConfigs[i];
                WeaponProgress weaponProgress = weaponProgresses.Find(weapon => weapon.WeaponId == config.WeaponId);
                float damage = weaponConfigs[i].GetStat(StatType.Damage, weaponProgress.DamageLevel);
                float range = weaponConfigs[i].GetStat(StatType.Range, weaponProgress.RangeLevel);
                float? fireDelay = null;
                float? fireAngle = null;
                float? aoeDamage = null;

                if (weaponProgress is AttackSpeedUpdatingWeaponProgress attackSpeedProgress &&
                    config.TryFindStat(StatType.AttackSpeed))
                {
                    fireDelay = 1f / weaponConfigs[i].GetStat(StatType.AttackSpeed, attackSpeedProgress.AttackSpeedLevel);
                }

                if (weaponProgress is AttackAngleUpdatingWeaponProgress attackAngleProgress &&
                    config.TryFindStat(StatType.AttackAngle))
                {
                    fireAngle = weaponConfigs[i].GetStat(StatType.AttackAngle, attackAngleProgress.AttackAngleLevel);
                }

                if (weaponProgress is AoeDamageUpdatingWeaponProgress aeoDamageProgress &&
                    config.TryFindStat(StatType.AoeDamage))
                {
                    aoeDamage = weaponConfigs[i].GetStat(StatType.AoeDamage, aeoDamageProgress.AoeDamageLevel);
                }

                Weapon weaponInstance = Instantiate(weaponConfigs[i].WeaponPrefab, transform);
                weaponInstance.Init(damage, range, fireDelay, fireAngle, aoeDamage);
                weaponInstance.SetPrefabReference(weaponConfigs[i].WeaponPrefab);
                weaponInstance.gameObject.SetActive(false);
                weapons[i] = weaponInstance;
            }

            return weapons;
        }

        public Dictionary<Weapon, Ammunition> CreateAmmunitionDictionary(Weapon[] weapons, Ammunition[] ammunitionPrefabs)
        {
            Dictionary<Weapon, Ammunition> weaponAmmoDictionary = new Dictionary<Weapon, Ammunition>();

            foreach (var weapon in weapons)
            {
                foreach (var ammoPrefab in ammunitionPrefabs)
                {
                    if (ammoPrefab.WeaponPrefab == weapon.PrefabReference)
                    {
                        var ammoInstance = Instantiate(ammoPrefab, transform);
                        ammoInstance.Init(_ammoPercent);
                        weaponAmmoDictionary[weapon] = ammoInstance;
                        break;
                    }
                }
            }

            return weaponAmmoDictionary;
        }

        private List<WeaponProgress> GetWeaponProgressType()
        {
            if (YG2.saves.IsDoneGameplayTraining)
            {
                return YG2.saves.WeaponsProgress;
            }
            else
            {
                return YG2.saves.TrainingWeaponsProgress;
            }
        }

        private float GetAmmoPercent()
        {
            var trainConfig = _sharedData.TrainUpgradeConfig.StatConfigs;
            var ammoLevel = YG2.saves.TrainProgress.AmmoLevel;
            StatConfig ammoConfig = null;

            foreach (var config in trainConfig)
            {
                if (config.StatType == StatType.Ammo)
                {
                    ammoConfig = config;
                }
            }

            return ammoConfig.GetValue(ammoLevel);
        }
    }
}
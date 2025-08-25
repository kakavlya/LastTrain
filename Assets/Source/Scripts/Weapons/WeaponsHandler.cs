using System;
using System.Collections.Generic;
using Assets.Source.Scripts.Weapons;
using UnityEngine;
using YG;

public class WeaponsHandler : MonoBehaviour
{
    [SerializeField] private WeaponUI[] _uiCells;
    [SerializeField] private Ammunition[] _ammunitions;
    [SerializeField] private PlayerInput _weaponInput;
    [SerializeField] private SharedData _sharedData;

    private Weapon[] _weapons;
    private Weapon _currentWeapon;
    private int _currentNumberWeapon;
    private Dictionary<Weapon, Ammunition> _weaponAmmoDictonary;

    public event Action<Weapon> OnWeaponChange;

    public void Init()
    {
        CreateWeapons();
        FillAmmoDictonary();

        foreach (var cell in _uiCells)
        {
            cell.gameObject.SetActive(false);
        }

        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].gameObject.SetActive(false);
            _uiCells[i].gameObject.SetActive(true);
            _uiCells[i].UconClicked += ChangeWeapon;

            if (_weaponAmmoDictonary.TryGetValue(_weapons[i], out Ammunition ammunition))
            {
                _uiCells[i].ActivateWeapon(_weapons[i], ammunition);
            }
            else
            {
                _uiCells[i].ActivateWeapon(_weapons[i], null);
            }

            _uiCells[i].DeactivateWeapon(_weapons[i]);
        }


        _currentNumberWeapon = 0;
        _currentWeapon = _weapons[0];
        _weapons[0].gameObject.SetActive(true);

        if (_weaponAmmoDictonary.TryGetValue(_currentWeapon, out Ammunition ammo))
        {
            _uiCells[0].ActivateWeapon(_currentWeapon, ammo);
        }
        else
        {
            _uiCells[0].ActivateWeapon(_currentWeapon, null);
        }

        OnWeaponChange?.Invoke(_currentWeapon);
        _weaponInput.WeaponChanged += ChangeWeapon;
        _weaponInput.Fired += HandleFire;
        _weaponInput.StopFired += HandleStopFire;

    }

    private void OnDisable()
    {
        foreach (var cell in _uiCells)
        {
            cell.UconClicked -= ChangeWeapon;
        }

        _weaponInput.WeaponChanged -= ChangeWeapon;
        _weaponInput.Fired -= HandleFire;
        _weaponInput.StopFired -= HandleStopFire;
    }

    private void ChangeWeapon(int weaponNumber)
    {
        if (weaponNumber > 0 && weaponNumber <= _weapons.Length)
        {
            _currentWeapon.gameObject.SetActive(false);

            if (_currentNumberWeapon >= 0 && _currentNumberWeapon < _weapons.Length)
            {
                _uiCells[_currentNumberWeapon].DeactivateWeapon(_currentWeapon);
            }

            _currentNumberWeapon = weaponNumber - 1;
            _currentWeapon = _weapons[_currentNumberWeapon];
            _currentWeapon.gameObject.SetActive(true);

            if (_weaponAmmoDictonary.TryGetValue(_currentWeapon, out Ammunition ammo))
            {
                _uiCells[_currentNumberWeapon].ActivateWeapon(_currentWeapon, ammo);
            }
            else
            {
                _uiCells[_currentNumberWeapon].ActivateWeapon(_currentWeapon, null);
            }

            OnWeaponChange?.Invoke(_currentWeapon);
        }
    }

    private void HandleFire()
    {
        if (_currentWeapon == null)
            return;

        if (_weaponAmmoDictonary.TryGetValue(_currentWeapon, out Ammunition ammo))
        {
            if (ammo.HasAmmo)
            {
                _currentWeapon.Fire(ammo);
            }
            else
            {
                _currentWeapon.StopFire();
            }
        }
        else
        {
            _currentWeapon.Fire();
        }
    }

    private void HandleStopFire()
    {
        _currentWeapon?.StopFire();
    }

    private void CreateWeapons()
    {
        List<WeaponProgress> weaponProgresses = GetWeaponProgressType();

        var weaponConfigs = _sharedData.WeaponConfigs;
        _weapons = new Weapon[weaponConfigs.Count];

        for (int i = 0; i < weaponConfigs.Count; i++)
        {
            var config = weaponConfigs[i];
            WeaponProgress weaponProgress = weaponProgresses.Find(weapon => weapon.WeaponId == config.WeaponId);

            if (weaponProgress == null)
            {
                weaponProgress = new WeaponProgress(config.WeaponId, 0);
                weaponProgresses.Add(weaponProgress);
            }

            float damage = weaponConfigs[i].GetStat(StatType.Damage, weaponProgress.DamageLevel);
            float range = weaponConfigs[i].GetStat(StatType.Range, weaponProgress.RangeLevel);

            Weapon weaponInstance = Instantiate(weaponConfigs[i].WeaponPrefab, transform);
            weaponInstance.Init(damage, range);
            weaponInstance.SetPrefabReference(weaponConfigs[i].WeaponPrefab);

            weaponInstance.gameObject.SetActive(false);
            _weapons[i] = weaponInstance;
        }
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

    private void FillAmmoDictonary()
    {
        _weaponAmmoDictonary = new Dictionary<Weapon, Ammunition>();

        foreach (var weapon in _weapons)
        {
            foreach (var ammoPrefab in _ammunitions)
            {
                if (ammoPrefab.WeaponPrefab == weapon.PrefabReference)
                {
                    var ammoInstance = Instantiate(ammoPrefab, transform);
                    _weaponAmmoDictonary[weapon] = ammoInstance;
                    break;
                }
            }
        }
    }
}

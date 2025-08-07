using System;
using System.Collections.Generic;
using Assets.Source.Scripts.Weapons;
using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    [SerializeField] private WeaponUI[] cells;
    [SerializeField] private Weapon[] _weaponPrefabs;
    [SerializeField] private Ammunition[] _ammunitions;
    [SerializeField] private PlayerInput _weaponInput;

    private Weapon[] _weapons;
    private Weapon _currentWeapon;
    private int _currentNumberWeapon;
    private Dictionary<Weapon, Ammunition> _weaponAmmoDictonary;

    public event Action<Weapon> OnWeaponChange;

    public void Init()
    {
        CreateWeapons();
        FillAmmoDictonary();

        foreach (var cell in cells)
        {
            cell.gameObject.SetActive(false);
        }

        for (int i = 0;  i < _weapons.Length; i++)
        {
            _weapons[i].gameObject.SetActive(false);
            cells[i].gameObject.SetActive(true);
            cells[i].UconClicked += ChooseWeapon;
            cells[i].ActivateWeapon(_weapons[i]);
            cells[i].DeactivateWeapon(_weapons[i]);
        }


        _currentNumberWeapon = 0;
        _currentWeapon = _weapons[0];
        _weapons[0].gameObject.SetActive(true);
        cells[0].ActivateWeapon(_currentWeapon);
        OnWeaponChange?.Invoke(_currentWeapon);
        _weaponInput.WeaponChanged += ChooseWeapon;
        _weaponInput.Fired += HandleFire;
        _weaponInput.StopFired += HandleStopFire;

    }

    private void OnDisable()
    {
        foreach (var cell in cells)
        {
            cell.UconClicked -= ChooseWeapon;
        }

        _weaponInput.WeaponChanged -= ChooseWeapon;
        _weaponInput.Fired -= HandleFire;
        _weaponInput.StopFired -= HandleStopFire;
    }

    private void ChooseWeapon(int weaponNumber)
    {
        if (weaponNumber > 0 && weaponNumber <= _weapons.Length)
        {
            _currentWeapon.gameObject.SetActive(false);

            if (_currentNumberWeapon >= 0 && _currentNumberWeapon < _weapons.Length)
            {
            cells[_currentNumberWeapon].DeactivateWeapon(_currentWeapon);

            }

            _currentNumberWeapon = weaponNumber - 1;
            _currentWeapon = _weapons[_currentNumberWeapon];
            _currentWeapon.gameObject.SetActive(true);
            cells[_currentNumberWeapon].ActivateWeapon(_currentWeapon);
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
                _currentWeapon.Fire();
                ammo.DecreaseProjectilesCount();
            }
            else
            {
                _currentWeapon.StopFire();
            }
        }

        _currentWeapon?.Fire();
    }

    private void HandleStopFire()
    {
        _currentWeapon?.StopFire();
    }

    private void CreateWeapons()
    {
        _weapons = new Weapon[_weaponPrefabs.Length];

        for (int i = 0; i < _weaponPrefabs.Length; i++)
        {
            Weapon weaponInstance = Instantiate(_weaponPrefabs[i], transform);
            weaponInstance.SetPrefabReference(_weaponPrefabs[i]);
            weaponInstance.gameObject.SetActive(false);
            _weapons[i] = weaponInstance;
        }
    }

    private void FillAmmoDictonary()
    {
        _weaponAmmoDictonary = new Dictionary<Weapon, Ammunition>();

        foreach (var weapon in _weapons)
        {
            foreach (var ammo in _ammunitions)
            {
                if (ammo.WeaponPrefab == weapon.PrefabReference)
                {
                    _weaponAmmoDictonary[weapon] = ammo;
                    break;
                }
            }
        }
    }
}

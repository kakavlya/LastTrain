using System;
using Assets.Source.Scripts.Weapons;
using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    [SerializeField] private WeaponCell[] _weaponCells;
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private WeaponInput _weaponInput;

    private Weapon _currentWeapon;

    public event Action<Weapon> OnWeaponChange;

    private void OnEnable()
    {
        foreach (var cell in _weaponCells)
        {
            cell.UconClicked += ChooseWeapon;
        }

        _weaponInput.WeaponChanged += ChooseWeapon;
    }

    private void OnDisable()
    {
        foreach (var cell in _weaponCells)
        {
            cell.UconClicked -= ChooseWeapon;
        }

        _weaponInput.WeaponChanged -= ChooseWeapon;
    }

    private void Start()
    {
        foreach (var weapon in _weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        _currentWeapon = _weapons[0];
        _weapons[0].gameObject.SetActive(true);
        OnWeaponChange?.Invoke(_currentWeapon);
    }

    private void ChooseWeapon(int weaponNumber)
    {
        if (weaponNumber > 0 && weaponNumber <= _weapons.Length)
        {
            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = _weapons[weaponNumber - 1];
            _currentWeapon.gameObject.SetActive(true);
            _weaponCells[weaponNumber - 1].SetIcon(_currentWeapon);
            OnWeaponChange?.Invoke(_currentWeapon);
        }
    }
}

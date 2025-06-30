using System;
using Assets.Source.Scripts.Weapons;
using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    [SerializeField] private WeaponUI[] _weaponUI;
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private WeaponInput _weaponInput;

    private Weapon _currentWeapon;
    private int _currentNumberWeapon;

    public event Action<Weapon> OnWeaponChange;

    public void Init()
    {
        foreach (var cell in _weaponUI)
        {
            cell.UconClicked += ChooseWeapon;
        }

        _weaponInput.WeaponChanged += ChooseWeapon;
    }

    private void OnDisable()
    {
        foreach (var cell in _weaponUI)
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

            if (_currentNumberWeapon > 0 && _currentNumberWeapon <= _weapons.Length)
            {
            _weaponUI[_currentNumberWeapon - 1].DeactivateWeapon(_currentWeapon);

            }

            _currentNumberWeapon = weaponNumber;
            _currentWeapon = _weapons[weaponNumber - 1];
            _currentWeapon.gameObject.SetActive(true);
            _weaponUI[weaponNumber - 1].ActivateWeapon(_currentWeapon);
            OnWeaponChange?.Invoke(_currentWeapon);
        }
    }
}

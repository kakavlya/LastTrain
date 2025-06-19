using System;
using Assets.Source.Scripts.Weapons;
using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private WeaponInput _weaponInput;

    private Weapon _currentWeapon;

    public event Action <Weapon> OnWeaponChange;

    private void OnEnable()
    {
        _weaponInput.WeaponChanged += ChangeWeapon;
    }

    private void OnDisable()
    {
        _weaponInput.WeaponChanged -= ChangeWeapon;
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

    private void ChangeWeapon(int weaponNumber)
    {
        if (weaponNumber > 0)
        {
            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = _weapons[weaponNumber - 1];
            _currentWeapon.gameObject.SetActive(true);
            OnWeaponChange?.Invoke(_currentWeapon);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using LastTrain.AmmunitionSystem;
using LastTrain.Player;
using LastTrain.UI.Gameplay;
using LastTrain.Weapons.Types;

namespace LastTrain.Weapons.System
{
    public class WeaponsHandler : MonoBehaviour
    {
        [SerializeField] private WeaponUI[] _uiCells;
        [SerializeField] private Ammunition[] _ammunitions;
        [SerializeField] private PlayerInput _weaponInput;
        [SerializeField] private WeaponCreator _weaponCreator;

        private Weapon[] _weapons;
        private Weapon _currentWeapon;
        private int _currentNumberWeapon;
        private Dictionary<Weapon, Ammunition> _weaponAmmoDictonary;

        public event Action<Weapon> OnWeaponChange;

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

        public void Init()
        {
            _weaponCreator.Init();
            _weapons = _weaponCreator.CreateWeapons();
            _weaponAmmoDictonary = _weaponCreator.CreateAmmunitionDictionary(_weapons, _ammunitions);

            SetupUI();

            _currentNumberWeapon = 0;
            _currentWeapon = _weapons[0];
            _weapons[0].gameObject.SetActive(true);

            ActivateCurrentWeaponUI();

            OnWeaponChange?.Invoke(_currentWeapon);
            _weaponInput.WeaponChanged += ChangeWeapon;
            _weaponInput.Fired += HandleFire;
            _weaponInput.StopFired += HandleStopFire;
        }

        private void SetupUI()
        {
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

                ActivateCurrentWeaponUI();

                OnWeaponChange?.Invoke(_currentWeapon);
            }
        }

        private void ActivateCurrentWeaponUI()
        {
            if (_weaponAmmoDictonary.TryGetValue(_currentWeapon, out Ammunition ammo))
            {
                _uiCells[_currentNumberWeapon].ActivateWeapon(_currentWeapon, ammo);
            }
            else
            {
                _uiCells[_currentNumberWeapon].ActivateWeapon(_currentWeapon, null);
            }
        }

        private void HandleFire(Vector3 target)
        {
            if (_currentWeapon == null)
                return;

            if (_weaponAmmoDictonary.TryGetValue(_currentWeapon, out Ammunition ammo))
            {
                if (ammo.HasAmmo)
                {
                    _currentWeapon.Fire(ammo, target);
                }
                else
                {
                    _currentWeapon.StopFire();
                }
            }
            else
            {
                _currentWeapon.Fire(null, target);
            }
        }

        private void HandleStopFire()
        {
            _currentWeapon?.StopFire();
        }
    }
}
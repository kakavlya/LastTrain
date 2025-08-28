using System;
using UnityEngine;

namespace LastTrain.Ammunition
{
    public class Ammunition : MonoBehaviour
    {
        [SerializeField] private Weapon _weaponPrefab;
        [SerializeField] private int _maxAmmo;

        private int _currentAmmo;

        public event Action<int> Updated;
        public event Action<int> AmmoAdded;

        public Weapon WeaponPrefab => _weaponPrefab;

        public bool HasAmmo { get; private set; } = true;

        public int CurrentAmmo { get; private set; }


        public void Init(float ammoPercent)
        {
            _currentAmmo = (int)(_maxAmmo * ammoPercent / 100f);
            CurrentAmmo = _currentAmmo;
            Updated?.Invoke(CurrentAmmo);
        }

        public void DecreaseProjectilesCount()
        {
            if (CurrentAmmo > 0)
            {
                CurrentAmmo--;
            }
            else
            {
                HasAmmo = false;
            }

            Updated?.Invoke(CurrentAmmo);
        }

        public void IncreaseProjectilesCount(int addedCount)
        {
            if (CurrentAmmo + addedCount < _currentAmmo)
            {
                CurrentAmmo += addedCount;
            }
            else
            {
                CurrentAmmo = _currentAmmo;
            }

            if (CurrentAmmo > 0)
            {
                HasAmmo = true;
            }

            Updated?.Invoke(CurrentAmmo);
            AmmoAdded?.Invoke(addedCount);
        }
    }
}
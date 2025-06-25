using System;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private int _maxAmmo;

    public event Action <int> Updated;

    public Weapon WeaponPrefab => _weaponPrefab;
    public int CurrentAmmo { get; private set; }
    public bool HasAmmo { get; private set; } = true;

    private void Start()
    {
        CurrentAmmo = _maxAmmo;
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

    public void IncreaseProjectilesCount(int count)
    {
        if (CurrentAmmo + count < _maxAmmo)
        {
            CurrentAmmo += count;
        }
        else
        {
            CurrentAmmo = _maxAmmo;
        }

        if (CurrentAmmo > 0)
        {
            HasAmmo = true;
        }

        Updated?.Invoke(CurrentAmmo);
    }
}
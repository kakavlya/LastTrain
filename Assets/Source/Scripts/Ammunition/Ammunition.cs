using System;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private int _maxAmmo;

    public event Action <int> Updated;
    public event Action<int> AmmoAdded;

    public Weapon WeaponPrefab => _weaponPrefab;
    public bool HasAmmo { get; private set; } = true;
    public int CurrentAmmo { get; private set; }

    private void Awake()
    {
        CurrentAmmo = _maxAmmo;
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
        Debug.Log(CurrentAmmo);
    }

    public void IncreaseProjectilesCount(int addedCount)
    {
        if (CurrentAmmo + addedCount < _maxAmmo)
        {
            CurrentAmmo += addedCount;
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
        AmmoAdded?.Invoke(addedCount);
    }
}
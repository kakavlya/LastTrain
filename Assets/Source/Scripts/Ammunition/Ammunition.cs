using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private int _maxAmmo;

    private int _currentAmmo;

    public Weapon WeaponPrefab => _weaponPrefab;
    public int CurrentAmmo => _currentAmmo;
    public bool HasAmmo { get; private set; } = true;

    private void Start()
    {
        _currentAmmo = _maxAmmo;
    }

    public void DecreaseProjectilesCount()
    {
        if (_currentAmmo > 0)
        {
            _currentAmmo--;
        }
        else
        {
            HasAmmo = false;
        }

        Debug.Log("Projectile count " + _currentAmmo);
    }

    public void IncreaseProjectilesCount(int count)
    {
        if (_currentAmmo + count < _maxAmmo)
        {
            _currentAmmo += count;
        }
        else
        {
            _currentAmmo = _maxAmmo;
        }

        if (_currentAmmo > 0)
        {
            HasAmmo = true;
        }
    }
}
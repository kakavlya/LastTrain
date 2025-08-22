using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Shotgun Settings")]
    [SerializeField] private int _bulletsInShot = 5;
    [SerializeField] private float _spreadAngle = 30;

    protected override void OnWeaponFire()
    {
        for (int i = 0; i < _bulletsInShot; i++)
        {
            var proj = UsePooling
                ? ProjectilePool.Instance.Spawn(ProjectilePrefab, FirePoint.position,
                Quaternion.LookRotation(GetRandomSpread()), Owner, ProjectileSpeed, Damage, Range)
                : Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));
        }
    }

    private Vector3 GetRandomSpread()
    {
        float horizontalSpread = Random.Range(-_spreadAngle / 2, _spreadAngle / 2);
        Quaternion spreadRotation = Quaternion.Euler(0, horizontalSpread, 0);
        return spreadRotation * Direction;
    }
}

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
                ? ProjectileTypesPool.Instance.Spawn(ProjectilePrefab, FirePoint.position,
        Quaternion.LookRotation(GetRandomSpread()), owner: gameObject, Speed, Damage, MaxAttackDistance)
                : Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));
        }
    }

    private Vector3 GetRandomSpread()
    {
        float baseAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
        float randomAngleDegree = baseAngle + Random.Range(-_spreadAngle / 2, _spreadAngle / 2);
        Vector3 direction = Quaternion.Euler(0, randomAngleDegree, 0) * Vector3.forward;
        return direction;
    }
}

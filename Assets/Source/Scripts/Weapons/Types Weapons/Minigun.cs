using UnityEngine;

public class Minigun : Weapon
{
    protected override void OnWeaponFire()
    {
        var proj = UsePooling
        ? ProjectileTypesPool.Instance.Spawn(ProjectilePrefab, FirePoint.position,
        Quaternion.LookRotation(Direction), owner: gameObject, Speed, Damage, MaxAttackDistance)
        : Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));
    }
}

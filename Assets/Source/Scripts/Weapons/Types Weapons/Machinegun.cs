using UnityEngine;

public class Machinegun : Weapon
{
    protected override void OnWeaponFire()
    {
        var proj = UsePooling
        ? ProjectilePool.Instance.Spawn(ProjectilePrefab, FirePoint.position,
        Quaternion.LookRotation(Direction), owner: gameObject, Speed, Damage, MaxAttackDistance)
        : Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));
    }
}

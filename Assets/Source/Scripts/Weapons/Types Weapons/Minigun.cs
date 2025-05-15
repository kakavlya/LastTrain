using UnityEngine;

public class Minigun : Weapon
{
    protected override void OnWeaponFire()
    {
        var proj = UsePooling
        ? ProjectileTypesPool.Instance.Spawn(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction), owner: gameObject)
        : Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));


        proj.Configure(
            owner: Owner,
            usePooling: UsePooling
        );
    }
}

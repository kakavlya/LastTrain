using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Weapon
{
    [Header("Explode Settings")]
    [SerializeField] private int _aoeDamage;
    [SerializeField] private float _aoeRange;

    protected override void OnWeaponFire()
    {
        Quaternion rotation = Quaternion.LookRotation(Direction, Vector3.forward);

        var proj = UsePooling
        ? ProjectilePool.Instance.Spawn(
            ProjectilePrefab,
            FirePoint.position,
            rotation,
            Owner,
            ProjectileSpeed, Damage,
            MaxAttackDistance,
            _aoeDamage,
            _aoeRange
            )
        : Instantiate(ProjectilePrefab, FirePoint.position, rotation);

        proj.SetVelocity();
    }
}

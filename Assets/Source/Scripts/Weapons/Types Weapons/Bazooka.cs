using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Weapon
{
    [Header("Explode Settings")]
    [SerializeField] private float _aoeDamage;
    [SerializeField] private float _aoeRange;

    private float _currentAoeDamage;

    public override void Init(float damage, float range, float? fireDelay, float? fireAngle, float? aoeDamage)
    {
        base.Init(damage, range, fireDelay, fireAngle, aoeDamage);

        _currentAoeDamage = aoeDamage ?? _aoeDamage;
    }

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
            Range,
            _currentAoeDamage,
            _aoeRange
            )
        : Instantiate(ProjectilePrefab, FirePoint.position, rotation);

        proj.SetVelocity();
    }
}

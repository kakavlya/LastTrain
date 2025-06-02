using System;
using Assets.Source.Scripts.Weapons;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AimingTargetProvider _aimingTarget;
    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected Projectile ProjectilePrefab;


    [Header("Shoot Settings")]
    [SerializeField] protected bool UsePooling = true;
    [SerializeField] protected float FireDelay = 0.1f;
    [SerializeField] protected float Speed = 100;
    [SerializeField] protected int Damage = 50;
    [SerializeField] protected float MaxAttackDistance = 100;

    //[field: SerializeField] public bool IsAutomatic;  

    private float _lastFireTime;

    protected Vector3 Direction;
    protected GameObject Owner;

    public event Action OnFired;
    public event Action OnStopFired;

    private void OnEnable()
    {
        Owner = GetComponent<GameObject>();
    }

    private bool FirePossibleCalculate()
    {
        if (Time.time - _lastFireTime < FireDelay)
            return false;

        if (_aimingTarget == null || !_aimingTarget.AimPointWorld.HasValue)
            return false;

        _lastFireTime = Time.time;
        return true;
    }

    private void CalculateDirection()
    {
        Vector3 target = _aimingTarget.AimPointWorld.Value;
        Vector3 origin = FirePoint.position;
        Vector3 direction = (target - origin).normalized;
        Direction = direction;
    }

    protected virtual void OnWeaponFire()
    {
        var proj = UsePooling
            ? ProjectileTypesPool.Instance.Spawn(ProjectilePrefab, FirePoint.position,
            Quaternion.LookRotation(Direction), owner: gameObject, Speed, Damage, MaxAttackDistance)
:           Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));
    }

    public void Fire()
    {
        if (FirePossibleCalculate() == false)
            return;

        CalculateDirection();

        OnWeaponFire();

        OnFired?.Invoke();
    }

    public virtual void StopFire()
    {
        OnStopFired?.Invoke();
    }
}

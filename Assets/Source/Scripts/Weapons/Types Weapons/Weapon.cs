using System;
using Assets.Source.Scripts.Weapons;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _owner;
    [SerializeField] private AimingTargetProvider _aimingTarget;
    [SerializeField] private ParticleSystem _muzzleEffectPrefab;
    [SerializeField] private Ammunition _ammunition;
    [SerializeField] private GameObject _owner;
    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected Projectile ProjectilePrefab;

    [Header("Shoot Settings")]
    [SerializeField] protected float FireDelay = 0.1f;
    [SerializeField] protected bool UsePooling = true;
    [SerializeField] protected float ProjectileSpeed = 100;
    [SerializeField] protected int Damage = 50;
    [SerializeField] protected float MaxAttackDistance = 100;

    private float _lastFireTime;

    protected Vector3 Direction;

    public event Action OnFired;
    public event Action OnStopFired;

    private void OnEnable()
    {
        //Owner = GetComponent<GameObject>();
        Owner = _owner != null ? _owner : gameObject;
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
            ? ProjectilePool.Instance.Spawn(ProjectilePrefab, FirePoint.position,
            Quaternion.LookRotation(Direction), _owner, ProjectileSpeed, Damage, MaxAttackDistance)
            : Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));
            Quaternion.LookRotation(Direction), owner: Owner, Speed, Damage, MaxAttackDistance)
:           Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));
    }

    public void Fire()
    {
        if (FirePossibleCalculate() == true)
        {
            if (_ammunition.HasProjectiles) 
            {
                CalculateDirection();
                OnFired?.Invoke();
                _ammunition.DecreaseProjectilesCount();
                OnWeaponFire();

                if (_muzzleEffectPrefab != null)
                    ParticlePool.Instance.Spawn(_muzzleEffectPrefab, FirePoint.transform.position);
            }
            else
            {
                StopFire();
            }
        }
    }

    public virtual void StopFire()
    {
        OnStopFired?.Invoke();
    }

    public virtual bool GetIsLoopedFireSound() => false;
}

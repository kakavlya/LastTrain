using System;
using Assets.Source.Scripts.Weapons;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AimingTargetProvider _aimingTarget;
    [SerializeField] private ParticleSystem _muzzleEffectPrefab;
    [SerializeField] private Ammunition _ammunition = null;
    [SerializeField] private Sprite _uiSprite;

    [SerializeField] protected GameObject Owner;
    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected Projectile ProjectilePrefab;
    [SerializeField] protected WeaponInput _weaponInput;

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

    public Sprite UISprite => _uiSprite;

    private void OnEnable()
    {
        _weaponInput.Fired += Fire;
        _weaponInput.StopFired += StopFire;
        Owner = Owner != null ? Owner : gameObject;
    }

    private void OnDisable()
    {
        _weaponInput.Fired -= Fire;
        _weaponInput.StopFired -= StopFire;
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
            Quaternion.LookRotation(Direction), Owner, ProjectileSpeed, Damage, MaxAttackDistance)
: Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));
    }

    private void Fire()
    {
        if (FirePossibleCalculate() == true)
        {
            if (_ammunition == null || _ammunition.HasAmmo)
            {
                CalculateDirection();
                OnFired?.Invoke();
                OnWeaponFire();

                if (_muzzleEffectPrefab != null)
                    ParticlePool.Instance.Spawn(_muzzleEffectPrefab, FirePoint.transform.position);

                if (_ammunition != null)
                    _ammunition.DecreaseProjectilesCount();
            }
            else
            {
                StopFire();
            }
        }
    }

    protected virtual void StopFire()
    {
        OnStopFired?.Invoke();
    }

    public virtual bool GetIsLoopedFireSound() => false;
}

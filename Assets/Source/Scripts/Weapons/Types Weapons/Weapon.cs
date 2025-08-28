using System;
using UnityEngine;
using LastTrain.Ammunition;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem _muzzleEffectPrefab;
    [SerializeField] private Sprite _uiSpriteActive;
    [SerializeField] private Sprite _uiSpriteDeactive;

    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected Projectile ProjectilePrefab;

    [Header("Shoot Settings")]
    [SerializeField] protected float FireDelay = 0.1f;
    [SerializeField] protected bool UsePooling = true;
    [SerializeField] protected float ProjectileSpeed = 100;

    private float _lastFireTime;
    private float _currentFireDelay;

    protected GameObject Owner;
    protected float Damage;
    protected float Range;

    public event Action OnFired;
    public event Action OnStopFired;

    protected Vector3 Direction => FirePoint.forward;

    public Weapon PrefabReference { get; private set; }
    public Sprite UISpriteActive => _uiSpriteActive;
    public Sprite UISpriteDeactive => _uiSpriteDeactive;

    public virtual void Init(float damage, float range, float? fireDelay, float? fireAngle, float? aoeDamage)
    {
        Owner = gameObject;
        Damage = damage;
        Range = range;

        _currentFireDelay = fireDelay ?? FireDelay;
    }

    public virtual bool GetIsLoopedFireSound() => false;

    public virtual void StopFire()
    {
        OnStopFired?.Invoke();
    }

    private bool FirePossibleCalculate()
    {
        if (Time.time - _lastFireTime < _currentFireDelay)
            return false;

        _lastFireTime = Time.time;
        return true;
    }

    protected virtual void OnWeaponFire()
    {
        var proj = UsePooling
            ? ProjectilePool.Instance.Spawn(
                ProjectilePrefab,
                FirePoint.position,
                Quaternion.LookRotation(Direction),
                Owner,
                ProjectileSpeed,
                Damage,
                Range)
                    : Instantiate(
                        ProjectilePrefab,
                        FirePoint.position,
                        Quaternion.LookRotation(Direction));
    }

    public void Fire(Ammunition ammo)
    {
        if (FirePossibleCalculate() == true)
        {
            if (ammo != null && !ammo.HasAmmo)
            {
                StopFire();
                return;
            }

            OnFired?.Invoke();
            OnWeaponFire();

            if (_muzzleEffectPrefab != null)
                ParticlePool.Instance.Spawn(_muzzleEffectPrefab, FirePoint.transform.position);

            ammo?.DecreaseProjectilesCount();
        }
    }

    public void Fire()
    {
        if (FirePossibleCalculate() == true)
        {
            OnFired?.Invoke();
            OnWeaponFire();

            if (_muzzleEffectPrefab != null)
                ParticlePool.Instance.Spawn(_muzzleEffectPrefab, FirePoint.transform.position);
        }
    }

    public void SetPrefabReference(Weapon prefab)
    {
        PrefabReference = prefab;
    }
}

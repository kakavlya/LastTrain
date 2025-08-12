using System;
using Assets.Source.Scripts.Weapons;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem _muzzleEffectPrefab;
    //[SerializeField] private Ammunition _ammunition = null;
    [SerializeField] private Sprite _uiSpriteActive;
    [SerializeField] private Sprite _uiSpriteDeactive;

    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected Projectile ProjectilePrefab;

    [Header("Shoot Settings")]
    [SerializeField] protected float FireDelay = 0.1f;
    [SerializeField] protected bool UsePooling = true;
    [SerializeField] protected float ProjectileSpeed = 100;
    [SerializeField] protected int Damage = 50;
    [SerializeField] protected float MaxAttackDistance = 100;

    protected GameObject Owner;
    private float _lastFireTime;

    protected Vector3 Direction => FirePoint.forward;

    public event Action OnFired;
    public event Action OnStopFired;

    public Weapon PrefabReference { get; private set; }
    public Sprite UISpriteActive => _uiSpriteActive;
    public Sprite UISpriteDeactive => _uiSpriteDeactive;
    //public Ammunition Ammunition => _ammunition;

    private void OnEnable()
    {
        Owner = gameObject;
    }

    public virtual bool GetIsLoopedFireSound() => false;

    public virtual void StopFire()
    {
        OnStopFired?.Invoke();
    }

    private bool FirePossibleCalculate()
    {
        if (Time.time - _lastFireTime < FireDelay)
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
                MaxAttackDistance)
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

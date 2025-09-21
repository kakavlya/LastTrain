using System;
using UnityEngine;
using LastTrain.AmmunitionSystem;
using LastTrain.Particles;
using LastTrain.Projectiles;
using LastTrain.Weapons.System;
using LastTrain.Weapons.Types;
public class FireTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite _uiSpriteActive;
    [SerializeField] private Sprite _uiSpriteDeactive;
    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected Projectile ProjectilePrefab;
    [SerializeField] protected ParticleSystem _muzzleEffectPrefab;
    [SerializeField] private AimingTargetProvider _aim;
    [SerializeField] private LayerMask _obstacleMask = ~0;
    [Header("Shoot Settings")][SerializeField] protected float FireDelay = 0.1f;
    [SerializeField] protected bool UsePooling = true;
    [SerializeField] protected float ProjectileSpeed = 100;
    [Header("Aiming Mode")][Tooltip("Если true — летим по плоскости (Y игнорируется). Если false — полностью 3D.")][SerializeField] private bool _shootPlanar = false;
    private float _lastFireTime; private float _currentFireDelay; protected GameObject Owner; protected float Damage; protected float Range; 
    public event Action OnFired;
    public event Action OnStopFired; protected Vector3 Direction => FirePoint.forward;
    public Weapon PrefabReference { get; private set; }
    public Transform FirepointPosition => FirePoint;
    public Sprite UISpriteActive => _uiSpriteActive;
    public Sprite UISpriteDeactive => _uiSpriteDeactive;
    public virtual void Init(float damage, float range, float? fireDelay, float? fireAngle, float? aoeDamage)
    { Owner = gameObject; Damage = damage; Range = range; _currentFireDelay = fireDelay ?? FireDelay; }
    public virtual void Fire(Ammunition ammo = null)
    {
        var ad = _aim.GetAim(); Vector3 target = ad.worldPoint; Vector3 dir = (target - FirePoint.position).normalized; float distToTarget = Vector3.Distance(FirePoint.position, target);
        if (Physics.Raycast(FirePoint.position, dir, out var block, distToTarget, _obstacleMask)) { target = block.point; dir = (target - FirePoint.position).normalized; }
        if (!FirePossibleCalculate()) return; if (ammo != null && !ammo.HasAmmo) { InvokeStopFire(); return; }
        OnFired?.Invoke(); Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
        var proj = UsePooling ? ProjectilePool.Instance.Spawn(ProjectilePrefab, FirePoint.position, rot, Owner, ProjectileSpeed, Damage, Range) : Instantiate(ProjectilePrefab, FirePoint.position, rot);
        if (_muzzleEffectPrefab != null) ParticlePool.Instance.Spawn(_muzzleEffectPrefab, FirePoint.transform.position);
        ammo?.DecreaseProjectilesCount(); Debug.DrawLine(_aim.GetAim().camRay.origin, _aim.GetAim().worldPoint, Color.cyan);
        Debug.DrawLine(FirePoint.position, target, Color.yellow); Debug.DrawRay(FirePoint.position, dir * 5f, Color.green);
    }
    public void SetPrefabReference(Weapon prefab) { PrefabReference = prefab; }
    public virtual bool GetIsLoopedFireSound() => false; public virtual void InvokeStopFire() { OnStopFired?.Invoke(); }
    protected void InvokeFire() { OnFired?.Invoke(); }
    protected virtual void OnWeaponFire()
    {
        var proj = UsePooling ? ProjectilePool.Instance.Spawn(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction), Owner, ProjectileSpeed, Damage, Range)
            : Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));
    }
    protected bool FirePossibleCalculate() { if (Time.time - _lastFireTime < _currentFireDelay) return false; _lastFireTime = Time.time; return true; }
}
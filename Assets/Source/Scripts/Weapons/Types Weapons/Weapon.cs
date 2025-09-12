using System;
using UnityEngine;
using LastTrain.AmmunitionSystem;
using LastTrain.Particles;
using LastTrain.Projectiles;
using LastTrain.Weapons.System;

namespace LastTrain.Weapons.Types
{
    public class Weapon : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Sprite _uiSpriteActive;
        [SerializeField] private Sprite _uiSpriteDeactive;
        [SerializeField] protected Transform FirePoint;
        [SerializeField] protected Projectile ProjectilePrefab;
        [SerializeField] protected ParticleSystem _muzzleEffectPrefab;
        [SerializeField] private AimingTargetProvider _aim;
        [SerializeField] private LayerMask _obstacleMask = ~0;

        [Header("Shoot Settings")]
        [SerializeField] protected float FireDelay = 0.1f;
        [SerializeField] protected bool UsePooling = true;
        [SerializeField] protected float ProjectileSpeed = 100;
        [SerializeField] protected float Range = 2000f;

        private float _lastFireTime;
        private float _currentFireDelay;

        protected GameObject Owner;
        protected float Damage;
        protected AimingTargetProvider Aim => _aim;
        protected LayerMask ObstacleMask => _obstacleMask;

        public event Action OnFired;
        public event Action OnStopFired;

        public Weapon PrefabReference { get; private set; }
        public Transform FirepointPosition => FirePoint;
        public Sprite UISpriteActive => _uiSpriteActive;
        public Sprite UISpriteDeactive => _uiSpriteDeactive;

        public virtual void Init(float damage, float range, float? fireDelay, float? fireAngle, float? aoeDamage)
        {
            Owner = gameObject;
            Damage = damage;
            if (range > 0) Range = range;
            _currentFireDelay = fireDelay ?? FireDelay;
        }

        public void SetAimProvider(AimingTargetProvider provider) => _aim = provider;

        public virtual void Fire(Ammunition ammo = null)
        {
            if (ammo != null && !ammo.HasAmmo)
            {
                InvokeStopFire();
                return;
            }

            if (!FirePossibleCalculate()) return;

            if (_aim == null || FirePoint == null)
            {
                return;
            }

            var ad = _aim.GetAim();
            Vector3 origin = FirePoint.position;
            Vector3 target = ad.worldPoint;
            Vector3 dir = target - origin;

            if (dir.sqrMagnitude < 1e-6f) dir = FirePoint.forward;
            else dir.Normalize();

            float distToTarget = Vector3.Distance(origin, target);
            float maxRay = (Range > 0f) ? Mathf.Min(distToTarget, Range) : distToTarget;
            Vector3 originNoSelf = origin + dir * 0.02f;

            if (Physics.Raycast(originNoSelf, dir, out var block, maxRay, _obstacleMask, QueryTriggerInteraction.Ignore))
            {
                target = block.point;
                dir = (target - origin).normalized;
            }

            OnFired?.Invoke();
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            var proj = UsePooling
                ? ProjectilePool.Instance.Spawn(ProjectilePrefab, origin, rot, Owner, ProjectileSpeed, Damage, Range)
                : Instantiate(ProjectilePrefab, origin, rot);

            if (_muzzleEffectPrefab != null)
                ParticlePool.Instance.Spawn(_muzzleEffectPrefab, FirePoint.position);

            ammo?.DecreaseProjectilesCount();
        }

        public void SetPrefabReference(Weapon prefab)
        {
            PrefabReference = prefab;
        }

        public virtual bool GetIsLoopedFireSound() => false;

        public virtual void InvokeStopFire() => OnStopFired?.Invoke();

        protected void InvokeFire()
        {
            OnFired?.Invoke();
        }

        protected virtual void OnWeaponFire()
        {

        }

        protected bool FirePossibleCalculate()
        {
            var fireTimeDifference = Time.time - _lastFireTime;

            if (fireTimeDifference < _currentFireDelay)
            {
                return false;
            }

            _lastFireTime = Time.time;
            return true;
        }
    }
}


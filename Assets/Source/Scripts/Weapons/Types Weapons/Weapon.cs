using LastTrain.AmmunitionSystem;
using LastTrain.Particles;
using LastTrain.Projectiles;
using LastTrain.Projectiles.Types;
using LastTrain.Weapons.System;
using System;
using UnityEngine;

namespace LastTrain.Weapons.Types
{
    public class Weapon : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Sprite _uiSpriteActive;
        [SerializeField] private Sprite _uiSpriteDeactive;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private ParticleSystem _muzzleEffectPrefab;
        [SerializeField] private AimingTargetProvider _aim;
        [SerializeField] private LayerMask _obstacleMask = ~0;

        [Header("Shoot Settings")]
        [SerializeField] private float _fireDelay = 0.1f;
        [SerializeField] private bool _usePooling = true;
        [SerializeField] private float _projectileSpeed = 100;
        [SerializeField] private float _range = 2000f;

        private float _lastFireTime;
        private float _currentFireDelay;
        private float _minDirectionSqrMagnitude = 1e-6f;
        private float _selfCollisionOffset = 0.02f;
        private GameObject _owner;
        private float _damage;

        public event Action Fired;

        public event Action StopFired;

        protected float SelfCollisionOffset => _selfCollisionOffset;

        protected float MinDirectionSqrMagnitude => _minDirectionSqrMagnitude;

        protected AimingTargetProvider Aim => _aim;

        protected LayerMask ObstacleMask => _obstacleMask;

        protected GameObject Owner => _owner;

        protected float Damage => _damage;

        public Transform FirePoint => _firePoint;

        public ParticleSystem MuzzleEffectPrefab => _muzzleEffectPrefab;

        public Projectile ProjectilePrefab => _projectilePrefab;

        public float FireDelay => _fireDelay;

        public bool UsePooling => _usePooling;

        public float ProjectileSpeed => _projectileSpeed;

        public float Range => _range;

        public Sprite UISpriteActive => _uiSpriteActive;

        public Sprite UISpriteDeactive => _uiSpriteDeactive;

        public float MaxRange => _range;

        public Weapon PrefabReference { get; private set; }

        public virtual void Init(float damage, float range, float? fireDelay, float? fireAngle, float? aoeDamage)
        {
            _owner = gameObject;
            _damage = damage;

            if (range > 0)
                _range = range;

            _currentFireDelay = fireDelay ?? _fireDelay;
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

            if (_aim == null || _firePoint == null)
            {
                return;
            }

            var ad = _aim.GetAim();
            Vector3 origin = _firePoint.position;
            Vector3 target = ad.WorldPoint;
            Vector3 dir = target - origin;

            if (dir.sqrMagnitude < _minDirectionSqrMagnitude)
            {
                dir = _firePoint.forward;
            }
            else
            {
                dir.Normalize();
            }

            float distToTarget = Vector3.Distance(origin, target);
            float maxRay = (_range > 0f) ? Mathf.Min(distToTarget, _range) : distToTarget;
            Vector3 originNoSelf = origin + (dir * _selfCollisionOffset);

            if (Physics.Raycast(originNoSelf, dir, out var block, maxRay, _obstacleMask, QueryTriggerInteraction.Ignore))
            {
                target = block.point;
                dir = (target - origin).normalized;
            }

            Fired?.Invoke();
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            var proj = _usePooling
                ? ProjectilePool.Instance.Spawn(_projectilePrefab, origin, rot, Owner, _projectileSpeed, Damage, Range)
                : Instantiate(_projectilePrefab, origin, rot);

            if (_muzzleEffectPrefab != null)
                ParticlePool.Instance.Spawn(_muzzleEffectPrefab, _firePoint.position);

            ammo?.DecreaseProjectilesCount();
        }

        public void SetPrefabReference(Weapon prefab)
        {
            PrefabReference = prefab;
        }

        public virtual bool GetIsLoopedFireSound() => false;

        public virtual void InvokeStopFire() => StopFired?.Invoke();

        protected void InvokeFire()
        {
            Fired?.Invoke();
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
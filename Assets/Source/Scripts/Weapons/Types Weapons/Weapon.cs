using System;
using UnityEngine;
using LastTrain.AmmunitionSystem;
using LastTrain.Particles;
using LastTrain.Projectiles;

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

        public virtual void Fire(Ammunition ammo = null, Vector3? targetWorldPos = null)
        {
            if (!FirePossibleCalculate())
                return;

            if (ammo != null && !ammo.HasAmmo)
            {
                InvokeStopFire();
                return;
            }

            OnFired?.Invoke();

            if (targetWorldPos.HasValue)
            {
                Vector3 dir = targetWorldPos.Value - FirePoint.position;
                dir.y = 0;
                dir = dir.normalized;

                var proj = UsePooling
                    ? ProjectilePool.Instance.Spawn(
                        ProjectilePrefab,
                        FirePoint.position,
                        Quaternion.LookRotation(dir),
                        Owner,
                        ProjectileSpeed,
                        Damage,
                        Range)
                    : Instantiate(
                        ProjectilePrefab,
                        FirePoint.position,
                        Quaternion.LookRotation(dir));
            }
            else
            {
                OnWeaponFire();
            }

            if (_muzzleEffectPrefab != null)
                ParticlePool.Instance.Spawn(_muzzleEffectPrefab, FirePoint.transform.position);

            ammo?.DecreaseProjectilesCount();
        }

        public void SetPrefabReference(Weapon prefab)
        {
            PrefabReference = prefab;
        }

        public virtual bool GetIsLoopedFireSound() => false;

        public virtual void InvokeStopFire()
        {
            OnStopFired?.Invoke();
        }

        protected void InvokeFire()
        {
            OnFired?.Invoke();
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

        protected bool FirePossibleCalculate()
        {
            if (Time.time - _lastFireTime < _currentFireDelay)
                return false;

            _lastFireTime = Time.time;
            return true;
        }
    }
}

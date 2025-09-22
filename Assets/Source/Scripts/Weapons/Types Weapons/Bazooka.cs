using UnityEngine;
using LastTrain.Projectiles;
using LastTrain.AmmunitionSystem;
using LastTrain.Particles;

namespace LastTrain.Weapons.Types
{
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

        public override void Fire(Ammunition ammo = null)
        {
            if (!FirePossibleCalculate())
                return;

            if (ammo != null && !ammo.HasAmmo) 
            { 
                InvokeStopFire();
                return;
            }

            if (Aim == null || FirePoint == null)
                return;

            var ad = Aim.GetAim();
            Vector3 origin = FirePoint.position;
            Vector3 target = ad.WorldPoint;
            Vector3 dir = target - origin;

            if (dir.sqrMagnitude < 1e-6f)
            {
                dir = FirePoint.forward;
            }
            else
            {
                dir.Normalize();
            }

            float distToTarget = Vector3.Distance(origin, target);
            float maxRay = (Range > 0f) ? Mathf.Min(distToTarget, Range) : distToTarget;
            Vector3 originNoSelf = origin + dir * 0.02f;

            if (Physics.Raycast(
                originNoSelf, dir, out var block, maxRay, ObstacleMask, QueryTriggerInteraction.Ignore))
            {
                target = block.point;
                dir = (target - origin).normalized;
            }

            InvokeFire();
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

            var proj = UsePooling
                ? ProjectilePool.Instance.Spawn(
                      ProjectilePrefab,
                      origin,
                      rot,
                      Owner,
                      ProjectileSpeed,
                      Damage,
                      Range,
                      _currentAoeDamage,
                      _aoeRange)
                : Instantiate(ProjectilePrefab, origin, rot);

            if (MuzzleEffectPrefab != null)
                ParticlePool.Instance.Spawn(MuzzleEffectPrefab, origin);

            ammo?.DecreaseProjectilesCount();
        }
    }
}

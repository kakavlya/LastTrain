using LastTrain.AmmunitionSystem;
using LastTrain.Particles;
using LastTrain.Projectiles;
using UnityEngine;

namespace LastTrain.Weapons.Types
{
    public class Shotgun : Weapon
    {
        [Header("Shotgun Settings")]
        [SerializeField] private int _bulletsInShot = 5;
        [SerializeField] private float _spreadAngle = 30;

        private float _currentSpreadAngle;
        private float _angleDivider = 0.5f;

        public override void Init(float damage, float range, float? fireDelay, float? fireAngle, float? aoeDamage)
        {
            base.Init(damage, range, fireDelay, fireAngle, aoeDamage);
            _currentSpreadAngle = fireAngle ?? _spreadAngle;
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

            if (Aim == null || FirePoint == null || ProjectilePrefab == null)
                return;

            var ad = Aim.GetAim();
            Vector3 origin = FirePoint.position;
            Vector3 target = ad.WorldPoint;
            Vector3 centerDir = target - origin;

            if (centerDir.sqrMagnitude < MinDirectionSqrMagnitude)
            {
                centerDir = FirePoint.forward;
            }
            else
            {
                centerDir.Normalize();
            }

            float distToTarget = Vector3.Distance(origin, target);
            float maxRay = (Range > 0f) ? Mathf.Min(distToTarget, Range) : distToTarget;
            Vector3 originNoSelf = origin + (centerDir * SelfCollisionOffset);

            if (Physics.Raycast(
                originNoSelf, centerDir, out var block, maxRay, ObstacleMask, QueryTriggerInteraction.Ignore))
            {
                target = block.point;
                centerDir = (target - origin).normalized;
            }

            InvokeFire();

            for (int i = 0; i < _bulletsInShot; i++)
            {
                Vector3 dir = SampleYawOnly(centerDir, _currentSpreadAngle * _angleDivider);
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

            if (MuzzleEffectPrefab != null)
                ParticlePool.Instance.Spawn(MuzzleEffectPrefab, FirePoint.position);

            ammo?.DecreaseProjectilesCount();
        }

        private Vector3 SampleYawOnly(Vector3 centerDir, float halfAngleDeg)
        {
            Vector3 baseDir = centerDir;

            if (baseDir.sqrMagnitude < 1e-6f)
                baseDir = FirePoint.forward;

            baseDir.Normalize();
            float randomYaw = Random.Range(-halfAngleDeg, halfAngleDeg);
            float randomPitch = Random.Range(-halfAngleDeg, halfAngleDeg);
            Quaternion yawRotation = Quaternion.AngleAxis(randomYaw, Vector3.up);
            Quaternion pitchRotation = Quaternion.AngleAxis(randomPitch, Vector3.right);
            return pitchRotation * yawRotation * baseDir;
        }
    }
}

using UnityEngine;
using LastTrain.Projectiles;
using LastTrain.AmmunitionSystem;
using LastTrain.Particles;

namespace LastTrain.Weapons.Types
{
    public class Shotgun : Weapon
    {
        [Header("Shotgun Settings")]
        [SerializeField] private int _bulletsInShot = 5;
        [SerializeField] private float _spreadAngle = 30;

        private float _currentSpreadAngle;

        public override void Init(float damage, float range, float? fireDelay, float? fireAngle, float? aoeDamage)
        {
            base.Init(damage, range, fireDelay, fireAngle, aoeDamage);
            _currentSpreadAngle = fireAngle ?? _spreadAngle;
        }

        public override void Fire(Ammunition ammo = null)
        {
            if (!FirePossibleCalculate()) return;
            if (ammo != null && !ammo.HasAmmo) { InvokeStopFire(); return; }
            if (Aim == null || FirePoint == null || ProjectilePrefab == null) return;

            var ad = Aim.GetAim();
            Vector3 origin = FirePoint.position;
            Vector3 target = ad.WorldPoint;

            Vector3 centerDir = target - origin;
            if (centerDir.sqrMagnitude < 1e-6f) centerDir = FirePoint.forward;
                else centerDir.Normalize();

            float distToTarget = Vector3.Distance(origin, target);
            float maxRay = (Range > 0f) ? Mathf.Min(distToTarget, Range) : distToTarget;
            Vector3 originNoSelf = origin + centerDir * 0.02f;

            if (Physics.Raycast(originNoSelf, centerDir, out var block, maxRay, ObstacleMask, QueryTriggerInteraction.Ignore))
            {
                target = block.point;
                centerDir = (target - origin).normalized;
            }

            InvokeFire();

            for (int i = 0; i < _bulletsInShot; i++)
            {
                Vector3 dir = SampleYawOnly(centerDir, _currentSpreadAngle * 0.5f);
                dir.y = 0;
                dir = dir.normalized;
                //Vector3 spreadDir = GetRandomSpread();

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

            if (_muzzleEffectPrefab != null)
                ParticlePool.Instance.Spawn(_muzzleEffectPrefab, FirePoint.position);

            ammo?.DecreaseProjectilesCount();

            Debug.DrawLine(ad.CamRay.origin, ad.WorldPoint, Color.cyan); 
            Debug.DrawLine(origin, origin + centerDir * 5f, Color.yellow);
        }

        //protected override void OnWeaponFire()
        //{
        //    for (int i = 0; i < _bulletsInShot; i++)
        //    {
        //        var proj = UsePooling
        //            ? ProjectilePool.Instance.Spawn(ProjectilePrefab, FirePoint.position,
        //            Quaternion.LookRotation(GetRandomSpread()), Owner, ProjectileSpeed, Damage, Range)
        //            : Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));
        //    }
        //}

        private Vector3 SampleYawOnly(Vector3 centerDir, float halfAngleDeg)
        {
            // спроецировать центральное направление на горизонт
            Vector3 flat = Vector3.ProjectOnPlane(centerDir, Vector3.up);
            if (flat.sqrMagnitude < 1e-6f) flat = Vector3.ProjectOnPlane(FirePoint.forward, Vector3.up);
            flat.Normalize();

            float yaw = Random.Range(-halfAngleDeg, halfAngleDeg);
            Quaternion q = Quaternion.AngleAxis(yaw, Vector3.up);
            return (q * flat).normalized;
        }

        private Vector3 GetRandomSpread(Vector3 centerDir)
        {
            float horizontalSpread = Random.Range(-_currentSpreadAngle / 2, _currentSpreadAngle / 2);
            Quaternion spreadRotation = Quaternion.Euler(0, horizontalSpread, 0);

            return spreadRotation * centerDir;
        }
    }
}

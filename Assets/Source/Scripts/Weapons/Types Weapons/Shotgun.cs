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

        public override void Fire(Ammunition ammo = null, Vector3? targetWorldPos = null)
        {
            if (!FirePossibleCalculate())
                return;

            if (ammo != null && !ammo.HasAmmo)
            {
                InvokeStopFire();
                return;
            }

            InvokeFire();

            if (targetWorldPos.HasValue)
            {
                for (int i = 0; i < _bulletsInShot; i++)
                {
                    Vector3 dir = targetWorldPos.Value - FirePoint.position;
                    dir.y = 0;
                    dir = dir.normalized;
                    Vector3 spreadDir = GetRandomSpread();

                    var proj = UsePooling
                        ? ProjectilePool.Instance.Spawn(
                            ProjectilePrefab,
                            FirePoint.position,
                            Quaternion.LookRotation(spreadDir),
                            Owner,
                            ProjectileSpeed,
                            Damage,
                            Range)
                        : Instantiate(
                            ProjectilePrefab,
                            FirePoint.position,
                            Quaternion.LookRotation(spreadDir));
                }
            }
            else
            {
                OnWeaponFire();
            }

            if (_muzzleEffectPrefab != null)
                ParticlePool.Instance.Spawn(_muzzleEffectPrefab, FirePoint.transform.position);

            ammo?.DecreaseProjectilesCount();
        }

        protected override void OnWeaponFire()
        {
            for (int i = 0; i < _bulletsInShot; i++)
            {
                var proj = UsePooling
                    ? ProjectilePool.Instance.Spawn(ProjectilePrefab, FirePoint.position,
                    Quaternion.LookRotation(GetRandomSpread()), Owner, ProjectileSpeed, Damage, Range)
                    : Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(Direction));
            }
        }

        private Vector3 GetRandomSpread()
        {
            float horizontalSpread = Random.Range(-_currentSpreadAngle / 2, _currentSpreadAngle / 2);
            Quaternion spreadRotation = Quaternion.Euler(0, horizontalSpread, 0);
            return spreadRotation * Direction;
        }
    }
}

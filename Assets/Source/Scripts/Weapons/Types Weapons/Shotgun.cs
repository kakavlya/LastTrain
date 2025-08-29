using UnityEngine;
using LastTrain.Projectiles;

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

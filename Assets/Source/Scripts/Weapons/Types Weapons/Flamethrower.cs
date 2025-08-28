using UnityEngine;
using LastTrain.Enemies;

namespace LastTrain.Weapons.Types
{
    public class Flamethrower : Weapon
    {
        [Header("Flamethrower Settings")]
        [SerializeField] private float _horisontalAngle = 30f;
        [SerializeField] private ParticleSystem _flameParticle;

        private float _verticalAngle = 60f;
        private float _minStartLifetime = 0.1f;
        private bool _isFiring;
        private Collider[] _hits = new Collider[30];
        private float _nextDamageTime;
        private float _currentHorisontalAngle;

        private void Update()
        {
            if (_isFiring)
            {
                if (!_flameParticle.isEmitting)
                {
                    _flameParticle.Play();
                }

                if (Time.time >= _nextDamageTime)
                {
                    ApplyDamageToTargets();
                    _nextDamageTime = Time.time + FireDelay;
                }
            }
        }

        public override bool GetIsLoopedFireSound() => true;

        public override void Init(float damage, float range, float? fireDelay, float? fireAngle, float? aoeDamage)
        {
            base.Init(damage, range, fireDelay, fireAngle, aoeDamage);
            _currentHorisontalAngle = fireAngle ?? _horisontalAngle;
            var mainSetting = _flameParticle.main;
            mainSetting.startSpeed = ProjectileSpeed;
            mainSetting.startLifetime = _minStartLifetime + (Range / ProjectileSpeed);
            var shapeSetting = _flameParticle.shape;
            Vector3 currentScale = shapeSetting.scale;
            shapeSetting.scale = new Vector3(_currentHorisontalAngle, currentScale.y, currentScale.z);
        }

        public override void StopFire()
        {
            base.StopFire();

            if (_isFiring)
            {
                _isFiring = false;
                _flameParticle.Stop();
            }
        }


        protected override void OnWeaponFire()
        {
            if (!_isFiring)
            {
                _isFiring = true;
                _nextDamageTime = Time.time;
            }
        }

        private void ApplyDamageToTargets()
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                Range,
                _hits
                );

            for (int i = 0; i < hitsCount; i++)
            {
                if (_hits[i].TryGetComponent(out EnemyHealth enemyHealth))
                {
                    Vector3 directionToTarget = (_hits[i].transform.position - transform.position).normalized;

                    if (CheckHorizontalAngle(directionToTarget) && CheckVerticalAngle(directionToTarget))
                    {
                        enemyHealth.TakeDamage(Damage);
                    }
                }
            }
        }

        private bool CheckHorizontalAngle(Vector3 directionToTarget)
        {
            Vector3 horizontalDirection = new Vector3(directionToTarget.x, 0, directionToTarget.z).normalized;
            Vector3 horizontalForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            float horizontalAngle = Vector3.Angle(horizontalForward, horizontalDirection);
            return horizontalAngle <= _currentHorisontalAngle / 2f;
        }

        private bool CheckVerticalAngle(Vector3 directionToTarget)
        {
            Vector3 verticalDirection = new Vector3(0, directionToTarget.y, 0).normalized;
            Vector3 verticalForward = new Vector3(0, transform.forward.y, 0).normalized;
            float verticalAngle = Vector3.Angle(verticalForward, verticalDirection);
            return verticalAngle <= _verticalAngle / 2f;
        }
    }
}
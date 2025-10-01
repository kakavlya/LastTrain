using LastTrain.Projectiles;
using LastTrain.Projectiles.Types;
using UnityEngine;

namespace LastTrain.Enemies
{
    public class EnemyShooterController : EnemyController
    {
        private const float _vectorMagnitudeTolerance = 1e-4f;
        private const float _minLengthThreshold = 0.01f;
        private const float _minRadiusThreshold = 0.1f;
        private const float _minDistanceThreshold = 0.2f;
        private const float _minBrainInterval = 0.05f;
        private const float _minFireInterval = 0.01f;
        private const float _hysteresisFactor = 0.5f;
        private const float _retreatSpeedMultiplier = 1.1f;
        private const float _strafeInterpolationFactor = 0.1f;
        private const float _strafeSpeedAverageFactor = 0.5f;
        private const float _fireIntervalRandomMin = 0.95f;
        private const float _fireIntervalRandomMax = 1.05f;
        private const float _projectileDistanceBuffer = 2f;
        private const float _projectileLifetimeFactor = 3f;
        private const float _minFireAngle = 1f;
        private const float _maxFireAngle = 179f;

        [SerializeField] private Transform _firePoint;

        private Transform _player;
        private Collider _playerCol;
        private EnemyMovement _movement;
        private float _approachSpeed;
        private float _attackSpeedFactorMin;
        private float _attackSpeedFactorMax;
        private float _keepMinSurf;
        private float _keepMaxSurf;
        private float _turnSpeed;
        private float _orbitSpeedDeg;
        private Vector2 _changeDirEvery;
        private float _checkRadiusSqr;
        private Projectile _projectilePrefab;
        private float _fireInterval;
        private float _projectileSpeed;
        private int _projectileDamage;
        private float _shootingDistance;
        private State _state;
        private int _orbitDir;
        private float _changeTimer;
        private float _brainInterval;
        private float _brainTimer;
        private float _fireTimer;
        private float _fireAngle;
        private Vector3 _currentTarget;
        private float _currentSpeed;

        private enum State 
        {
            Approach,
            Strafe,
            Retreat,
        }

        protected override void Awake()
        {
            base.Awake();
            _movement = GetComponent<EnemyMovement>();
        }

        protected override void ResetStateForSpawn()
        {
            _state = State.Approach;
            _orbitDir = 1;
            _changeTimer = 0f;
            _brainTimer = 0f;
            _fireTimer = 0f;
            _currentSpeed = 0f;
            _currentTarget = transform.position;
            _movement?.SetSpeed(0f);
        }

        protected override void OnDespawn()
        {
            _movement?.SetSpeed(0f);
            CancelInvoke();
            StopAllCoroutines();
            var rb = GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        protected override void OnDeath()
        {
            _movement?.SetSpeed(0f);
        }

        private void Update()
        {
            if (!IsAlive || _player == null || _playerCol == null || _movement == null)
                return;

            _movement.SetSpeed(_currentSpeed);
            _movement.MoveForwardTo(_currentTarget);
            _brainTimer -= Time.deltaTime;

            if (_brainTimer <= 0f)
            {
                _brainTimer += _brainInterval;
                Think();
            }

            if (_state == State.Strafe)
                StrafeFrame();

            HandleFire();
        }

        public void Init(
            Transform player,
            Collider playerCollider,
            float approachSpeed,
            float attackSpeedFactorMin,
            float attackSpeedFactorMax,
            float keepMinFromSurface,
            float keepMaxFromSurface,
            float shootingDistance,
            Projectile projectilePrefab,
            float fireInterval,
            float projectileSpeed,
            int projectileDamage,
            float turnSpeed,
            float orbitSpeedDeg,
            Vector2 changeDirEvery,
            float checkRadius,
            float brainInterval = 0.15f,
            float fireAngle = 25f)
        {
            _player = player;
            _playerCol = playerCollider;
            _approachSpeed = approachSpeed;
            _attackSpeedFactorMin = attackSpeedFactorMin;
            _attackSpeedFactorMax = attackSpeedFactorMax;
            _keepMinSurf = keepMinFromSurface;
            _keepMaxSurf = keepMaxFromSurface;
            _shootingDistance = shootingDistance;
            _projectilePrefab = projectilePrefab;
            _fireInterval = fireInterval;
            _projectileSpeed = projectileSpeed;
            _projectileDamage = projectileDamage;
            _turnSpeed = turnSpeed;
            _orbitSpeedDeg = orbitSpeedDeg;
            _changeDirEvery = changeDirEvery;
            _checkRadiusSqr = checkRadius * checkRadius;
            _brainInterval = Mathf.Max(_minBrainInterval, brainInterval);
            _fireAngle = Mathf.Clamp(fireAngle, _minFireAngle, _maxFireAngle);

            if (Health != null && Health.IsDead)
            {
                enabled = false;
                return;
            }

            _movement?.SetTurnSpeed(_turnSpeed);
            _movement.SetSpeed(_approachSpeed);
            _brainTimer = Random.Range(0f, _brainInterval);
            _changeTimer = Random.Range(_changeDirEvery.x, _changeDirEvery.y);
            _fireTimer = Random.Range(0f, Mathf.Max(_minFireInterval, _fireInterval));
            EnterApproach();
        }

        private void Think()
        {
            float sqr = (_player.position - transform.position).sqrMagnitude;

            if (sqr > _checkRadiusSqr)
            {
                EnterApproach();
                return;
            }

            float distSurf = DistanceToPlayerSurface(transform.position);

            if (distSurf < _keepMinSurf - _hysteresisFactor)
            {
                EnterRetreat();
            }
            else if (distSurf > _keepMaxSurf + _hysteresisFactor)
            {
                EnterApproach();
            }
            else
            {
                EnterStrafe();
            }
        }

        private void EnterApproach()
        {
            _state = State.Approach;
            _currentSpeed = _approachSpeed;
            Vector3 pFlat = new Vector3(_player.position.x, transform.position.y, _player.position.z);
            Vector3 radial = transform.position - pFlat;
            float rLen = radial.magnitude;
            radial = rLen > _vectorMagnitudeTolerance ? radial / rLen : transform.forward;
            float targetCenterDist = ProjectCenterDistanceForSurface(_keepMaxSurf);
            _currentTarget = pFlat + (radial * targetCenterDist);
        }

        private void EnterRetreat()
        {
            _state = State.Retreat;
            _currentSpeed = _approachSpeed * _retreatSpeedMultiplier;
            Vector3 pFlat = new Vector3(_player.position.x, transform.position.y, _player.position.z);
            Vector3 radial = transform.position - pFlat;
            float rLen = radial.magnitude;
            radial = rLen > _vectorMagnitudeTolerance ? radial / rLen : -transform.forward;
            float targetCenterDist = ProjectCenterDistanceForSurface(_keepMinSurf);
            _currentTarget = pFlat + (radial * targetCenterDist);
        }

        private void EnterStrafe()
        {
            _state = State.Strafe;
            _currentSpeed = _approachSpeed * Mathf.Lerp(_attackSpeedFactorMin, _attackSpeedFactorMax, _strafeSpeedAverageFactor);

            if (_changeTimer <= 0f)
                _changeTimer = Random.Range(_changeDirEvery.x, _changeDirEvery.y);
        }

        private void StrafeFrame()
        {
            _changeTimer -= Time.deltaTime;

            if (_changeTimer <= 0f)
            {
                _orbitDir = (Random.value < 0.5f) ? -_orbitDir : _orbitDir;
                _changeTimer = Random.Range(_changeDirEvery.x, _changeDirEvery.y);
            }

            Vector3 pos = transform.position;
            Vector3 pFlat = new Vector3(_player.position.x, pos.y, _player.position.z);
            Vector3 radial = pos - pFlat;
            float radius = radial.magnitude;
            radial = radius > _vectorMagnitudeTolerance ? radial / radius : transform.forward;
            Vector3 tangent = Vector3.Cross(Vector3.up, radial).normalized * _orbitDir;
            float wRad = _orbitSpeedDeg * Mathf.Deg2Rad;
            float orbitStep = Mathf.Max(_minLengthThreshold, wRad * Mathf.Max(radius, _minRadiusThreshold)) * Time.deltaTime;
            float midSurf = 0.5f * (_keepMinSurf + _keepMaxSurf);
            float desiredR = Mathf.Lerp(radius, ProjectCenterDistanceForSurface(midSurf), _strafeInterpolationFactor);
            Vector3 ringBase = pFlat + (radial * desiredR);
            _currentTarget = ringBase + (tangent * orbitStep);
        }

        private void HandleFire()
        {
            if (_projectilePrefab == null || _firePoint == null)
                return;

            _fireTimer -= Time.deltaTime;

            if (_fireTimer > 0f)
                return;

            Vector3 aimPoint = _playerCol.ClosestPoint(_firePoint.position);
            float distSurf = Vector3.Distance(_firePoint.position, aimPoint);

            if (distSurf > _shootingDistance)
                return;

            Vector3 shootDir = aimPoint - _firePoint.position;
            float len = shootDir.magnitude;

            if (len < _minLengthThreshold)
                return;

            shootDir /= len;
            float angle = Vector3.Angle(transform.forward, shootDir);

            if (angle > _fireAngle)
                return;

            Fire(shootDir);
            _fireTimer = _fireInterval * Random.Range(_fireIntervalRandomMin, _fireIntervalRandomMax);
        }

        private void Fire(Vector3 dir)
        {
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            float maxDistance = Mathf.Max(_shootingDistance + _projectileDistanceBuffer, _projectileSpeed * (_fireInterval * _projectileLifetimeFactor));
            bool usePooling = _projectilePrefab.UsePooling;

            if (usePooling && ProjectilePool.Instance != null)
            {
                var proj = ProjectilePool.Instance.Spawn(
                    projectilePrefab: _projectilePrefab,
                    position: _firePoint.position,
                    rotation: rot,
                    owner: gameObject,
                    speed: _projectileSpeed,
                    damage: _projectileDamage,
                    maxDistance: maxDistance);
                proj.SetVelocity();
            }
            else
            {
                var proj = Instantiate(_projectilePrefab, _firePoint.position, rot);
                proj.Initial(
                    position: _firePoint.position,
                    rotation: rot,
                    owner: gameObject,
                    speed: _projectileSpeed,
                    damage: _projectileDamage,
                    maxAttackDistance: maxDistance,
                    usePooling: usePooling);
                proj.SetVelocity();
            }
        }

        private float DistanceToPlayerSurface(Vector3 worldPos)
        {
            Vector3 closest = _playerCol.ClosestPoint(worldPos);
            return Vector3.Distance(worldPos, closest);
        }

        private float ProjectCenterDistanceForSurface(float desiredSurf)
        {
            Vector3 pFlat = new Vector3(_player.position.x, transform.position.y, _player.position.z);
            float radialNow = (transform.position - pFlat).magnitude;
            float surfNow = DistanceToPlayerSurface(transform.position);
            float delta = desiredSurf - surfNow;
            return Mathf.Max(_minDistanceThreshold, radialNow + delta);
        }
    }
}
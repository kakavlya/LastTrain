using UnityEngine;

namespace LastTrain.Enemies
{
    public class EnemyShooterController : EnemyController
    {
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
        private bool _initialized;
        private State _state;
        private int _orbitDir = 1;
        private float _changeTimer;
        private float _brainInterval;
        private float _brainTimer;
        private float _fireTimer;
        private float _maxFireAngle;
        private Vector3 _currentTarget;
        private float _currentSpeed;

        private enum State { Approach, Strafe, Retreat }

        private void Update()
        {
            if (!_initialized || _player == null || _playerCol == null) return;

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
            float maxFireAngle = 25f
        )
        {
            _player = player;
            _playerCol = playerCollider;
            _movement = GetComponent<EnemyMovement>();
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
            _brainInterval = Mathf.Max(0.05f, brainInterval);
            _maxFireAngle = Mathf.Clamp(maxFireAngle, 1f, 179f);
            _movement.SetSpeed(_approachSpeed);
            _movement.SetTurnSpeed(_turnSpeed);
            _brainTimer = Random.Range(0f, _brainInterval);
            _changeTimer = Random.Range(_changeDirEvery.x, _changeDirEvery.y);
            _fireTimer = Random.Range(0f, _fireInterval);
            EnterApproach();
            _initialized = true;
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
            const float hysteresis = 0.5f;

            if (distSurf < _keepMinSurf - hysteresis)
            {
                EnterRetreat();
            }
            else if (distSurf > _keepMaxSurf + hysteresis)
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
            Vector3 radial = (transform.position - pFlat);
            float rLen = radial.magnitude;
            radial = rLen > 1e-4f ? radial / rLen : transform.forward;
            float targetCenterDist = ProjectCenterDistanceForSurface(_keepMaxSurf);
            _currentTarget = pFlat + radial * targetCenterDist;
        }

        private void EnterRetreat()
        {
            _state = State.Retreat;
            _currentSpeed = _approachSpeed * 1.1f;
            Vector3 pFlat = new Vector3(_player.position.x, transform.position.y, _player.position.z);
            Vector3 radial = (transform.position - pFlat);
            float rLen = radial.magnitude;
            radial = rLen > 1e-4f ? radial / rLen : -transform.forward;
            float targetCenterDist = ProjectCenterDistanceForSurface(_keepMinSurf);
            _currentTarget = pFlat + radial * targetCenterDist;
        }

        private void EnterStrafe()
        {
            _state = State.Strafe;
            _currentSpeed = _approachSpeed * Mathf.Lerp(_attackSpeedFactorMin, _attackSpeedFactorMax, 0.5f);

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
            Vector3 radial = (pos - pFlat);
            float radius = radial.magnitude;
            radial = radius > 1e-4f ? radial / radius : transform.forward;
            Vector3 tangent = Vector3.Cross(Vector3.up, radial).normalized * _orbitDir;
            float wRad = _orbitSpeedDeg * Mathf.Deg2Rad;
            float orbitStep = Mathf.Max(0.01f, wRad * Mathf.Max(radius, 0.1f)) * Time.deltaTime;
            float midSurf = 0.5f * (_keepMinSurf + _keepMaxSurf);
            float desiredR = Mathf.Lerp(radius, ProjectCenterDistanceForSurface(midSurf), 0.1f);
            Vector3 ringBase = pFlat + radial * desiredR;
            _currentTarget = ringBase + tangent * orbitStep;
        }

        private void HandleFire()
        {
            if (_projectilePrefab == null) return;

            _fireTimer -= Time.deltaTime;

            if (_fireTimer > 0f) return;

            Vector3 aimPoint = _playerCol.ClosestPoint(_firePoint.position);
            float distSurf = Vector3.Distance(_firePoint.position, aimPoint);

            if (distSurf > _shootingDistance) return;

            Vector3 shootDir = (aimPoint - _firePoint.position);
            float len = shootDir.magnitude;

            if (len < 0.01f) return;

            shootDir /= len;
            float angle = Vector3.Angle(transform.forward, shootDir);

            if (angle > _maxFireAngle) return;

            Fire(shootDir);
            _fireTimer = _fireInterval * Random.Range(0.95f, 1.05f);
        }

        private void Fire(Vector3 dir)
        {
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            var proj = Instantiate(_projectilePrefab, _firePoint.position, rot);
            float maxDistance = Mathf.Max(_shootingDistance + 2f, _projectileSpeed * (_fireInterval * 3f));
            bool usePooling = _projectilePrefab.UsePooling;
            proj.Initial(
                position: _firePoint.position,
                rotation: rot,
                owner: gameObject,
                speed: _projectileSpeed,
                damage: _projectileDamage,
                maxAttackDistance: maxDistance,
                usePooling: usePooling
            );

            proj.SetVelocity();
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
            return Mathf.Max(0.2f, radialNow + delta);
        }
    }
}

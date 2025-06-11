// EnemyShooterController.cs
using Assets.Source.Scripts.Player;
using Player;
using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyShooterController : MonoBehaviour
    {
        public enum State { Approach, Attack }

        [SerializeField] private Transform _firePoint;

        private Transform _player;
        private EnemyMovement _movement;
        private EnemyHealth _health;

        private float _approachSpeed;
        private float _attackSpeedFactorMin = 0.8f;
        private float _attackSpeedFactorMax = 1.2f;
        private float _speedChange = 2f; // how fast to change speed
        private float _turnSpeed;

        private float _shootingDistance;
        private Projectile _projectilePrefab;
        private float _fireInterval;
        private float _projectileSpeed;
        private int _projectileDamage;

        private float _currentSpeed;
        private float _targetSpeed;

        private State _state;
        private float _fireTimer;
        private TrainMovement _trainMovement;

        public void Init(
            Transform player,
            float approachSpeed,
            float attackSpeedFactorMin,
            float attackSpeedFactorMax,
            float shootingDistance,
            float turnSpeed,
            Projectile projectilePrefab,
            float fireInterval,
            float projectileSpeed,
            int projectileDamage,
            float speedChange
        )
        {
            //Debug.Log("Initializing Shooter on EnemyShooterController");
            _player = player;
            _movement = GetComponent<EnemyMovement>();
            _health = GetComponent<EnemyHealth>();
            _approachSpeed = approachSpeed;
            _attackSpeedFactorMin = attackSpeedFactorMin;
            _attackSpeedFactorMax = attackSpeedFactorMax;
            _shootingDistance = shootingDistance;
            _turnSpeed = turnSpeed;
            _projectilePrefab = projectilePrefab;
            _fireInterval = fireInterval;
            _projectileSpeed = projectileSpeed;
            _projectileDamage = projectileDamage;
            _speedChange = speedChange;

            _currentSpeed = _approachSpeed;
            _targetSpeed = _approachSpeed;

            _movement.SetSpeed(_currentSpeed);
            _movement.SetTurnSpeed(_turnSpeed);
            _trainMovement = _player.GetComponent<TrainMovement>();
            EnterApproach();
        }

        private void Update()
        {
            if (_player == null || _health == null || _health.IsDead)
                return;

            _currentSpeed = Mathf.MoveTowards(_currentSpeed,
                _targetSpeed, _speedChange * Time.deltaTime);

            
            _movement.SetSpeed(_currentSpeed);

            switch (_state)
            {
                case State.Approach: UpdateApproach(); break;
                case State.Attack: UpdateAttack(); break;
            }
            //Debug.Log($"Current state: {_state}");
            //Debug.Log($"Current speed: {_currentSpeed}");
        }

        private void EnterApproach()
        {
            //Debug.Log("Entering Approach State Speed is " + _moveSpeed);
            _state = State.Approach;
            _fireTimer = 0f;
        }

        private void UpdateApproach()
        {
            float dist = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(_player.position.x, 0, _player.position.z)
            );

            if (dist <= _shootingDistance)
            {
                _state = State.Attack;
                float playerSpeed = _trainMovement != null ? _trainMovement.Speed() : _approachSpeed;

                float factor = Random.Range(_attackSpeedFactorMin, _attackSpeedFactorMax);
                _targetSpeed = playerSpeed * factor;
                return;
            }
            //Vector3 dir = (_player.position - transform.position).WithY(0f).normalized;
            Vector3 dir = RotateToTrain();
            Vector3 targetPos = _player.position + dir * _shootingDistance;

            //_movement.MoveForward();
            _movement.MoveForwardTo(targetPos);

            //Debug.Log($"Moving to target: {targetPos}, speed : {_targetSpeed}");
        }
        private void EnterAttack()
        {
            _state = State.Attack;
            _fireTimer = _fireInterval;
        }

        private void UpdateAttack()
        {
            float dist = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(_player.position.x, 0, _player.position.z)
            );

            if (dist > _shootingDistance)
            {
                _state = State.Approach;
                _targetSpeed = _approachSpeed;
                return;
            }

            Vector3 dir = RotateToTrain();
            Vector3 targetPos = _player.position + dir * _shootingDistance;
            _movement.MoveForwardTo(targetPos);

            _fireTimer += Time.deltaTime;
            if (_fireTimer >= _fireInterval)
            {
                Fire();
                _fireTimer = 0f;
            }
        }

        private Vector3 RotateToTrain()
        {
            Vector3 dir = _player.position - transform.position;
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f)
            {
                var targetRot = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRot,
                    _turnSpeed * Time.deltaTime
                );
            }

            return dir;
        }

        private void Fire()
        {
            if (_projectilePrefab == null || _firePoint == null) return;

            var go = Instantiate(
                _projectilePrefab.gameObject,
                _firePoint.position,
                _firePoint.rotation
            );
            var proj = go.GetComponent<Projectile>();
            proj.Initial(position: _firePoint.position,
                            rotation: _firePoint.rotation,
                         owner: gameObject,
                         speed: _projectileSpeed,
                         damage: _projectileDamage,
                         maxAttackDistance: _shootingDistance,
                         usePooling: false
                         );
        }
    }
}

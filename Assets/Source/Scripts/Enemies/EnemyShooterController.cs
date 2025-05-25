// EnemyShooterController.cs
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

        private float _moveSpeed;
        private float _shootingDistance;
        private float _turnSpeed;
        private Projectile _projectilePrefab;
        private float _fireInterval;
        private float _projectileSpeed;
        private int _projectileDamage;

        private State _state;
        private float _fireTimer;

        public void Init(
            Transform player,
            float moveSpeed,
            float shootingDistance,
            float turnSpeed,
            Projectile projectilePrefab,
            float fireInterval,
            float projectileSpeed,
            int projectileDamage
        )
        {
            //Debug.Log("Initializing Shooter on EnemyShooterController");
            _player = player;
            _movement = GetComponent<EnemyMovement>();
            _moveSpeed = moveSpeed;
            _shootingDistance = shootingDistance;
            _turnSpeed = turnSpeed;
            _projectilePrefab = projectilePrefab;
            _fireInterval = fireInterval;
            _projectileSpeed = projectileSpeed;
            _projectileDamage = projectileDamage;

            _movement.SetSpeed(_moveSpeed);
            _movement.SetTurnSpeed(_turnSpeed);

            EnterApproach();
        }

        private void Update()
        {
            if (_player == null) return;

            switch (_state)
            {
                case State.Approach: UpdateApproach(); break;
                case State.Attack: UpdateAttack(); break;
            }
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

            //Debug.Log("Distance to player: " + dist);

            if (dist <= _shootingDistance)
            {
                EnterAttack();
                return;
            }

            Vector3 dir = (_player.position - transform.position).WithY(0f).normalized;
            Vector3 targetPos = _player.position + dir * _shootingDistance;

            _movement.MoveForwardTo(targetPos);
        }
        //private void UpdateApproach()
        //{
        //    // 1) Плоская дистанция до игрока
        //    float dist = Vector3.Distance(
        //        new Vector3(transform.position.x, 0, transform.position.z),
        //        new Vector3(_player.position.x, 0, _player.position.z)
        //    );

        //    //Debug.Log($"[Approach] dist={dist:F2}");


        //    if (dist <= _shootingDistance)
        //    {
        //        EnterAttack();
        //        return;
        //    }

        //    // 2) Поворот «сразу» к игроку по горизонтали
        //    Vector3 dir = _player.position - transform.position;
        //    dir.y = 0f;
        //    if (dir.sqrMagnitude > 0.001f)
        //    {
        //        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        //        transform.rotation = Quaternion.RotateTowards(
        //            transform.rotation,
        //            targetRot,
        //            _turnSpeed * Time.deltaTime
        //        );
        //    }

        //    // 3) Движение вперёд без рудиментов RotateTowards внутри Movement
        //    //_movement.MoveForward();
        //    Vector3 moveDir  = (_player.position - transform.position).normalized;
        //    moveDir.y = 0f;
        //    transform.position += moveDir * (_moveSpeed * Time.deltaTime);

        //    Vector3 oldpos = transform.position;
        //    Debug.Log($"[Approach] Moved from {oldpos:F2} -> {transform.position:F2}");
        //}

        private void EnterAttack()
        {
            _state = State.Attack;
            _fireTimer = _fireInterval;
        }

        private void UpdateAttack()
        {

            Vector3 dir = (_player.position - transform.position).WithY(0f).normalized;
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRot,
                    _turnSpeed * Time.deltaTime
                );
            }

            _fireTimer += Time.deltaTime;
            if (_fireTimer >= _fireInterval)
            {
                Fire();
                _fireTimer = 0f;
            }

            if (Vector3.Distance(transform.position, _player.position) > _shootingDistance)
                EnterApproach();
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
            //proj.Configure(
            //    owner: gameObject,
            //    usePooling: false
            //    //damage: _projectileDamage,
            //    //lifetime: proj.Lifetime,
            //    //speed: _projectileSpeed
            //);
        }
    }
}

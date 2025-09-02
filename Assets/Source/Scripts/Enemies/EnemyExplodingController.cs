using LastTrain.Core;
using UnityEngine;

namespace LastTrain.Enemies
{
    public class EnemyExplodingController : EnemyController
    {
        [SerializeField] private float _checkRadius = 10f;

        private Transform _player;
        private Collider _playerCollider;
        private EnemyMovement _movement;

        private float _explosionRadius;
        private int _damage;

        private bool _hasExploded;
        private float _checkRadiusSqr;

        protected override void Awake()
        {
            base.Awake();
            _movement = GetComponent<EnemyMovement>();
        }

        protected override void ResetStateForSpawn()
        {
            _hasExploded = false;
            _checkRadiusSqr = _checkRadius * _checkRadius;
            _movement?.SetSpeed(0f);
        }

        protected override void OnDespawn()
        {
            _movement?.SetSpeed(0f);
            CancelInvoke();
            StopAllCoroutines();
            var rb = GetComponent<Rigidbody>();
            if (rb != null) { rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }
        }

        protected override void OnDeath()
        {
            _hasExploded = true;
            _movement?.SetSpeed(0f);
        }

        public void Init(Transform player, Collider playerCollider, float speed, float explosionRadius, int damage)
        {
            _player = player;
            _playerCollider = playerCollider;
            _explosionRadius = Mathf.Max(0f, explosionRadius);
            _damage = damage;

            if (Health != null && Health.IsDead)
            {
                enabled = false;
                return;
            }

            _movement?.SetSpeed(speed);
        }

        private void Update()
        {
            if (!IsAlive || _hasExploded || _player == null || _playerCollider == null)
                return;

            _movement.MoveForwardTo(_player.position);

            Vector3 toPlayer = _player.position - transform.position;
            if (toPlayer.sqrMagnitude > _checkRadiusSqr)
                return;

            Vector3 closest = _playerCollider.ClosestPoint(transform.position);
            float dist = Vector3.Distance(closest, transform.position);
            if (dist <= _explosionRadius)
                Explode();
        }

        private void Explode()
        {
            if (_hasExploded || !IsAlive) return;

            _hasExploded = true;

            var dmg = _player.GetComponent<IDamageable>();
            if (dmg is HealthBase hb && hb.IsDead == false)
                hb.TakeDamage(_damage);
            else
                dmg?.TakeDamage(_damage);

            if (Health != null) Health.HandleDie();
            else enabled = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.35f);
            Gizmos.DrawSphere(transform.position, _explosionRadius);
        }
    }
}

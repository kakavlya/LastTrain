using UnityEngine;

namespace LastTrain.Enemies
{
    public class EnemyExplodingController : EnemyController
    {
        [SerializeField] private Collider _playerCollider;
        [SerializeField] private float _checkRadius = 10f;

        private Transform _player;
        private EnemyMovement _movement;
        private EnemyHealth _health;
        private float _explosionRadius;
        private int _damage;
        private bool _hasExploded;
        private float _checkRadiusSqr;

        public void Init(Transform player, Collider playerCollider, float speed, float explosionRadius, int damage)
        {
            _player = player;
            _playerCollider = playerCollider;
            _movement = GetComponent<EnemyMovement>();
            _movement.SetSpeed(speed);
            _health = GetComponent<EnemyHealth>();
            _explosionRadius = explosionRadius;
            _damage = damage;
            _hasExploded = false;
            _checkRadiusSqr = _checkRadius * _checkRadius;
        }

        private void Update()
        {
            if (_hasExploded || _player == null || _playerCollider == null)
                return;

            _movement.MoveForwardTo(_player.position);

            if ((_player.position - transform.position).sqrMagnitude > _checkRadiusSqr)
                return;

            Vector3 closest = _playerCollider.ClosestPoint(transform.position);
            float dist = Vector3.Distance(closest, transform.position);

            if (dist <= _explosionRadius)
            {
                Explode();
            }
        }

        private void Explode()
        {
            if (_hasExploded) return;

            _hasExploded = true;
            _player.GetComponent<IDamageable>()?.TakeDamage(_damage);
            _health?.HandleDie();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
            Gizmos.DrawSphere(transform.position, _explosionRadius);
        }
    }
    public static class Vector3Extensions
    {
        public static Vector3 Flat(this Vector3 v) => new Vector3(v.x, 0f, v.z);
    }
}
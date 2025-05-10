using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    public class EnemyExplodingController : MonoBehaviour
    {

        private Transform _player;
        private EnemyMovement _movement;
        private float _explosionRadius;
        private int _damage;
        private bool _hasExploded;

        public void Init(Transform player, float speed, float explosionRadius, int damage)
        {
            _player = player;
            _movement = GetComponent<EnemyMovement>();
            _movement.SetSpeed(speed);

            _explosionRadius = explosionRadius;
            _damage = damage;
            _hasExploded = false;
        }

        private void Update()
        {
            if (_hasExploded || _player == null)
                return;

            _movement.MoveForwardTo(_player.position);

            float dist = Vector3.Distance(transform.position.Flat(), _player.position.Flat());
            if (dist <= _explosionRadius)
                Explode();
        }

        private void Explode()
        {
            _hasExploded = true;
            _player.GetComponent<IDamageable>()?.TakeDamage(_damage);
            //Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);
            //foreach (var hit in hits)
            //{
            //    // Maybe Playerhealth can be replaced by something more generic, like shield or armor
            //    if (hit.TryGetComponent<PlayerHealth>(out var playerHealth))
            //    {
            //        Debug.Log(hit.name);
            //        playerHealth.TakeDamage(_damage);
            //    }
            //}

            // TODO: запустить VFX/SFX взрыва

            Destroy(gameObject);
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
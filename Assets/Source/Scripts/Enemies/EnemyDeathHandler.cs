using UnityEngine;
using LastTrain.Effects;

namespace LastTrain.Enemies
{
    public class EnemyDeathHandler : MonoBehaviour
    {
        [SerializeField] private ModelEffects _enemyView;
        [SerializeField] private EnemyDeathEffect _deathEffect;
        [SerializeField] private float _delayBeforeDespawn = 2f;

        private EnemyMovement _movement;
        private EnemyController _controller;
        private VisualWobble _wobble;
        private Collider[] _collidersToToggle;
        private Rigidbody _rb;

        private bool _despawnScheduled;

        private void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
            _controller = GetComponent<EnemyController>();
            _wobble = GetComponentInChildren<VisualWobble>(true);
            _collidersToToggle = GetComponentsInChildren<Collider>(true);
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _despawnScheduled = false;

            if (_movement != null) _movement.enabled = true;
            if (_controller != null) _controller.enabled = true;
            if (_wobble != null) _wobble.enabled = true;

            foreach (var col in _collidersToToggle)
                if (col != null) col.enabled = true;

            if (_rb != null)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }

            _deathEffect?.ResetEffect();
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(Despawn));
        }

        public void HandleDeath()
        {
            if (_despawnScheduled) return;
            _despawnScheduled = true;

            if (_movement != null)
            {
                _movement.SetSpeed(0f);
                _movement.enabled = false;
            }

            if (_controller != null) _controller.enabled = false;
            if (_wobble != null) _wobble.enabled = false;

            foreach (var col in _collidersToToggle)
                if (col != null) col.enabled = false;

            if (_rb != null)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }

            _enemyView?.PlayDeathFX();
            _deathEffect?.Play();

            Invoke(nameof(Despawn), _delayBeforeDespawn);
        }

        private void Despawn()
        {
            if (!gameObject.activeInHierarchy) return;

            EnemyPool.Instance.ReleaseEnemy(gameObject);
        }
    }
}

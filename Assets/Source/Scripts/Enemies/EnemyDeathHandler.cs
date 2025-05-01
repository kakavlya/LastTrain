using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    public class EnemyDeathHandler : MonoBehaviour
    {
        private EnemyMovement _movement;
        private EnemyController _controller;
        private EnemyVisualWobble _wobble;
        private Collider[] _collidersToDisable;

        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private EnemyDeathEffect _deathEffect;

        [SerializeField] private float _delayBeforeDespawn = 2f;
        [SerializeField] private bool _useObjectPool = false;

        private void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
            _controller = GetComponent<EnemyController>();
            _wobble = GetComponentInChildren<EnemyVisualWobble>();
            _collidersToDisable = GetComponentsInChildren<Collider>();
        }


        public void HandleDeath()
        {
            if (_movement != null) _movement.enabled = false;
            if (_controller != null) _controller.enabled = false;
            if (_wobble != null) _wobble.enabled = false;

            foreach (var col in _collidersToDisable)
                if (col != null) col.enabled = false;

            _enemyView?.PlayDeathFX();
            _deathEffect?.Play();

            Invoke(nameof(DespawnOrDestroy), _delayBeforeDespawn);
        }

        private void DespawnOrDestroy()
        {
            // Todo: Implement object pooling logic here if useObjectPool is true
            Destroy(gameObject);
        }

        public void ResetState()
        {
            if (_movement != null) _movement.enabled = true;
            if (_controller != null) _controller.enabled = true;
            if (_wobble != null) _wobble.enabled = true;

            foreach (var col in _collidersToDisable)
                if (col != null) col.enabled = true;

            _deathEffect?.ResetEffect();
        }
    }
}
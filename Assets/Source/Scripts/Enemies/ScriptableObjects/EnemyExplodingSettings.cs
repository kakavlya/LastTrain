using UnityEngine;

namespace LastTrain.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Explode", fileName = "NewExplodeSettings")]
    public class EnemyExplodingSettings : EnemyBehaviorSettings
    {
        [Header("Movement")]
        [Tooltip("Movement to target")]
        [SerializeField] private float _moveSpeed = 5f;

        [Header("Explosion")]
        [SerializeField] private float _explosionRadius = 3f;

        [Tooltip("Damage in radius")]
        [SerializeField] private int _damage = 100;

        public override void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider)
        {
            var exploder = enemy.GetComponent<EnemyExplodingController>();

            if (exploder == null)
                exploder = enemy.AddComponent<EnemyExplodingController>();

            exploder.Init(
                player: playerTarget,
                playerCollider: playerCollider,
                speed: _moveSpeed,
                explosionRadius: _explosionRadius,
                damage: _damage);
        }
    }
}
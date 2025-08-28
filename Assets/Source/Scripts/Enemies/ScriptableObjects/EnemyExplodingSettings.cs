using UnityEngine;

namespace LastTrain.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Explode", fileName = "NewExplodeSettings")]
    public class EnemyExplodingSettings : EnemyBehaviorSettings
    {
        [Header("Movement")]
        [Tooltip("Movement to target")]
        public float moveSpeed = 5f;

        [Header("Explosion")]
        public float explosionRadius = 3f;

        [Tooltip("Damage in radius")]
        public int damage = 100;

        public override void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider)
        {
            var exploder = enemy.GetComponent<EnemyExplodingController>();
            if (exploder == null)
                exploder = enemy.AddComponent<EnemyExplodingController>();

            exploder.Init(
                player: playerTarget,
                playerCollider: playerCollider,
                speed: moveSpeed,
                explosionRadius: explosionRadius,
                damage: damage
            );
        }
    }
}
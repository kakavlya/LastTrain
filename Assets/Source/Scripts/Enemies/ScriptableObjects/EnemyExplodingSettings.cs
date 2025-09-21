using UnityEngine;
using UnityEngine.Serialization;

namespace LastTrain.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Explode", fileName = "NewExplodeSettings")]
    public class EnemyExplodingSettings : EnemyBehaviorSettings
    {
        [Header("Movement")]
        [Tooltip("Movement to target")]
        [FormerlySerializedAs("moveSpeed")]
        public float MoveSpeed = 5f;

        [Header("Explosion")]
        [FormerlySerializedAs("explosionRadius")]
        public float ExplosionRadius = 3f;

        [Tooltip("Damage in radius")]
        [FormerlySerializedAs("damage")]
        public int Damage = 100;

        public override void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider)
        {
            var exploder = enemy.GetComponent<EnemyExplodingController>();
            if (exploder == null)
                exploder = enemy.AddComponent<EnemyExplodingController>();

            exploder.Init(
                player: playerTarget,
                playerCollider: playerCollider,
                speed: MoveSpeed,
                explosionRadius: ExplosionRadius,
                damage: Damage
            );
        }
    }
}
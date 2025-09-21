using UnityEngine;
using UnityEngine.Serialization;

namespace LastTrain.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Explode", fileName = "NewExplodeSettings")]
    public class EnemyExplodingSettings : EnemyBehaviorSettings
    {
        [Header("Movement")]
        [Tooltip("Movement to target")]
        public float MoveSpeed = 5f;

        [Header("Explosion")]
        public float ExplosionRadius = 3f;

        [Tooltip("Damage in radius")]
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
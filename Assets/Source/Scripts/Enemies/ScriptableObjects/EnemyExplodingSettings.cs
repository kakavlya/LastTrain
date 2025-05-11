using System.Collections;
using UnityEngine;

namespace Assets.Source.Scripts.Enemies
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
        public override void Initialize(GameObject enemy, Transform playerTarget)
        {
            var exploder = enemy.GetComponent<EnemyExplodingController>();
            if (exploder == null)
                exploder = enemy.AddComponent<EnemyExplodingController>();

            exploder.Init(
                player: playerTarget,
                speed: moveSpeed,
                explosionRadius: explosionRadius,
                damage: damage
            );
        }
    }
}
using Assets.Source.Scripts.Enemies;
using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Ram")]
    public class EnemyRamSettings : EnemyBehaviorSettings
    {
        [Header("Ram Distances")]
        public float holdDistance;
        public float impactOffset;

        [Header("Ram Speeds")]
        public float holdSpeed;
        public float chargeSpeed;
        public float turnSpeed;

        [Header("Ram Delays")]
        public float impactPause;
        public Vector2 holdPauseRange;

        [Header("Ram Damage")]
        public int damage;

        public override void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider)
        {
            var ram = enemy.GetComponent<EnemyRamController>();
            if (ram == null) ram = enemy.AddComponent<EnemyRamController>();

            ram.Init(
                player: playerTarget,
                playerCollider: playerCollider,
                impactOffset: impactOffset,
                holdDistance: holdDistance,
                holdSpeed: holdSpeed,
                chargeSpeed: chargeSpeed,
                impactPause: impactPause,
                holdPauseRange: holdPauseRange,
                damage: damage
            );
        }
    }
}

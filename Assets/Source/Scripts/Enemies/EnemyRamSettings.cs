using Assets.Source.Scripts.Enemies;
using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Ram")]
    public class EnemyRamSettings : EnemyBehaviorSettings
    {
        [Header("Ram Distances")]
        public float holdDistance;
        public float impactRadius;
        public float retreatDistance;

        [Header("Ram Speeds")]
        public float holdSpeed;
        public float chargeSpeed;
        public float retreatSpeed;
        public float turnSpeed;

        [Header("Ram Delays")]
        public float impactPause;
        public Vector2 holdPauseRange;

        [Header("Ram Damage")]
        public int damage;

        public override void Initialize(GameObject enemy, Transform playerTarget)
        {
            var ram = enemy.GetComponent<EnemyRamController>();
            if (ram == null) ram = enemy.AddComponent<EnemyRamController>();

            ram.Init(
                player: playerTarget,
                holdDistance: holdDistance,
                impactRadius: impactRadius,
                retreatDistance: retreatDistance,
                holdSpeed: holdSpeed,
                chargeSpeed: chargeSpeed,
                retreatSpeed: retreatSpeed,
                impactPause: impactPause,
                holdPauseRange: holdPauseRange,
                turnSpeed: turnSpeed,
                damage: damage
            );
        }
    }
}

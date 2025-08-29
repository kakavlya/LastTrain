using UnityEngine;

namespace LastTrain.Enemies
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

        [Header("Ram Tuning (Dynamics)")]
        public float maxAccel = 30f;
        public float maxDecel = 40f;
        public float speedSmoothTime = 0.12f;
        public float checkRadius = 10f;
        public AnimationCurve impactRecover = AnimationCurve.EaseInOut(0, 0, 1, 1);

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

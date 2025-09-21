using UnityEngine;

namespace LastTrain.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Ram")]
    public class EnemyRamSettings : EnemyBehaviorSettings
    {
        [Header("Ram Distances")]
        public float HoldDistance;
        public float ImpactOffset;

        [Header("Ram Speeds")]
        public float HoldSpeed;
        public float ChargeSpeed;
        public float TurnSpeed;

        [Header("Ram Delays")]
        public float ImpactPause;
        public Vector2 HoldPauseRange;

        [Header("Ram Damage")]
        public int Damage;

        [Header("Ram Tuning (Dynamics)")]
        public float MaxAccel = 30f;
        public float MaxDecel = 40f;
        public float SpeedSmoothTime = 0.12f;
        public float CheckRadius = 10f;
        public AnimationCurve ImpactRecover = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public override void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider)
        {
            var ram = enemy.GetComponent<EnemyRamController>();

            if (ram == null) 
                ram = enemy.AddComponent<EnemyRamController>();

            ram.Init(
                player: playerTarget,
                playerCollider: playerCollider,
                impactOffset: ImpactOffset,
                holdDistance: HoldDistance,
                holdSpeed: HoldSpeed,
                chargeSpeed: ChargeSpeed,
                impactPause: ImpactPause,
                holdPauseRange: HoldPauseRange,
                damage: Damage
            );
        }
    }
}

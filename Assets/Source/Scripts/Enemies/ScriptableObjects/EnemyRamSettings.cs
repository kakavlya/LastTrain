using UnityEngine;

namespace LastTrain.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Ram")]
    public class EnemyRamSettings : EnemyBehaviorSettings
    {
        [Header("Ram Distances")]
        [SerializeField] private float _holdDistance;
        [SerializeField] private float _impactOffset;

        [Header("Ram Speeds")]
        [SerializeField] private float _holdSpeed;
        [SerializeField] private float _chargeSpeed;
        [SerializeField] private float _turnSpeed;

        [Header("Ram Delays")]
        [SerializeField] private float _impactPause;
        [SerializeField] private Vector2 _holdPauseRange;

        [Header("Ram Damage")]
        [SerializeField] private int _damage;
        
        [Header("Ram Tuning (Dynamics)")]
        [SerializeField] private float _maxAccel = 30f;
        [SerializeField] private float _maxDecel = 40f;
        [SerializeField] private float _speedSmoothTime = 0.12f;
        [SerializeField] private float _checkRadius = 10f;

        [SerializeField] private AnimationCurve _impactRecover = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public override void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider)
        {
            var ram = enemy.GetComponent<EnemyRamController>();

            if (ram == null)
                ram = enemy.AddComponent<EnemyRamController>();

            ram.Init(
                player: playerTarget,
                playerCollider: playerCollider,
                impactOffset: _impactOffset,
                holdDistance: _holdDistance,
                holdSpeed: _holdSpeed,
                chargeSpeed: _chargeSpeed,
                impactPause: _impactPause,
                holdPauseRange: _holdPauseRange,
                damage: _damage
            );
        }
    }
}

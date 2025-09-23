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

        [Header("Ram Delays")]
        [SerializeField] private float _impactPause;
        [SerializeField] private Vector2 _holdPauseRange;

        [Header("Ram Damage")]
        [SerializeField] private int _damage;

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

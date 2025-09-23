using LastTrain.Projectiles.Types;
using UnityEngine;

namespace LastTrain.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Shooter", fileName = "NewShooterSettings")]
    public class EnemyShooterSettings : EnemyBehaviorSettings
    {
        [Header("Movement")]
        [Tooltip("Base approach/retreat movement speed (m/s). Typically 3–6 m/s.")]
        [Min(0f)] [SerializeField] private float _moveSpeed = 4f;

        [Tooltip("Maximum vertical axis rotation speed (deg/s). 90–180 — responsive.")]
        [Range(45f, 360f)] [SerializeField] private float _turnSpeed = 120f;

        [Tooltip("Minimum speed multiplier when strafing/flanking." +
            " Keep it at 0.6-0.9 to make the shooter slightly slower than when closing in.")]
        [Range(0.3f, 1.0f)] [SerializeField] private float _attackSpeedFactorMin = 0.7f;

        [Tooltip("Maximum speed multiplier during strafing/flanking. Must be ≥ minimum. Typically 0.8–0.95.")]
        [Range(0.3f, 1.0f)] [SerializeField] private float _attackSpeedFactorMax = 0.9f;

        [Tooltip("Smoothing of speed changes (acceleration/deceleration)." +
            " Currently not used by the controller, reserve for the future.")]
        [SerializeField] private float _speedChange = 10f;

        [Tooltip("Minimum distance from the player's SURFACE (in meters). Recommended 5-15.")]
        [Min(0.1f)] [SerializeField] private float _minDistanceFromSurface = 5f;

        [Tooltip("Maximum distance from the player's SURFACE (in meters)." +
            " Should be greater than the minimum. Recommended 15-35.")]
        [Min(0.5f)] [SerializeField] private float _maxDistanceFromSurface = 25f;

        [Tooltip("Angular speed of circling around the player (deg/s). 5–90. More – faster circling.")]
        [Range(1f, 180f)] [SerializeField] private float _orbitSpeedDegrees = 5f;

        [Tooltip("Radius of early check from player CENTER (m). Set ≥ (MaxDistanceFromSurface + 3).")]
        [Min(1f)] [SerializeField] private float _checkRadius = 10f;

        [Tooltip("Interval of possible change of bypass direction [min, max] in seconds. Usually 1.5–4.")]
        [SerializeField] private Vector2 _changeDirectionEvery = new(3f, 4f);

        [Header("Shooting")]
        [Tooltip("The maximum distance from the player's SURFACE that shooting is allowed." +
            " Usually ≥ MinDistanceFromSurface.")]
        [Min(0.1f)] [SerializeField] private float _shootingDistance = 20f;

        [Tooltip("Projectile Prefab.")]
        [SerializeField] private Projectile _projectilePrefab;

        [Tooltip("Pause between shots (sec). Typically 0.2–2.0.")]
        [Min(0.05f)] [SerializeField] private float _fireInterval = 1.5f;

        [Tooltip("Projectile speed (m/s).")]
        [Min(0.1f)] [SerializeField] private float _projectileSpeed = 12f;

        [Tooltip("Damage per projectile.")]
        [Min(0)] [SerializeField] private int _projectileDamage = 25;

        public override void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider)
        {
            var shooter = enemy.GetComponent<EnemyShooterController>();

            if (shooter == null)
                shooter = enemy.AddComponent<EnemyShooterController>();

            float safeCheckRadius = Mathf.Max(_checkRadius, _maxDistanceFromSurface + 3f);
            float safeShootingDist = Mathf.Max(_shootingDistance, _minDistanceFromSurface + 0.25f);

            shooter.Init(
                player: playerTarget,
                playerCollider: playerCollider,
                approachSpeed: _moveSpeed,
                attackSpeedFactorMin: _attackSpeedFactorMin,
                attackSpeedFactorMax: _attackSpeedFactorMax,
                keepMinFromSurface: _minDistanceFromSurface,
                keepMaxFromSurface: _maxDistanceFromSurface,
                shootingDistance: safeShootingDist,
                projectilePrefab: _projectilePrefab,
                fireInterval: _fireInterval,
                projectileSpeed: _projectileSpeed,
                projectileDamage: _projectileDamage,
                turnSpeed: _turnSpeed,
                orbitSpeedDeg: _orbitSpeedDegrees,
                changeDirEvery: _changeDirectionEvery,
                checkRadius: safeCheckRadius
            );
        }
    }
}

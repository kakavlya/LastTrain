using UnityEngine;

namespace LastTrain.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Shooter", fileName = "NewShooterSettings")]
    public class EnemyShooterSettings : EnemyBehaviorSettings
    {
        [Header("Movement")]
        [Tooltip("Base approach/retreat movement speed (m/s). Typically 3–6 m/s.")]
        [Min(0f)] public float moveSpeed = 4f;

        [Tooltip("Maximum vertical axis rotation speed (deg/s). 90–180 — responsive.")]
        [Range(45f, 360f)] public float turnSpeed = 120f;

        [Tooltip("Minimum speed multiplier when strafing/flanking." +
            " Keep it at 0.6-0.9 to make the shooter slightly slower than when closing in.")]
        [Range(0.3f, 1.0f)] public float attackSpeedFactorMin = 0.7f;

        [Tooltip("Maximum speed multiplier during strafing/flanking. Must be ≥ minimum. Typically 0.8–0.95.")]
        [Range(0.3f, 1.0f)] public float attackSpeedFactorMax = 0.9f;

        [Tooltip("Smoothing of speed changes (acceleration/deceleration)." +
            " Currently not used by the controller, reserve for the future.")]
        public float speedChange = 10f;

        [Tooltip("Minimum distance from the player's SURFACE (in meters). Recommended 5-15.")]
        [Min(0.1f)] public float minDistanceFromSurface = 5f;

        [Tooltip("Maximum distance from the player's SURFACE (in meters)." +
            " Should be greater than the minimum. Recommended 15-35.")]
        [Min(0.5f)] public float maxDistanceFromSurface = 25f;

        [Tooltip("Angular speed of circling around the player (deg/s). 5–90. More – faster circling.")]
        [Range(1f, 180f)] public float orbitSpeedDegrees = 5f;

        [Tooltip("Radius of early check from player CENTER (m). Set ≥ (maxDistanceFromSurface + 3).")]
        [Min(1f)] public float checkRadius = 10f;

        [Tooltip("Interval of possible change of bypass direction [min, max] in seconds. Usually 1.5–4.")]
        public Vector2 changeDirectionEvery = new(3f, 4f);

        [Header("Shooting")]
        [Tooltip("The maximum distance from the player's SURFACE that shooting is allowed." +
            " Usually ≥ minDistanceFromSurface.")]
        [Min(0.1f)] public float shootingDistance = 20f;

        [Tooltip("Projectile prefab.")]
        public Projectile projectilePrefab;

        [Tooltip("Pause between shots (sec). Typically 0.2–2.0.")]
        [Min(0.05f)] public float fireInterval = 1.5f;

        [Tooltip("Projectile speed (m/s).")]
        [Min(0.1f)] public float projectileSpeed = 12f;

        [Tooltip("Damage per projectile.")]
        [Min(0)] public int projectileDamage = 25;

        public override void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider)
        {
            var shooter = enemy.GetComponent<EnemyShooterController>();

            if (shooter == null)
                shooter = enemy.AddComponent<EnemyShooterController>();

            float safeCheckRadius = Mathf.Max(checkRadius, maxDistanceFromSurface + 3f);
            float safeShootingDist = Mathf.Max(shootingDistance, minDistanceFromSurface + 0.25f);

            shooter.Init(
                player: playerTarget,
                playerCollider: playerCollider,
                approachSpeed: moveSpeed,
                attackSpeedFactorMin: attackSpeedFactorMin,
                attackSpeedFactorMax: attackSpeedFactorMax,
                keepMinFromSurface: minDistanceFromSurface,
                keepMaxFromSurface: maxDistanceFromSurface,
                shootingDistance: safeShootingDist,
                projectilePrefab: projectilePrefab,
                fireInterval: fireInterval,
                projectileSpeed: projectileSpeed,
                projectileDamage: projectileDamage,
                turnSpeed: turnSpeed,
                orbitSpeedDeg: orbitSpeedDegrees,
                changeDirEvery: changeDirectionEvery,
                checkRadius: safeCheckRadius
            );
        }
    }
}

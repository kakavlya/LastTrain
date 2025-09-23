using LastTrain.Projectiles.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace LastTrain.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Shooter", fileName = "NewShooterSettings")]
    public class EnemyShooterSettings : EnemyBehaviorSettings
    {
        [Header("Movement")]
        [Tooltip("Base approach/retreat movement speed (m/s). Typically 3–6 m/s.")]
        [FormerlySerializedAs ("MoveSpeed")]
        [Min(0f)] public float MoveSpeed = 4f;

        [Tooltip("Maximum vertical axis rotation speed (deg/s). 90–180 — responsive.")]
        [FormerlySerializedAs("TurnSpeed")]
        [Range(45f, 360f)] public float TurnSpeed = 120f;

        [Tooltip("Minimum speed multiplier when strafing/flanking." +
            " Keep it at 0.6-0.9 to make the shooter slightly slower than when closing in.")]
        [FormerlySerializedAs("AttackSpeedFactorMin")]
        [Range(0.3f, 1.0f)] public float AttackSpeedFactorMin = 0.7f;

        [Tooltip("Maximum speed multiplier during strafing/flanking. Must be ≥ minimum. Typically 0.8–0.95.")]
        [FormerlySerializedAs("AttackSpeedFactorMax")]
        [Range(0.3f, 1.0f)] public float AttackSpeedFactorMax = 0.9f;

        [Tooltip("Smoothing of speed changes (acceleration/deceleration)." +
            " Currently not used by the controller, reserve for the future.")]
        [FormerlySerializedAs("SpeedChange")]
        public float SpeedChange = 10f;

        [Tooltip("Minimum distance from the player's SURFACE (in meters). Recommended 5-15.")]
        [FormerlySerializedAs("MinDistanceFromSurface")]
        [Min(0.1f)] public float MinDistanceFromSurface = 5f;

        [Tooltip("Maximum distance from the player's SURFACE (in meters)." +
            " Should be greater than the minimum. Recommended 15-35.")]
        [FormerlySerializedAs("MaxDistanceFromSurface")]
        [Min(0.5f)] public float MaxDistanceFromSurface = 25f;

        [Tooltip("Angular speed of circling around the player (deg/s). 5–90. More – faster circling.")]
        [FormerlySerializedAs("OrbitSpeedDegrees")]
        [Range(1f, 180f)] public float OrbitSpeedDegrees = 5f;

        [Tooltip("Radius of early check from player CENTER (m). Set ≥ (MaxDistanceFromSurface + 3).")]
        [FormerlySerializedAs("CheckRadius")]
        [Min(1f)] public float CheckRadius = 10f;

        [Tooltip("Interval of possible change of bypass direction [min, max] in seconds. Usually 1.5–4.")]
        [FormerlySerializedAs("ChangeDirectionEvery")]
        public Vector2 ChangeDirectionEvery = new(3f, 4f);

        [Header("Shooting")]
        [Tooltip("The maximum distance from the player's SURFACE that shooting is allowed." +
            " Usually ≥ MinDistanceFromSurface.")]
        [FormerlySerializedAs("ShootingDistance")]
        [Min(0.1f)] public float ShootingDistance = 20f;

        [Tooltip("Projectile Prefab.")]
        [FormerlySerializedAs("ProjectilePrefab")]
        public Projectile ProjectilePrefab;

        [Tooltip("Pause between shots (sec). Typically 0.2–2.0.")]
        [FormerlySerializedAs("FireInterval")]
        [Min(0.05f)] public float FireInterval = 1.5f;

        [Tooltip("Projectile speed (m/s).")]
        [FormerlySerializedAs("ProjectileSpeed")]
        [Min(0.1f)] public float ProjectileSpeed = 12f;

        [Tooltip("Damage per projectile.")]
        [FormerlySerializedAs("ProjectileDamage")]
        [Min(0)] public int ProjectileDamage = 25;

        public override void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider)
        {
            var shooter = enemy.GetComponent<EnemyShooterController>();

            if (shooter == null)
                shooter = enemy.AddComponent<EnemyShooterController>();

            float safeCheckRadius = Mathf.Max(CheckRadius, MaxDistanceFromSurface + 3f);
            float safeShootingDist = Mathf.Max(ShootingDistance, MinDistanceFromSurface + 0.25f);

            shooter.Init(
                player: playerTarget,
                playerCollider: playerCollider,
                approachSpeed: MoveSpeed,
                attackSpeedFactorMin: AttackSpeedFactorMin,
                attackSpeedFactorMax: AttackSpeedFactorMax,
                keepMinFromSurface: MinDistanceFromSurface,
                keepMaxFromSurface: MaxDistanceFromSurface,
                shootingDistance: safeShootingDist,
                projectilePrefab: ProjectilePrefab,
                fireInterval: FireInterval,
                projectileSpeed: ProjectileSpeed,
                projectileDamage: ProjectileDamage,
                turnSpeed: TurnSpeed,
                orbitSpeedDeg: OrbitSpeedDegrees,
                changeDirEvery: ChangeDirectionEvery,
                checkRadius: safeCheckRadius
            );
        }
    }
}

using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Shooter", fileName = "NewShooterSettings")]
    public class EnemyShooterSettings : EnemyBehaviorSettings
    {
        [Header("Движение")]
        [Tooltip("Базовая скорость передвижения при сближении/отходе (м/с). Обычно 3–6 м/с.")]
        [Min(0f)] public float moveSpeed = 4f;

        [Tooltip("Максимальная скорость поворота по вертикальной оси (град/с). 90–180 — отзывчиво.")]
        [Range(45f, 360f)] public float turnSpeed = 120f;

        [Tooltip("Минимальный множитель скорости во время обстрела/обхода. Держите 0.6–0.9, чтобы стрелок был чуть медленнее, чем при сближении.")]
        [Range(0.3f, 1.0f)] public float attackSpeedFactorMin = 0.7f;

        [Tooltip("Максимальный множитель скорости во время обстрела/обхода. Должен быть ≥ минимального. Обычно 0.8–0.95.")]
        [Range(0.3f, 1.0f)] public float attackSpeedFactorMax = 0.9f;

        [Tooltip("Сглаживание изменения скорости (ускорение/замедление). Сейчас контроллером не используется, задел на будущее.")]
        public float speedChange = 10f;

        [Tooltip("Минимальная дистанция от ПОВЕРХНОСТИ игрока (в метрах). Рекомендовано 5-15.")]
        [Min(0.1f)] public float minDistanceFromSurface = 5f;

        [Tooltip("Максимальная дистанция от ПОВЕРХНОСТИ игрока (в метрах). Должна быть больше минимальной. Рекомендовано 15–35.")]
        [Min(0.5f)] public float maxDistanceFromSurface = 25f;

        [Tooltip("Угловая скорость обхода вокруг игрока (град/с). 5–90. Больше — быстрее кружит.")]
        [Range(1f, 180f)] public float orbitSpeedDegrees = 5f;

        [Tooltip("Радиус «ранней» проверки от ЦЕНТРА игрока (м). Задайте ≥ (maxDistanceFromSurface + 3).")]
        [Min(1f)] public float checkRadius = 10f;

        [Tooltip("Интервал возможной смены направления обхода [мин, макс] в секундах. Обычно 1.5–4.")]
        public Vector2 changeDirectionEvery = new(3f, 4f);

        [Header("Стрельба")]
        [Tooltip("Максимальная дистанция от ПОВЕРХНОСТИ игрока, на которой разрешено стрелять. Обычно ≥ minDistanceFromSurface.")]
        [Min(0.1f)] public float shootingDistance = 20f;

        [Tooltip("Префаб снаряда.")]
        public Projectile projectilePrefab;

        [Tooltip("Пауза между выстрелами (сек). Типично 0.2–2.0.")]
        [Min(0.05f)] public float fireInterval = 1.5f;

        [Tooltip("Скорость снаряда (м/с).")]
        [Min(0.1f)] public float projectileSpeed = 12f;

        [Tooltip("Урон за один снаряд.")]
        [Min(0)] public int projectileDamage = 25;

        public override void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider)
        {
            var shooter = enemy.GetComponent<EnemyShooterController>();
            if (shooter == null) shooter = enemy.AddComponent<EnemyShooterController>();

            // Санити: гарантируем разумные зависимости
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

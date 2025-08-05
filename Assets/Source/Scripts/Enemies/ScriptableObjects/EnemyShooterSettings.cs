using Assets.Source.Scripts.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Behavior/Shooter", fileName = "NewShooterSettings")]
    public class EnemyShooterSettings : EnemyBehaviorSettings
    {
        [Header("Movement")]
        [Tooltip("Mooving speed")]
        public float moveSpeed = 4f;
        public float turnSpeed = 120f;
        public float attackSpeedFactorMin = 0.7f;
        public float attackSpeedFactorMax = 0.9f;
        public float speedChange = 10f;
        [Tooltip("Minimal shooting distance")]
        public float shootingDistance = 10f;

        [Header("Shooting")]
        [Tooltip("Projectile prefab")]
        public Projectile projectilePrefab;
        [Tooltip("Spawn Point")]
        public float fireInterval = 1.5f;
        [Tooltip("Speed of the projectile")]
        public float projectileSpeed = 12f;
        [Tooltip("Projectile Damage")]
        public int projectileDamage = 25;

        public override void Initialize(GameObject enemy, Transform playerTarget, BoxCollider playerCollider)
        {
            var shooter = enemy.GetComponent<EnemyShooterController>();
            if (shooter == null) shooter = enemy.AddComponent<EnemyShooterController>();

            shooter.Init(
                player: playerTarget,
                approachSpeed: moveSpeed,
                attackSpeedFactorMin: attackSpeedFactorMin,
                attackSpeedFactorMax: attackSpeedFactorMax,
                shootingDistance: shootingDistance,
                projectilePrefab: projectilePrefab,
                fireInterval: fireInterval,
                projectileSpeed: projectileSpeed,
                projectileDamage: projectileDamage,
                turnSpeed: turnSpeed,
                speedChange: speedChange
            );
        }
    }
}
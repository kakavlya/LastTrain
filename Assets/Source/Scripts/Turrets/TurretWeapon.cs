using LastTrain.Projectiles;
using LastTrain.Projectiles.Types;
using UnityEngine;

namespace LastTrain.Turrets
{
    /// <summary>
    /// Handles the firing mechanics for a single turret barrel.
    /// No player input, no AimingTargetProvider, no Ammunition.
    /// Attach to the rotating part of the turret prefab (the child that faces the target).
    /// </summary>
    public class TurretWeapon : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Projectile _projectilePrefab;

        [Header("Audio")]
        [SerializeField] private AudioClip _shootClip;
        [Tooltip("Volume multiplier relative to the global effects volume. " +
                 "0.4 makes the turret noticeably quieter than the player weapon.")]
        [SerializeField] [Range(0f, 1f)] private float _volumeMultiplier = 0.4f;

        [Header("Defaults (overridden by Init)")]
        [SerializeField] private float _projectileSpeed = 60f;

        private float _damage;
        private float _range;
        private float _fireDelay;
        private float _lastFireTime = float.NegativeInfinity;

        private AudioSource _audioSource;

        /// <summary>
        /// The effective spawn origin for projectiles.
        /// Returns _firePoint if assigned and sane, otherwise falls back to this transform.
        /// </summary>
        public Transform FirePoint => GetSaneFirePoint();

        // ── Lifecycle ─────────────────────────────────────────────────────────

        private void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = 1f;
            _audioSource.rolloffMode  = AudioRolloffMode.Linear;
            _audioSource.minDistance  = 5f;
            _audioSource.maxDistance  = 40f;
            ApplyVolume();

            // Diagnostic: log fire point positions so misconfigured prefabs are caught immediately.
            if (_firePoint != null)
            {
                float dist = Vector3.Distance(transform.position, _firePoint.position);
                Debug.Log(
                    $"[TurretWeapon] {name} — FirePoint diagnostic:\n" +
                    $"  RotatingPart world pos : {transform.position:F1}\n" +
                    $"  FirePoint world pos     : {_firePoint.position:F1}\n" +
                    $"  FirePoint local pos     : {_firePoint.localPosition:F1}\n" +
                    $"  Distance from weapon    : {dist:F2} u" +
                    (dist > 5f ? "  ← SUSPICIOUS (>5 u). Will fall back to RotatingPart position." : ""));
            }
            else
            {
                Debug.Log($"[TurretWeapon] {name} — no FirePoint assigned. Will fire from RotatingPart position.");
            }
        }

        // ── Public API ───────────────────────────────────────────────────────

        /// <summary>
        /// Called by TurretController after instantiation.
        /// Replaces inspector defaults with config-driven values.
        /// </summary>
        public void Init(float damage, float range, float fireDelay, float projectileSpeed = -1f)
        {
            _damage    = damage;
            _range     = range;
            _fireDelay = fireDelay;

            if (projectileSpeed > 0f)
                _projectileSpeed = projectileSpeed;

            ApplyVolume();
        }

        /// <summary>Returns true when the fire-rate cooldown has elapsed.</summary>
        public bool CanFire() => Time.time - _lastFireTime >= _fireDelay;

        /// <summary>
        /// Fires a pooled projectile in the given world-space direction and plays the shoot sound.
        /// Caller is responsible for checking CanFire() first.
        /// </summary>
        public void Fire(Vector3 direction)
        {
            if (_projectilePrefab == null)
            {
                Debug.LogWarning($"[TurretWeapon] {name} — _projectilePrefab is NULL. Assign it in the Inspector.");
                return;
            }

            _lastFireTime = Time.time;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            Vector3 spawnPos = GetSaneFirePoint().position;

            if (ProjectilePool.Instance != null)
            {
                Debug.Log($"[TurretWeapon] {name} — projectile spawned at {spawnPos:F1}.");
                ProjectilePool.Instance.Spawn(
                    _projectilePrefab, spawnPos, rotation,
                    gameObject, _projectileSpeed, _damage, _range);
            }
            else
            {
                Debug.LogWarning($"[TurretWeapon] {name} — ProjectilePool.Instance is NULL. Using Instantiate fallback.");
                Instantiate(_projectilePrefab, spawnPos, rotation);
            }

            PlayShootSound();
        }

        // ── Audio ─────────────────────────────────────────────────────────────

        private void PlayShootSound()
        {
            if (_audioSource == null || _shootClip == null)
                return;

            _audioSource.PlayOneShot(_shootClip);
        }

        /// <summary>
        /// Returns _firePoint if it is assigned and within a reasonable distance of this
        /// weapon's own transform. If it is suspiciously far (bad hierarchy / scale issue),
        /// falls back to this transform so projectiles spawn at the turret pivot instead.
        /// </summary>
        private Transform GetSaneFirePoint()
        {
            const float maxSaneDistance = 5f;

            if (_firePoint == null)
                return transform;

            if (Vector3.Distance(transform.position, _firePoint.position) > maxSaneDistance)
                return transform;

            return _firePoint;
        }

        /// <summary>
        /// Re-applies volume so it stays proportional to the player's effects volume setting.
        /// Called from Init and Awake; can also be called externally if settings change.
        /// </summary>
        public void ApplyVolume()
        {
            if (_audioSource == null)
                return;

            // Base on the global effects volume so the turret respects the player's settings.
            float baseVolume = Audio.AudioManager.Instance != null
                ? Audio.AudioManager.Instance.EffectsAudioVolume
                : 1f;

            _audioSource.volume = baseVolume * _volumeMultiplier;
        }
    }
}

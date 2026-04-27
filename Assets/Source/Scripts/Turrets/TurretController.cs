using UnityEngine;

namespace LastTrain.Turrets
{
    /// <summary>
    /// Brain of a single turret mount. Reads the nearest target from TurretTargetSelector,
    /// rotates the barrel toward it with axis constraints, and fires via TurretWeapon
    /// when the aim is close enough.
    ///
    /// Rotation constraints (all in local space relative to the parent hardpoint):
    ///   Z axis — free 360° rotation (horizontal/azimuth)
    ///   X axis — clamped [minPitchDeg, maxPitchDeg], default 0..15° (elevation)
    ///   Y axis — locked at 0 (no roll)
    ///
    /// Prefab structure expected:
    ///   TurretRoot         ← TurretController + TurretTargetSelector
    ///     └─ RotatingPart  ← TurretWeapon  (assign to _rotatingPart + _weapon)
    ///          ├─ Model    ← mesh, scale/rotation offsets live here
    ///          └─ FirePoint ← empty Transform at barrel tip
    /// </summary>
    public class TurretController : MonoBehaviour
    {
        [Header("Sub-components")]
        [SerializeField] private TurretTargetSelector _targetSelector;
        [SerializeField] private TurretWeapon _weapon;

        [Tooltip("The child Transform that physically rotates to face the enemy. " +
                 "Usually the barrel/body mesh parent. Defaults to this transform if null.")]
        [SerializeField] private Transform _rotatingPart;

        [Header("Rotation speed")]
        [SerializeField] private float _rotationSpeed = 120f;   // deg/s

        [Header("Axis constraints (local space, relative to hardpoint parent)")]
        [Tooltip("Minimum elevation angle on the X axis. 0 = horizontal, negative = pointing down.")]
        [SerializeField] private float _minPitchDeg = 0f;

        [Tooltip("Maximum elevation angle on the X axis. 15 = slightly upward.")]
        [SerializeField] private float _maxPitchDeg = 15f;

        [Header("Fire constraint")]
        [Tooltip("Turret fires only when the azimuth (Z axis) difference to the target is within this angle. " +
                 "X (pitch) is ignored — turret can be slightly mis-elevated and still fire. " +
                 "15° is a good default: tight enough to look deliberate, loose enough for a moving train.")]
        [SerializeField] private float _maxAzimuthFireDeg = 15f;

        [Header("Debug")]
        [Tooltip("Enable console logs for every step of the fire chain. Disable when no longer needed.")]
        [SerializeField] private bool _debugLog = true;

        private bool _active;
        private bool _paused;

        // ── Lifecycle ─────────────────────────────────────────────────────────

        private void Awake()
        {
            if (_rotatingPart == null)
                _rotatingPart = transform;
        }

        private void Update()
        {
            if (!_active || _paused)
            {
                if (_debugLog && !_active)
                    Debug.Log($"[Turret] {name} — not active yet (Begin() not called).");
                return;
            }

            Transform target = _targetSelector.CurrentTarget;

            if (target == null)
            {
                if (_debugLog && Time.frameCount % 60 == 0)   // throttle to ~1/sec
                    Debug.Log($"[Turret] {name} — no target in scan radius ({_targetSelector.ScanRadius} u).");
                return;
            }

            RotateConstrained(target.position);
            TryFire(target.position);
        }

        // ── Public API ───────────────────────────────────────────────────────

        public void Init(float damage, float range, float fireDelay, float projectileSpeed = -1f)
        {
            _weapon.Init(damage, range, fireDelay, projectileSpeed);
        }

        public void Begin()
        {
            _active = true;
            if (_debugLog)
                Debug.Log($"[Turret] {name} — Begin() called. Turret is now active.");
        }

        public void Pause()  => _paused = true;
        public void Resume() => _paused = false;

        // ── Constrained rotation ──────────────────────────────────────────────

        private void RotateConstrained(Vector3 targetWorldPos)
        {
            // Work in the parent's local space so the train's own rotation is
            // factored out. If there is no parent, fall back to world space.
            Transform parent = _rotatingPart.parent;

            Vector3 localDir = parent != null
                ? parent.InverseTransformDirection(targetWorldPos - _rotatingPart.position)
                : (targetWorldPos - _rotatingPart.position);

            if (localDir.sqrMagnitude < 1e-6f)
                return;

            localDir.Normalize();

            // ── Z axis: free azimuth rotation ────────────────────────────────
            // atan2(x, y) gives the rotation around Z from the +Y axis toward +X.
            float azimuthZ = Mathf.Atan2(localDir.x, localDir.y) * Mathf.Rad2Deg;

            // ── X axis: clamped elevation ─────────────────────────────────────
            // asin of the Z component gives the angle above/below the local XY plane.
            float rawPitch = Mathf.Asin(Mathf.Clamp(localDir.z, -1f, 1f)) * Mathf.Rad2Deg;
            float pitchX   = Mathf.Clamp(rawPitch, _minPitchDeg, _maxPitchDeg);

            // ── Y axis: locked at 0 ───────────────────────────────────────────
            Quaternion targetLocal = Quaternion.Euler(pitchX, 0f, azimuthZ);

            _rotatingPart.localRotation = Quaternion.RotateTowards(
                _rotatingPart.localRotation,
                targetLocal,
                _rotationSpeed * Time.deltaTime);
        }

        // ── Fire ─────────────────────────────────────────────────────────────

        private void TryFire(Vector3 targetWorldPos)
        {
            if (!_weapon.CanFire())
            {
                if (_debugLog && Time.frameCount % 30 == 0)
                    Debug.Log($"[Turret] {name} — cooldown active, waiting.");
                return;
            }

            // ── Azimuth-only alignment check ─────────────────────────────────
            // Compute the desired azimuth (Z rotation) toward the target,
            // then compare it to the rotating part's current local Z angle.
            // Only the horizontal rotation matters — pitch (X) is ignored.
            Transform parent = _rotatingPart.parent;
            Vector3 localDir = parent != null
                ? parent.InverseTransformDirection(targetWorldPos - _rotatingPart.position)
                : (targetWorldPos - _rotatingPart.position);

            float targetAzimuth  = Mathf.Atan2(localDir.x, localDir.y) * Mathf.Rad2Deg;
            float currentAzimuth = _rotatingPart.localEulerAngles.z;

            // DeltaAngle gives the shortest signed arc [-180, 180], handling wraparound.
            float azimuthDelta = Mathf.Abs(Mathf.DeltaAngle(currentAzimuth, targetAzimuth));

            if (azimuthDelta > _maxAzimuthFireDeg)
            {
                if (_debugLog && Time.frameCount % 15 == 0)
                    Debug.Log($"[Turret] {name} — azimuth delta {azimuthDelta:F1}° > {_maxAzimuthFireDeg}°. Still rotating.");
                return;
            }

            // Aim the projectile straight at the target (not limited to barrel forward).
            Vector3 dir = (targetWorldPos - _weapon.FirePoint.position).normalized;

            if (_debugLog)
                Debug.Log($"[Turret] {name} — FIRED. Azimuth delta: {azimuthDelta:F1}°");

            _weapon.Fire(dir);
        }
    }
}

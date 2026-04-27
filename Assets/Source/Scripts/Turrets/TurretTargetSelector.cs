using LastTrain.Enemies;
using UnityEngine;

namespace LastTrain.Turrets
{
    /// <summary>
    /// Scans nearby colliders on a fixed timer and exposes the nearest live enemy transform.
    /// Uses OverlapSphere — never FindObjectsOfType. Attach to the turret root GameObject.
    ///
    /// Debug visualization:
    ///   Editor (Gizmos)  — always shown when the object is selected (orange wire sphere).
    ///   Play mode        — enable _showRangeInPlayMode to draw a circle via Debug.DrawLine.
    ///                      Requires the Game-view Gizmos button to be ON.
    /// </summary>
    public class TurretTargetSelector : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float _scanRadius = 35f;
        [SerializeField] private float _scanInterval = 0.2f;
        [SerializeField] private LayerMask _enemyMask;

        [Header("Debug (play mode)")]
        [Tooltip("Draw the scan-radius circle in the Game view during play (requires Gizmos ON in Game view).")]
        [SerializeField] private bool _showRangeInPlayMode = true;

        [Tooltip("Number of line segments used to draw the circle. More = smoother but more overhead.")]
        [SerializeField] [Range(16, 64)] private int _debugSegments = 32;

        [Tooltip("Circle is drawn in XZ plane offset upward by this amount so it sits above the ground.")]
        [SerializeField] private float _debugHeightOffset = 0.1f;

        private Transform _currentTarget;
        private float _scanTimer;

        /// <summary>
        /// The nearest live enemy found during the last scan, or null if none in range.
        /// </summary>
        public Transform CurrentTarget => _currentTarget;

        /// <summary>Expose radius so TurretController can verify projectile range >= scan radius.</summary>
        public float ScanRadius => _scanRadius;

        // ── Unity ────────────────────────────────────────────────────────────

        private void Update()
        {
            _scanTimer -= Time.deltaTime;

            if (_scanTimer <= 0f)
            {
                _scanTimer = _scanInterval;
                ScanForTarget();
            }

#if UNITY_EDITOR
            if (_showRangeInPlayMode && Application.isPlaying)
            {
                DrawPlayModeCircle(_scanRadius, new Color(1f, 0.5f, 0.1f));

                if (_currentTarget != null)
                    DrawPlayModeCircle(
                        (_currentTarget.position - transform.position).magnitude,
                        new Color(1f, 1f, 0.1f, 0.6f));
            }
#endif
        }

        // ── Scan ─────────────────────────────────────────────────────────────

        private void ScanForTarget()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _scanRadius, _enemyMask);

            float bestSqrDist = float.MaxValue;
            _currentTarget = null;

            foreach (Collider hit in hits)
            {
                var health = hit.GetComponent<EnemyHealth>();

                if (health == null || health.IsDead)
                    continue;

                float sqrDist = (hit.transform.position - transform.position).sqrMagnitude;

                if (sqrDist < bestSqrDist)
                {
                    bestSqrDist = sqrDist;
                    _currentTarget = hit.transform;
                }
            }
        }

        // ── Debug drawing ─────────────────────────────────────────────────────

#if UNITY_EDITOR
        /// <summary>
        /// Draws an XZ-plane circle using Debug.DrawLine — visible in Game view when
        /// the Gizmos toggle is enabled.
        /// </summary>
        private void DrawPlayModeCircle(float radius, Color color)
        {
            if (radius <= 0f)
                return;

            Vector3 center = transform.position + Vector3.up * _debugHeightOffset;
            float angleStep = 360f / _debugSegments;

            for (int i = 0; i < _debugSegments; i++)
            {
                float a1 = i * angleStep * Mathf.Deg2Rad;
                float a2 = (i + 1) * angleStep * Mathf.Deg2Rad;

                Vector3 p1 = center + new Vector3(Mathf.Cos(a1) * radius, 0f, Mathf.Sin(a1) * radius);
                Vector3 p2 = center + new Vector3(Mathf.Cos(a2) * radius, 0f, Mathf.Sin(a2) * radius);

                Debug.DrawLine(p1, p2, color);
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Editor-only static preview (no play required).
            Gizmos.color = new Color(1f, 0.5f, 0.1f, 0.4f);
            Gizmos.DrawWireSphere(transform.position, _scanRadius);
        }
#endif
    }
}

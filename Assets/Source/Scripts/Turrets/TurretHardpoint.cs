using UnityEngine;

namespace LastTrain.Turrets
{
    /// <summary>
    /// Marker component placed on child Transforms of the train prefab.
    /// Defines where a turret can be mounted and which slot index it corresponds to.
    ///
    /// Phase 0: data only — no runtime logic.
    /// Phase 1: TurretsHandler will read these to spawn turret prefabs.
    /// </summary>
    public class TurretHardpoint : MonoBehaviour
    {
        [Tooltip("Index into TurretsProgress list. Must be unique per train prefab.")]
        [SerializeField] private int _slotIndex;

        [Tooltip("Optional: a child Transform used as the visual attach point / pivot for the turret. " +
                 "Leave null to use this GameObject's Transform.")]
        [SerializeField] private Transform _attachPoint;

        /// <summary>Index into SavesYG.TurretsProgress. Set once per hardpoint in the prefab.</summary>
        public int SlotIndex => _slotIndex;

        /// <summary>World-space attach point for the spawned turret. Falls back to this transform.</summary>
        public Transform AttachPoint => _attachPoint != null ? _attachPoint : transform;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.6f, 0f, 0.8f);
            Gizmos.DrawWireSphere(AttachPoint.position, 0.5f);
            UnityEditor.Handles.Label(
                AttachPoint.position + Vector3.up * 0.7f,
                $"Hardpoint [{_slotIndex}]");
        }
#endif
    }
}

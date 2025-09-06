using UnityEngine;

namespace LastTrain.Weapons.System
{
    public class AimingTargetProvider : MonoBehaviour
    {
        [Header("Planar aiming (fixed world Y)")]
        [Tooltip("Мировая высота плоскости прицеливания (Y). Задай один раз в сцене.")]
        [SerializeField] private float _planeHeightY = 15f;
        [SerializeField] private LayerMask _hitMask = ~0;

        private Camera _cam;
        private const float FallbackDist = 1000f;

        public Vector3 AimPointPlanar { get; private set; }
        public Ray AimRay { get; private set; }
        public bool HasPoint { get; private set; }

        private void Update()
        {
            if (_cam == null) return;

            AimRay = _cam.ScreenPointToRay(Input.mousePosition);

            var plane = new Plane(Vector3.up, new Vector3(0f, _planeHeightY, 0f));

            if (plane.Raycast(AimRay, out float t))
            {
                AimPointPlanar = AimRay.GetPoint(t);
                HasPoint = true;
            }
            else
            {
                AimPointPlanar = AimRay.GetPoint(FallbackDist);
                HasPoint = false;
            }
        }

        //public Vector3 GetTargetPoint(float fallback = FallbackDist)
        //    => HasPoint ? AimPointPlanar : AimRay.GetPoint(fallback);
        public Vector3 GetTargetPoint(float fallback = FallbackDist)
            => HasPoint? AimPointPlanar : AimRay.GetPoint(fallback);

        public void Init()
        {
            _cam = Camera.main;
        }

        public bool TryGetWorldTarget(out Vector3 worldPoint, float maxDistance = 5000f)
        {
            var ray = (_cam != null) ? AimRay : Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, maxDistance, _hitMask))
            {
                worldPoint = hit.point;
                return true;
            }
            
            worldPoint = ray.GetPoint(FallbackDist);

            return false;
        }

public void SetPlaneHeight(float worldY)
        {
            _planeHeightY = worldY;
        }
    }
}

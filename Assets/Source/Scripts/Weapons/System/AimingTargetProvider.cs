using UnityEngine;

namespace LastTrain.Weapons.System
{
    public class AimingTargetProvider : MonoBehaviour
    {
        [Header("Aim-from-camera")]
        [SerializeField] private Camera _cam;       
        [SerializeField] private LayerMask _aimMask = ~0;
        [SerializeField] private float _maxDistance = 5000f;
        
        public Vector2 ScreenPoint { get; set; }

        public void Init(Camera cam = null)
        {
            _cam = cam ? cam : Camera.main;
        }
    
        public AimData GetAim()
        {
            if (_cam == null)
                _cam = Camera.main;

            Vector2 sp = ScreenPoint == Vector2.zero ? (Vector2)Input.mousePosition : ScreenPoint;
            Ray ray = _cam.ScreenPointToRay(sp);

            if (Physics.Raycast(ray, out var hit, _maxDistance, _aimMask, QueryTriggerInteraction.Ignore))
                return new AimData { CamRay = ray, HasHit = true, WorldPoint = hit.point };

            return new AimData { CamRay = ray, HasHit = false, WorldPoint = ray.GetPoint(_maxDistance) };
        }
    }
}

using UnityEngine;

namespace LastTrain.Weapons.System
{
    public struct AimData
    {
        [SerializeField] private Ray _camRay;
        [SerializeField] private bool _hasHit;
        [SerializeField] private Vector3 _worldPoint;

        public AimData(Ray ray, bool hasHit, Vector3 point)
        {
            _camRay = ray;
            _hasHit = hasHit;
            _worldPoint = point;
        }

        public Ray CamRay => _camRay;

        public bool HasHit => _hasHit;

        public Vector3 WorldPoint => _worldPoint;
    }
}

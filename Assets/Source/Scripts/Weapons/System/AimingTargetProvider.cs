using System.Collections;
using UnityEngine;

namespace Assets.Source.Scripts.Weapons
{
    public class AimingTargetProvider : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundMask;

        private Camera _mainCamera;
        private float _maxDistance = 90000f;
        public Vector3 AimDirection { get; private set; } = Vector3.forward;
        public Vector3? AimPointWorld { get; private set; }

        public void Init()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _groundMask))
            {
                AimPointWorld = hit.point;
                Vector3 flatDir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
                AimDirection = flatDir.normalized;
            }
            else
            {
                AimPointWorld = null;
            }
        }
    }
}
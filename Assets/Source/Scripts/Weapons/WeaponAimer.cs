using UnityEngine;

public class WeaponAimer : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform _weaponPivot;
    [SerializeField] private LayerMask _groundMask;

    [Header("Aiming Settings")]
    [SerializeField] private float _rotationSpeed = 360f; // degrees per second

    private void Awake()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;
    }

    private void Update()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _groundMask))
        {
            Vector3 targetPos = hit.point;
            Vector3 direction = targetPos - _weaponPivot.position;

            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                _weaponPivot.rotation = Quaternion.RotateTowards(
                    _weaponPivot.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}

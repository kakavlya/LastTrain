using Assets.Source.Scripts.Weapons;
using UnityEngine;

public class WeaponTrajectory : MonoBehaviour
{
    [SerializeField] private WeaponRotator _weaponRotator;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _length;
    [SerializeField] private float _width;

    private int _pointCount = 2;

    private void Start()
    {
        _lineRenderer.startWidth = _width;
        _lineRenderer.endWidth = 0;
        _lineRenderer.positionCount = _pointCount;
        _weaponRotator.Rotated += UpdateTrajectory;
    }

    private void UpdateTrajectory(Vector3 direction)
    {
        if (direction.magnitude > 0.1)
        {
            Vector3 endPoint = _firePoint.position + direction * _length;

            _lineRenderer.SetPosition(0, _firePoint.position);
            _lineRenderer.SetPosition(1, endPoint);
            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
}

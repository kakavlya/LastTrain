using SplineMesh;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Spline _spline;

    private Transform _target;
    private float _distance;

    private void Update()
    {
        if (_spline == null) return;

        _distance += _speed * Time.deltaTime;
        CurveSample sample = _spline.GetSampleAtDistance(_distance);
        transform.position = sample.location;
        transform.rotation = sample.Rotation * Quaternion.Euler(0, 180, 0);
    }
}

using SplineMesh;
using UnityEngine;

namespace Player
{
    public class TrainMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private LevelDesigner _levelDesigner;

        private Spline _spline;
        private float _distance;
        private Transform _splineTransform;

        private void OnEnable()
        {
            _levelDesigner.StartedElementDefined += SetCurrentSpline;
        }

        private void Update()
        {
            if (_spline == null || _distance > _spline.Length) return;

            _distance += _speed * Time.deltaTime;
            CurveSample sample = _spline.GetSampleAtDistance(_distance);
            Vector3 globalPosition = _splineTransform.TransformPoint(sample.location);
            transform.position = new Vector3(globalPosition.x, transform.position.y, globalPosition.z);
            transform.rotation = sample.Rotation * Quaternion.Euler(0, 180, 0);
        }

        private void SetCurrentSpline(LevelElement levelElement)
        {
            _distance = 0f;
            _spline = levelElement.GetComponentInChildren<Spline>();
            _splineTransform = _spline.transform;
        }
    }
}
using System;
using SplineMesh;
using UnityEngine;
using Level;

namespace Player
{
    public class TrainMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private LevelGenerator _levelGenerator;

        private LevelElement _currentLevelElement;
        private Spline _spline;
        private float _distance;
        private Transform _splineTransform;

        public event Action<LevelElement> SplineIsOvered;

        private void OnEnable()
        {
            _levelGenerator.StartedElementDefined += SetCurrentSpline;
            _levelGenerator.ElementChanged += SetCurrentSpline;
        }

        private void OnDisable()
        {
            _levelGenerator.StartedElementDefined -= SetCurrentSpline;
        }

        private void Update()
        {
            _distance += _speed * Time.deltaTime;

            if (_spline == null || _distance > _spline.Length)
            {
                SplineIsOvered?.Invoke(_currentLevelElement);
                return;
            }

            CurveSample sample = _spline.GetSampleAtDistance(_distance);
            Vector3 globalPosition = _splineTransform.TransformPoint(sample.location);
            transform.position = new Vector3(globalPosition.x, transform.position.y, globalPosition.z);
            transform.rotation = sample.Rotation * Quaternion.Euler(0, 180, 0);
        }

        public float Speed()
        {
            return _speed;
        }

        private void SetCurrentSpline(LevelElement levelElement)
        {
            _distance = 0f;
            _currentLevelElement = levelElement;
            _spline = levelElement.GetComponentInChildren<Spline>();
            _splineTransform = _spline.transform;
        }
    }
}
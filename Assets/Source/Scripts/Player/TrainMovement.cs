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
        private bool _running;

        public event Action<LevelElement> SplineIsOvered;

        public void Init()
        {
            _levelGenerator.StartedElementDefined += SetCurrentSpline;
            _levelGenerator.ElementChanged += SetCurrentSpline;
        }

        private void OnDisable()
        {
            _levelGenerator.StartedElementDefined -= SetCurrentSpline;
            _levelGenerator.ElementChanged -= SetCurrentSpline;
        }

        private void Update()
        {
            if (!_running)
                return;

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

        public void StartMovement() => _running = true;

        public void StopMovement() => _running = false;

        public float Speed()
        {
            return _speed;
        }

        private void SetCurrentSpline(LevelElement levelElement)
        {
            _currentLevelElement = levelElement;
            _spline = levelElement.GetComponentInChildren<Spline>();
            _splineTransform = _spline.transform;

            Vector3 localPosition = _splineTransform.InverseTransformPoint(transform.position);
            float nearestDistance = FindNearestDistanceOnSpline(_spline, localPosition);
            _distance = nearestDistance;
        }

        private float FindNearestDistanceOnSpline(Spline spline, Vector3 targetLocalPosition)
        {
            float step = 0.1f;
            float bestDistance = 0f;
            float minSqrDistance = float.MaxValue;

            for (float d = 0f; d < spline.Length; d += step)
            {
                Vector3 pos = spline.GetSampleAtDistance(d).location;
                float sqrDist = (pos - targetLocalPosition).sqrMagnitude;

                if (sqrDist < minSqrDistance)
                {
                    minSqrDistance = sqrDist;
                    bestDistance = d;
                }
            }

            return bestDistance;
        }
    }
}
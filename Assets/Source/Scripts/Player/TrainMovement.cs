using System;
using SplineMesh;
using UnityEngine;
using Level;
using Unity.VisualScripting;

namespace Player
{
    public class TrainMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private float _transitionDuration = 2f;

        private LevelElement _currentLevelElement;
        private Spline _currentSpline;
        private Spline _nextSpline;
        private float _distance;
        private Transform _currentSplineTransform;
        private Transform _nextSplineTransform;
        private bool _isRunning;
        private bool _isTransition;
        private bool _isTrainStart = true;
        private float _transitionProgress;

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
            if (!_isRunning)
                return;

            if (_isTransition)
            {
                _transitionProgress += Time.deltaTime * _speed;

                var currentEndSample = _currentSpline.GetSampleAtDistance(_currentSpline.Length - 0.01f);
                var nextStartSample = _nextSpline.GetSampleAtDistance(0);

                Vector3 startPoint = _currentSplineTransform.TransformPoint(currentEndSample.location);
                Vector3 endPoint = _nextSplineTransform.TransformPoint(nextStartSample.location);

                transform.position = Vector3.Lerp(startPoint, endPoint, _transitionProgress);
                transform.rotation = Quaternion.Slerp(
                    currentEndSample.Rotation * Quaternion.Euler(0, 180, 0),
                    nextStartSample.Rotation * Quaternion.Euler(0, 180, 0),
                    _transitionProgress
                );

                if (_transitionProgress >= 1f)
                {
                    _isTransition = false;

                    _currentSpline = _nextSpline;
                    _currentSplineTransform = _nextSplineTransform;
                    _distance = 0;

                    CurveSample startSample = _currentSpline.GetSampleAtDistance(0);
                    Vector3 startPos = _currentSplineTransform.TransformPoint(startSample.location);
                    transform.position = startPos;
                    transform.rotation = startSample.Rotation * Quaternion.Euler(0, 180, 0);
                }

                return;
            }

            _distance += _speed * Time.deltaTime;

            if (_distance >= _currentSpline.Length)
            {
                if (_nextSpline != null)
                {
                    _isTransition = true;
                    _transitionProgress = 0f;
                    return;
                }

                SplineIsOvered?.Invoke(_currentLevelElement);
                return;
            }

            CurveSample sample = _currentSpline.GetSampleAtDistance(_distance);
            Vector3 globalPosition = _currentSplineTransform.TransformPoint(sample.location);
            transform.position = globalPosition;
            transform.rotation = sample.Rotation * Quaternion.Euler(0, 180, 0);
        }

        public void StartMovement() => _isRunning = true;

        public void StopMovement() => _isRunning = false;

        public float Speed()
        {
            return _speed;
        }

        private void SetCurrentSpline(LevelElement currentLevelElement, LevelElement nextLevelElement)
        {
            _currentLevelElement = currentLevelElement;
            _currentSpline = currentLevelElement.GetComponentInChildren<Spline>();
            _currentSplineTransform = _currentSpline.transform;
            _nextSpline = nextLevelElement.GetComponentInChildren<Spline>();
            _nextSplineTransform = _nextSpline.transform;

            if (_isTrainStart)
            {
                CurveSample startSample = _currentSpline.GetSampleAtDistance(0);
                Vector3 startPos = _currentSplineTransform.TransformPoint(startSample.location);
                transform.position = startPos;
                transform.rotation = startSample.Rotation * Quaternion.Euler(0, 180, 0);
                _isTrainStart = false;
            }

            _distance = 0;
        }
    }
}
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

        private float _endOffset = 0.01f;
        private Quaternion _rottationCorrection = Quaternion.Euler(0, 180, 0);
        private LevelElement _currentLevelElement;
        private Spline _currentSpline;
        private Spline _nextSpline;
        private float _distance;
        private Transform _currentSplineTransform;
        private Transform _nextSplineTransform;
        private bool _isRunning;
        private bool _isTransitionAvailable;
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

            if (_isTransitionAvailable)
            {
                Transition();
                return;
            }

            _distance += _speed * Time.deltaTime;

            if (_distance >= _currentSpline.Length)
            {
                if (_nextSpline != null)
                {
                    _isTransitionAvailable = true;
                    _transitionProgress = 0f;
                    return;
                }

                SplineIsOvered?.Invoke(_currentLevelElement);
                return;
            }

            CurveSample sample = _currentSpline.GetSampleAtDistance(_distance);
            Vector3 globalPosition = _currentSplineTransform.TransformPoint(sample.location);
            transform.position = globalPosition;
            transform.rotation = sample.Rotation * _rottationCorrection;
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
                transform.rotation = startSample.Rotation * _rottationCorrection;
                _isTrainStart = false;
            }

            _distance = 0;
        }

        private void Transition()
        {
            _transitionProgress += Time.deltaTime * _speed;

            var currentEndSample = _currentSpline.GetSampleAtDistance(_currentSpline.Length - _endOffset);
            var nextStartSample = _nextSpline.GetSampleAtDistance(0);

            Vector3 startPoint = _currentSplineTransform.TransformPoint(currentEndSample.location);
            Vector3 endPoint = _nextSplineTransform.TransformPoint(nextStartSample.location);

            transform.position = Vector3.Lerp(startPoint, endPoint, _transitionProgress);
            transform.rotation = Quaternion.Slerp(
                currentEndSample.Rotation * _rottationCorrection,
                nextStartSample.Rotation * _rottationCorrection,
                _transitionProgress
            );

            if (_transitionProgress >= 1f)
            {
                _isTransitionAvailable = false;

                _currentSpline = _nextSpline;
                _currentSplineTransform = _nextSplineTransform;
                _distance = 0;

                CurveSample startSample = _currentSpline.GetSampleAtDistance(0);
                Vector3 startPos = _currentSplineTransform.TransformPoint(startSample.location);
                transform.position = startPos;
                transform.rotation = startSample.Rotation * _rottationCorrection;
                SplineIsOvered?.Invoke(_currentLevelElement);
            }
        }
    }
}

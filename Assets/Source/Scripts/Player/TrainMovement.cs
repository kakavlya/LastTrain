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
        private Spline _currentSpline;
        private Spline _nextSpline;
        private float _distance;
        private Transform _currentSplineTransform;
        private Transform _nextSplineTransform;
        private bool _isRunning;
        private bool _isTransition;
        private bool _isTrainStart = true;
        private float _transitionProgress;
        private Vector3 _currentPosition;
        private bool _firstPosition = true;
        private bool _justFinishedTransition;

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
                _transitionProgress += _speed * Time.deltaTime;

                CurveSample currentSample = _currentSpline.GetSampleAtDistance(_distance);
                CurveSample nextSample = _nextSpline.GetSampleAtDistance(0);

                if (_firstPosition)
                {
                    _currentPosition = _currentSplineTransform.TransformPoint(currentSample.location);
                    _firstPosition = false;
                }

                Vector3 nextPosition = _nextSplineTransform.TransformPoint(nextSample.location);

                transform.position = Vector3.MoveTowards(_currentPosition, nextPosition, _speed * Time.deltaTime);
                _currentPosition = transform.position;

                transform.rotation = nextSample.Rotation * Quaternion.Euler(0, 180, 0);

                if (_transitionProgress >= 1f)
                {
                    _isTransition = false;
                    _currentSpline = _nextSpline;
                    _currentSplineTransform = _nextSplineTransform;
                    _distance = 0;
                }

                return;
            }

            if (_justFinishedTransition)
            {
                _justFinishedTransition = false;
                _distance += _speed * Time.deltaTime; 
            }
            else
            {
                _distance += _speed * Time.deltaTime;

                if (_distance >= _currentSpline.Length)
                {
                    SplineIsOvered?.Invoke(_currentLevelElement);
                    return;
                }
            }

            CurveSample sample = _currentSpline.GetSampleAtDistance(_distance);
            Vector3 globalPosition = _currentSplineTransform.TransformPoint(sample.location);
            transform.position = new Vector3(globalPosition.x, transform.position.y, globalPosition.z);
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
            _distance = 0;
            _currentLevelElement = currentLevelElement;
            _currentSpline = currentLevelElement.GetComponentInChildren<Spline>();
            _currentSplineTransform = _currentSpline.transform;
            _nextSpline = nextLevelElement.GetComponentInChildren<Spline>();
            _nextSplineTransform = _nextSpline.transform;

            if (!_isTrainStart)
            {
                _isTransition = true;
                _transitionProgress = 0;
                _firstPosition = true;
            }
            else
            {
                CurveSample startSample = _currentSpline.GetSampleAtDistance(0);
                Vector3 startPos = _currentSplineTransform.TransformPoint(startSample.location);
                transform.position = new Vector3(startPos.x, transform.position.y, startPos.z);
                transform.rotation = startSample.Rotation * Quaternion.Euler(0, 180, 0);
                _isTrainStart = false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Level;
using Player;
using UnityEngine;

namespace Level
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private LevelElementsCreator _creator;
        [SerializeField] private Vector3 _startElementPosition;
        [SerializeField] private TrainMovement _trainMovement;

        private List<LevelElement> _elementsOnScene = new List<LevelElement>();
        private LevelElement _currentElement;
        private int _maxCount = 5;
        private Vector3 _workingPosition;

        public Vector3 StartTrainPosition { get; private set; }

        public event Action<LevelElement> StartedElementDefined;
        public event Action<LevelElement> ElementChanged;

        private void OnEnable()
        {
            _trainMovement.SplineIsOvered += ChangeCurrentElementAndExtendLevel;
        }

        private void OnDisable()
        {
            _trainMovement.SplineIsOvered -= ChangeCurrentElementAndExtendLevel;
        }

        private void Awake()
        {
            _workingPosition = _startElementPosition;

            InitStartedElements();
        }

        private void AddElementOnLevel()
        {
            LevelElement element = _creator.CreateElement(_workingPosition);
            _elementsOnScene.Add(element);
        }

        private void InitStartedElements()
        {
            for (int i = 0; i < _maxCount; ++i)
            {
                AddElementOnLevel();
                CalculateNextPosition();
            }

            _currentElement = _elementsOnScene[_elementsOnScene.Count / 2];
            StartedElementDefined?.Invoke(_currentElement);
            StartTrainPosition = _currentElement.transform.position;
        }

        private void CalculateNextPosition()
        {
            if (_elementsOnScene.Count == 0) return;

            LevelElement lastElement = _elementsOnScene[_elementsOnScene.Count - 1];
            float elementWidth = lastElement.GetComponent<MeshRenderer>().bounds.size.x;
            _workingPosition.x = lastElement.transform.position.x + elementWidth;
        }

        private void ChangeCurrentElementAndExtendLevel(LevelElement element)
        {
            int index = _elementsOnScene.IndexOf(element);

            if (_elementsOnScene.Count > index + 1)
            {
                _currentElement = _elementsOnScene[index + 1];
                ElementChanged?.Invoke(_currentElement);
                AddElementOnLevel();
                CalculateNextPosition();
                Destroy(_elementsOnScene[0].gameObject);
                _elementsOnScene.Remove(_elementsOnScene[0]);
            }
        }
    }
}

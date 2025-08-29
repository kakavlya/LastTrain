using System;
using System.Collections.Generic;
using UnityEngine;
using LastTrain.Player;

namespace LastTrain.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private LevelElementsCreator _creator;
        [SerializeField] private Vector3 _startElementPosition;
        [SerializeField] private TrainMovement _trainMovement;

        private List<LevelElement> _elementsOnScene = new List<LevelElement>();
        private LevelElement _currentElement;
        private LevelElement _nextElement;
        private int _maxCount = 5;
        private Vector3 _workingPosition;

        public event Action<LevelElement, LevelElement> StartedElementDefined;
        public event Action<LevelElement, LevelElement> ElementChanged;

        public Vector3 StartTrainPosition { get; private set; }

        private void OnDisable()
        {
            _trainMovement.SplineIsOvered -= ChangeCurrentElementAndExtendLevel;
        }

        public void Init()
        {
            _trainMovement.SplineIsOvered += ChangeCurrentElementAndExtendLevel;
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
            _nextElement = _elementsOnScene[_elementsOnScene.Count / 2 + 1];
            StartedElementDefined?.Invoke(_currentElement, _nextElement);
            StartTrainPosition = _currentElement.transform.position;
        }

        private void CalculateNextPosition()
        {
            if (_elementsOnScene.Count == 0) return;

            LevelElement lastElement = _elementsOnScene[_elementsOnScene.Count - 1];
            float elementWidth = lastElement.GetComponent<Terrain>().terrainData.size.x;
            _workingPosition.x = lastElement.transform.position.x + elementWidth;
        }

        private void ChangeCurrentElementAndExtendLevel(LevelElement element)
        {
            int index = _elementsOnScene.IndexOf(element);

            if (_elementsOnScene.Count > index + 2)
            {
                _currentElement = _elementsOnScene[index + 1];
                _nextElement = _elementsOnScene[index + 2];
                ElementChanged?.Invoke(_currentElement, _nextElement);
                AddElementOnLevel();
                CalculateNextPosition();
                Destroy(_elementsOnScene[0].gameObject);
                _elementsOnScene.Remove(_elementsOnScene[0]);
            }
        }
    }
}

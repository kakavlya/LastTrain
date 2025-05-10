using System;
using System.Collections.Generic;
using Level;
using UnityEngine;

public class LevelDesigner : MonoBehaviour
{
    [SerializeField] private LevelElementsGenerator _generator;
    [SerializeField] private Vector3 _startElementPosition;

    private List<LevelElement> _elementsOnScene = new List<LevelElement>();
    private LevelElement _currentElement;
    private int _maxCount = 5;
    private Vector3 _workingPosition;

    public Vector3 StartTrainPosition { get; private set; }

    public event Action<LevelElement> StartedElementDefined;

    private void Awake()
    {
        _workingPosition = _startElementPosition;

        InitStartedElements();
    }

    private void AddElementOnLevel()
    {
        LevelElement element = _generator.GenerateElement(_workingPosition);
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
        int currentIndex = 0;
        float width = 0;

        foreach (LevelElement element in _elementsOnScene)
        {
            if (element == _currentElement)
            {
                currentIndex = _elementsOnScene.IndexOf(element);
                return;
            }
        }

        for (; currentIndex < _elementsOnScene.Count; ++currentIndex)
        {
            width += _elementsOnScene[currentIndex].GetComponent<MeshRenderer>().bounds.size.x;
        }

        _workingPosition.x = width;
    }

    private void ChangeCurrentElement(LevelElement element)
    {
        _currentElement = element;
    }
}

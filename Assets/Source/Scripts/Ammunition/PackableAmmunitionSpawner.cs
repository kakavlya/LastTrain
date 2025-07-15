using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;

public class PackableAmmunitionSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelGenerator _levelGenerator;

    [SerializeField] private PickableAmmunition[] _pickableAmmunitionPrefabs;

    [Header("Settings")]
    [SerializeField] private int _generatePercent;

    private void OnEnable()
    {
        _levelGenerator.StartedElementDefined += SetStartedRandomAmmunition;
        _levelGenerator.ElementChanged += SetNextRandomAmmunition;
    }


    private void SetStartedRandomAmmunition(LevelElement currentElement, LevelElement nextElement)
    {
        var points = currentElement.PickableAmmunitionPoints;
        
    }

    private void SetNextRandomAmmunition(LevelElement currentElement, LevelElement nextElement)
    {

    }
}

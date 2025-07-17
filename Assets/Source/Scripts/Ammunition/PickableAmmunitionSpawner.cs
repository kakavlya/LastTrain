using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;

public class PickableAmmunitionSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private SharedData _sharedData;
    [SerializeField] private PickableAmmunition[] _pickableAmmunitionPrefabs;

    private int _generatePercent;
    private int _maxGeneratePercent = 100;

    public void Init()
    {
        _generatePercent = _sharedData.LevelSetting.AmmunitionGeneratePercent;
        _levelGenerator.StartedElementDefined += SetStartedRandomAmmunition;
        _levelGenerator.ElementChanged += SetNextRandomAmmunition;
    }


    private void SetStartedRandomAmmunition(LevelElement currentElement, LevelElement nextElement)
    {
        var points = currentElement.PickableAmmunitionPoints;

        foreach (var point in points)
        {
            if (Random.Range(0, _maxGeneratePercent + 1) <= _generatePercent)
            {
                int ammoNum = Random.Range(0, _pickableAmmunitionPrefabs.Length);
                PickableAmmunitionPool.Instance.Spawn(_pickableAmmunitionPrefabs[ammoNum], point.position);
            }
        }

        SetNextRandomAmmunition(currentElement, nextElement);
    }

    private void SetNextRandomAmmunition(LevelElement currentElement, LevelElement nextElement)
    {
        var points = nextElement.PickableAmmunitionPoints;

        foreach (var point in points)
        {
            if (Random.Range(0, _maxGeneratePercent + 1) <= _generatePercent)
            {
                int ammoNum = Random.Range(0, _pickableAmmunitionPrefabs.Length);
                PickableAmmunitionPool.Instance.Spawn(_pickableAmmunitionPrefabs[ammoNum], point.position);
            }
        }
    }
}

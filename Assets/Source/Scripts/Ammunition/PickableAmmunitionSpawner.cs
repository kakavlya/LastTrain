using System.Collections.Generic;
using Level;
using UnityEngine;

public class PickableAmmunitionSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private SharedData _sharedData;
    [SerializeField] private PickableAmmunition[] _pickableAmmunitionPrefabs;

    private List<PickableAmmunition> _selectedAmmunitionPrefabs = new List<PickableAmmunition>();
    private int _generatePercent;
    private int _maxGeneratePercent = 100;

    public void Init()
    {
        SelectAmmunition();
        _generatePercent = _sharedData.LevelSetting.AmmunitionGeneratePercent;
        _levelGenerator.StartedElementDefined += SetStartedRandomAmmunition;
        _levelGenerator.ElementChanged += SetNextRandomAmmunition;
    }

    private void SelectAmmunition()
    {
        var selectedWeapons = _sharedData.WeaponConfigs;

        for (int i = 0; i < _pickableAmmunitionPrefabs.Length; i++)
        {
            for (int j = 0; j < selectedWeapons.Count; j++)
            {
                if (selectedWeapons[j].WeaponPrefab == _pickableAmmunitionPrefabs[i].PrefabTypeOfWeapon)
                {
                    _selectedAmmunitionPrefabs.Add(_pickableAmmunitionPrefabs[i]);
                }
            }
        }
    }

    private void SetStartedRandomAmmunition(LevelElement currentElement, LevelElement nextElement)
    {
        var points = currentElement.PickableAmmunitionPoints;

        foreach (var point in points)
        {
            if (Random.Range(0, _maxGeneratePercent + 1) <= _generatePercent && _selectedAmmunitionPrefabs.Count > 0)
            {
                int ammoNum = Random.Range(0, _selectedAmmunitionPrefabs.Count);
                PickableAmmunitionPool.Instance.Spawn(_selectedAmmunitionPrefabs[ammoNum], point.position);
            }
        }

        SetNextRandomAmmunition(currentElement, nextElement);
    }

    private void SetNextRandomAmmunition(LevelElement currentElement, LevelElement nextElement)
    {
        var points = nextElement.PickableAmmunitionPoints;

        foreach (var point in points)
        {
            if (Random.Range(0, _maxGeneratePercent + 1) <= _generatePercent && _selectedAmmunitionPrefabs.Count > 0)
            {
                int ammoNum = Random.Range(0, _selectedAmmunitionPrefabs.Count);
                PickableAmmunitionPool.Instance.Spawn(_selectedAmmunitionPrefabs[ammoNum], point.position);
            }
        }
    }
}
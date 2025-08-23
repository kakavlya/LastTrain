using System;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class ProgressHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sumLevelsText;

    private int _sumLevels;
    public static ProgressHandler Instance { get; private set; }

    public int Level => _sumLevels;

    public event Action LevelChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RefreshSumLevels();
    }

    public void RefreshSumLevels()
    {
        _sumLevels = 0;
        var data = SaveManager.Instance.Data;
        List<WeaponProgress> weaponProgress = data.WeaponsProgress;
        TrainProgress trainProgress = data.TrainProgress;

        foreach (WeaponProgress progress in weaponProgress)
        {
            _sumLevels += progress.GetSumLevels();

            if (progress.IsAvailable)
            {
                _sumLevels++;
            }
        }

        _sumLevels += trainProgress.GetSumLevels();
        LevelChanged?.Invoke();
        _sumLevelsText.text = $"Level: {_sumLevels.ToString()}";
    }
}

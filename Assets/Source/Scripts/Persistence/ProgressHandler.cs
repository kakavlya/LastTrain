using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

namespace LastTrain.Persistence
{
    public class ProgressHandler : MonoBehaviour
    {
        public static ProgressHandler Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI _sumLevelsText;

        private int _sumLevels;

        public event Action LevelChanged;

        public int Level => _sumLevels;

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

        private void OnEnable()
        {
            LevelChanged += UpdateLevelText;
        }

        private void OnDisable()
        {
            LevelChanged -= UpdateLevelText;
        }

        private void Start()
        {
            // Run save migration once, before anything reads from the save.
            if (SavesYG.MigrateIfNeeded())
                YG2.SaveProgress();

            RefreshSumLevels();
        }

        public void RefreshSumLevels()
        {
            _sumLevels = 0;

            List<WeaponProgress> weaponProgress = YG2.saves.WeaponsProgress;
            TrainProgress trainProgress = YG2.saves.TrainProgress;

            foreach (WeaponProgress progress in weaponProgress)
            {
                _sumLevels += progress.GetSumLevels();

                if (progress.IsAvailable)
                {
                    _sumLevels++;
                }
            }

            _sumLevels += trainProgress.GetSumLevels();

            // Turret levels are intentionally excluded here until Phase 3,
            // when TurretProgress entries are authored and can be summed.

            LevelChanged?.Invoke();
        }

        private void UpdateLevelText()
        {
            if (_sumLevelsText != null)
                _sumLevelsText.text = _sumLevels.ToString();
        }
    }
}

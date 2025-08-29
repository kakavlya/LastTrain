using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using LastTrain.Coins;
using LastTrain.Data;
using LastTrain.Level;
using LastTrain.Persistence;

namespace LastTrain.Training
{
    public class TrainingHandler : MonoBehaviour
    {
        public static TrainingHandler Instance;

        [SerializeField] private string _menuSceneName;
        [SerializeField] private string _gameplaySceneName;
        [SerializeField] private LevelSetting _trainingSetting;
        [SerializeField] private List<WeaponUpgradeConfig> _trainingsWeapons;
        [SerializeField] private SharedData _sharedData;

        private bool _isDoneGameplayTraining;
        private bool _isDoneMenuTraining;

        public bool IsDoneGameplayTraining => _isDoneGameplayTraining;
        public bool IsDoneMenuTraining => _isDoneMenuTraining;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

            }
            else
            {
                Destroy(gameObject);
            }

            GetTrainingProgress();
            TryPlayTrainingGameplay();
        }

        public void TryEndGameplayTrainingAndLoadMenu()
        {
            if (_isDoneGameplayTraining == false)
            {
                Time.timeScale = 1f;
                YG2.saves.IsDoneGameplayTraining = true;
                GetTrainingProgress();
                CoinsHandler.Instance.SetTrainingStatus(false);
                SceneManager.LoadScene(_menuSceneName);
            }
        }

        private void GetTrainingProgress()
        {
            _isDoneGameplayTraining = YG2.saves.IsDoneGameplayTraining;
            _isDoneMenuTraining = YG2.saves.IsDoneMenuTraining;
        }

        private void TryPlayTrainingGameplay()
        {
            if (SceneManager.GetActiveScene().name == _menuSceneName && !_isDoneGameplayTraining && !_isDoneMenuTraining)
            {
                _sharedData.LevelSetting = _trainingSetting;
                _sharedData.WeaponConfigs = _trainingsWeapons;
                CoinsHandler.Instance.SetTrainingStatus(true);
                SceneManager.LoadScene(_gameplaySceneName);
            }
        }
    }
}

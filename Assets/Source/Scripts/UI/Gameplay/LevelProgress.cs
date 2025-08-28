using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using LastTrain.Coins;
using LastTrain.Data;
using LastTrain.Level;

namespace LastTrain.UI.Gameplay
{
    public class LevelProgress : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countdownText;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private int _startDelaySeconds;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private SharedData _sharedData;

        private int _levelDurationSeconds;
        private int _progressValue = 1;
        private LevelSetting _nextLevel;

        public event Action CountdownFinished;
        public event Action LevelComplited;

        public void Init()
        {
            if (_sharedData.LevelSetting.LevelDurationSec > 0)
                _levelDurationSeconds = _sharedData.LevelSetting.LevelDurationSec;
        }

        public void StartCountdown()
        {
            StartCoroutine(CountdownBeforePlaying());
            CountdownFinished?.Invoke();
        }

        private IEnumerator CountdownBeforePlaying()
        {
            _countdownText.enabled = true;
            int seconds = _startDelaySeconds;

            while (seconds > 0)
            {
                _countdownText.text = seconds.ToString();
                seconds -= _progressValue;
                yield return new WaitForSeconds(_progressValue);
            }

            _countdownText.enabled = false;
            StartCoroutine(CountdownLevelProgress());
        }

        private IEnumerator CountdownLevelProgress()
        {
            int progressSeconds = 0;

            while (progressSeconds <= _levelDurationSeconds)
            {
                _progressSlider.value = (float)progressSeconds / _levelDurationSeconds * _progressSlider.maxValue;
                progressSeconds += _progressValue;
                yield return new WaitForSeconds(_progressValue);
            }

            LevelComplited?.Invoke();
            UnlockNextLevel();
            CoinsHandler.Instance.AddCoins(_sharedData.LevelSetting.LevelReward);
        }

        private void UnlockNextLevel()
        {
            var currentLevel = _sharedData.LevelSetting;
            var levelsArray = _sharedData.AllLevels;

            for (int i = 0; i < levelsArray.Length; i++)
            {
                if (levelsArray[i] == currentLevel && i + 1 < levelsArray.Length)
                {
                    var nextLevel = levelsArray[i + 1];
                    nextLevel.IsAvailable = true;
                    var savedLevel = YG2.saves.LevelsAvailability.Find(level => level.LevelNumber == nextLevel.LevelNumber);

                    if (savedLevel != null)
                    {
                        savedLevel.IsAvailable = true;
                        _nextLevel = nextLevel;
                        _nextLevelButton.onClick.AddListener(StartNextLevel);
                    }

                    YG2.SaveProgress();
                    return;
                }
            }
        }

        private void StartNextLevel()
        {
            _sharedData.LevelSetting = _nextLevel;
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }
    }
}

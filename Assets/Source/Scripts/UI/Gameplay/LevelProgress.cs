using LastTrain.Coins;
using LastTrain.Level;
using LastTrain.Data;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace LastTrain.UI.Gameplay
{
    public class LevelProgress : MonoBehaviour
    {
        [SerializeField] private GameObject _startTimer;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private int _startDelaySeconds;
        [SerializeField] private Button _nextLevelButton;

        private int _levelDurationSeconds;
        private int _progressValue = 1;
        private LevelSetting _nextLevel;

        public event Action LevelCompleted;

        public void Init()
        {
            if (TransferData.Instance.LevelSetting.LevelDurationSec > 0)
                _levelDurationSeconds = TransferData.Instance.LevelSetting.LevelDurationSec;

            _startTimer.SetActive(false);
        }

        public void StartCountdown()
        {
            StartCoroutine(CountdownBeforePlaying());
        }

        private IEnumerator CountdownBeforePlaying()
        {
            _startTimer.SetActive(true);
            TextMeshProUGUI timerText = _startTimer.GetComponentInChildren<TextMeshProUGUI>();
            int seconds = _startDelaySeconds;

            while (seconds > 0)
            {
                timerText.text = seconds.ToString();
                seconds -= _progressValue;
                yield return new WaitForSeconds(_progressValue);
            }

            _startTimer.SetActive(false);
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

            LevelCompleted?.Invoke();
            UnlockNextLevel();
            CoinsHandler.Instance.AddCoins(TransferData.Instance.LevelSetting.LevelReward);
        }

        private void UnlockNextLevel()
        {
            var currentLevel = TransferData.Instance.LevelSetting;
            var levelsArray = TransferData.Instance.AllLevels;

            for (int i = 0; i < levelsArray.Length; i++)
            {
                if (levelsArray[i] == currentLevel && i + 1 < levelsArray.Length)
                {
                    var nextLevel = levelsArray[i + 1];
                    var savedLevel = YG2.saves.LevelsAvailability.Find(level => level.LevelNumber == nextLevel.LevelNumber);

                    if (savedLevel != null)
                    {
                        savedLevel.SetAvailable(true);
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
            TransferData.Instance.SetLevelSetting(_nextLevel);
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }
    }
}

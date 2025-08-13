using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private int _startDelaySeconds;
    [SerializeField] private SharedData _sharedData;

    private int _levelDurationSeconds;
    private int _progressValue = 1;

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
        OpenNextLevel();
        CoinsHandler.Instance.AddCoins(_sharedData.LevelSetting.LevelReward);
    }

    private void OpenNextLevel()
    {
        var currentLevel = _sharedData.LevelSetting;
        var levelsArray = _sharedData.AllLevels;

        for (int i = 0; i < levelsArray.Length; i++)
        {
            if (levelsArray[i] == currentLevel && i + 1 < levelsArray.Length)
            {
                var nextLevel = levelsArray[i + 1];
                nextLevel.IsAvailable = true;

                var savedLevel = SaveManager.Instance.Data.LevelsAvailability.Find(level => level.Name == nextLevel.LevelName);
                
                if (savedLevel != null)
                {
                    savedLevel.Available = true;
                }

                SaveManager.Instance.Save();
                return;
            }
        }
    }
}

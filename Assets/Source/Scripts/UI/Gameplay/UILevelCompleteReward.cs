using TMPro;
using UnityEngine;
using LastTrain.Coins;
using LastTrain.Data;

namespace LastTrain.UI.Gameplay
{
    public class UILevelCompleteReward : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textForKillsCount;
        [SerializeField] private TextMeshProUGUI _textForLevelCount;
        [SerializeField] private SharedData _sharedData;
        [SerializeField] private LevelProgress _levelProgress;

        private int _countCoinsForKills;
        private int _countCoinsForCompleted;
        private string _textForKillsCompleted = "Kill bonus: +";
        private string _textForLevelCompleted = "Level bonus: +";

        private void Start()
        {
            _levelProgress.LevelComplited += ShowLevelResults;
            CoinsHandler.Instance.Added += AddCoinsForKills;
            _countCoinsForCompleted = _sharedData.LevelSetting.LevelReward;
        }

        private void OnDisable()
        {
            CoinsHandler.Instance.Added -= AddCoinsForKills;
            _levelProgress.LevelComplited -= ShowLevelResults;
        }

        private void AddCoinsForKills(int reward)
        {
            _countCoinsForKills += reward;
        }

        private void ShowLevelResults()
        {
            _textForKillsCount.text = _textForKillsCompleted + _countCoinsForKills.ToString();
            _textForLevelCount.text = _textForLevelCompleted + _countCoinsForCompleted.ToString();
        }
    }
}
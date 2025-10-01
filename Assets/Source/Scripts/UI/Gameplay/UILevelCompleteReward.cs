using LastTrain.Coins;
using LastTrain.Data;
using TMPro;
using UnityEngine;

namespace LastTrain.UI.Gameplay
{
    public class UILevelCompleteReward : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textForKillsCount;
        [SerializeField] private TextMeshProUGUI _textForLevelCount;
        [SerializeField] private LevelProgress _levelProgress;

        private int _countCoinsForKills;
        private int _countCoinsForCompleted;

        private void Start()
        {
            _levelProgress.LevelCompleted += ShowLevelResults;
            CoinsHandler.Instance.Added += AddCoinsForKills;
            _countCoinsForCompleted = TransferData.Instance.LevelSetting.LevelReward;
        }

        private void OnDisable()
        {
            CoinsHandler.Instance.Added -= AddCoinsForKills;
            _levelProgress.LevelCompleted -= ShowLevelResults;
        }

        private void AddCoinsForKills(int reward)
        {
            _countCoinsForKills += reward;
        }

        private void ShowLevelResults()
        {
            _textForKillsCount.text = _countCoinsForKills.ToString();
            _textForLevelCount.text = _countCoinsForCompleted.ToString();
        }
    }
}
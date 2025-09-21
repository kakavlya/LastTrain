using UnityEngine;
using UnityEngine.UI;
using YG;
using LastTrain.Coins;

namespace LastTrain.Advertisement
{
    public class RewardAdv : MonoBehaviour
    {
        [SerializeField] private Button _rewardButton;
        [SerializeField] private int _rewardCount;
        [SerializeField] private GameObject _rewardScreen;
        [SerializeField] private Button _rewardScreenBackButton;

        private string _rewardID = "1";

        private void Awake()
        {
            _rewardButton.onClick.AddListener(OpenRewardAd);
            YG2.onCloseRewardedAdv += ShowRewardScreen;
            _rewardScreenBackButton.onClick.AddListener(() => _rewardScreen.SetActive(false));
        }

        private void OnDestroy()
        {
            _rewardButton?.onClick.RemoveListener(OpenRewardAd);
            YG2.onCloseRewardedAdv -= ShowRewardScreen;
            _rewardScreenBackButton.onClick.RemoveAllListeners();
        }

        private void OpenRewardAd()
        {
            YG2.RewardedAdvShow(_rewardID, () =>
            {
                if (_rewardID == "1")
                {
                    CoinsHandler.Instance.AddCoins(_rewardCount);
                    _rewardButton.interactable = false;
                }

            });
        }

        private void ShowRewardScreen()
        {
            _rewardScreen.SetActive(true);
        }
    }
}

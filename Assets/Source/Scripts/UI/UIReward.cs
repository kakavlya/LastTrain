using TMPro;
using UnityEngine;

public class UIReward : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textCount;

    private void OnEnable()
    {
        RewardHandler.Instance.RewardChanged += UpdateRewardUI;
    }

    private void OnDisable()
    {
        RewardHandler.Instance.RewardChanged -= UpdateRewardUI;
    }

    private void UpdateRewardUI(int newRewardCount)
    {
        _textCount.text = newRewardCount.ToString();
    }
}

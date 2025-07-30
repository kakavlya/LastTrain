using TMPro;
using UnityEngine;

public class UIReward : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textCount;

    private void Start()
    {
        CoinsHandler.Instance.coinsChanged += UpdateCoinsUI;
        UpdateCoinsUI(CoinsHandler.Instance.CoinsCount);
    }

    private void OnDisable()
    {
        CoinsHandler.Instance.coinsChanged -= UpdateCoinsUI;
    }

    private void UpdateCoinsUI(int newRewardCount)
    {
        _textCount.text = newRewardCount.ToString();
    }
}

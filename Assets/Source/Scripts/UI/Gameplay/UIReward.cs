using TMPro;
using UnityEngine;

public class UIReward : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textAllCount;

    private void Start()
    {
        CoinsHandler.Instance.CoinsChanged += UpdateCoinsUI;
        UpdateCoinsUI(CoinsHandler.Instance.CoinsCount);
    }

    private void OnDisable()
    {
        CoinsHandler.Instance.CoinsChanged -= UpdateCoinsUI;
    }

    private void UpdateCoinsUI(int newRewardCount)
    {
        _textAllCount.text = newRewardCount.ToString();
    }
}

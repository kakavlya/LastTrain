using UnityEngine;
using TMPro;
using YG;

public class DebugCoinsButton : MonoBehaviour
{
    [SerializeField] private int amount = 1000;        
    [SerializeField] private TMP_Text coinsText;

    public void AddCoins()
    {
        var coins = YG2.saves.Coins;
        coins += amount;
        YG2.SaveProgress();

        if (coinsText != null)
            coinsText.text = $"Coins: {coins}";
    }
}

using UnityEngine;
using TMPro;        

public class DebugCoinsButton : MonoBehaviour
{
    [SerializeField] private int amount = 1000;        
    [SerializeField] private TMP_Text coinsText;

    public void AddCoins()
    {
        var data = SaveManager.Instance.Data;
        data.Coins += amount;
        SaveManager.Instance.Save();

        if (coinsText != null)
            coinsText.text = $"Coins: {data.Coins}";
    }
}

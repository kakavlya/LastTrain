using System;
using UnityEngine;

public class CoinsHandler : MonoBehaviour
{
    private int _coinsCount;

    public static CoinsHandler Instance { get; private set; }

    public event Action<int> coinsChanged;

    public int CoinsCount
    {
        get => _coinsCount;
        private set
        {
            if (_coinsCount != value)
            {
                _coinsCount = value;
                coinsChanged?.Invoke(_coinsCount);
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CoinsCount = SaveManager.Instance.Data.Coins;
    }

    public void AddCoins(int addedCoins)
    {
        CoinsCount += addedCoins;
        SaveManager.Instance.Data.Coins += addedCoins;
    }
}

using System;
using UnityEngine;

public class RewardHandler : MonoBehaviour
{
    private int _rewardCount;

    public static RewardHandler Instance { get; private set; }

    public event Action<int> RewardChanged;

    public int RewardCount
    {
        get => _rewardCount;
        private set
        {
            if (_rewardCount != value)
            {
                _rewardCount = value;
                RewardChanged?.Invoke(_rewardCount);
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
    }

    public void AddReward(int addedReward)
    {
        RewardCount += addedReward;
    }
}

using System;
using UnityEngine;
using YG;

namespace LastTrain.Coins
{
    public class CoinsHandler : MonoBehaviour
    {
        public static CoinsHandler Instance { get; private set; }

        private int _coinsCount;
        private bool _isTraining;

        public event Action<int> CoinsChanged;
        public event Action<int> Added;

        public int CoinsCount
        {
            get => _coinsCount;
            private set
            {
                if (_coinsCount != value)
                {
                    _coinsCount = value;
                    CoinsChanged?.Invoke(_coinsCount);
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
            CoinsCount = YG2.saves.Coins;
        }

        public void AddCoins(int addedCoins)
        {
            if (_isTraining)
                return;

            CoinsCount += addedCoins;
            YG2.saves.Coins += addedCoins;
            Added?.Invoke(addedCoins);
            YG2.SaveProgress();
        }

        public void RemoveCoins(int removedCoins)
        {
            CoinsCount -= removedCoins;
            YG2.saves.Coins -= removedCoins;
            YG2.SaveProgress();
        }

        public void SetTrainingStatus(bool isActive)
        {
            _isTraining = isActive;
        }
    }
}

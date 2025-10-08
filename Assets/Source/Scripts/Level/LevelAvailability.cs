using UnityEngine;

namespace LastTrain.Level
{
    [System.Serializable]
    public class LevelAvailability
    {
        [SerializeField] private int _levelNumber;
        [SerializeField] private bool _isAvailable;

        public LevelAvailability(int number, bool isAvailable)
        {
            _levelNumber = number;
            _isAvailable = isAvailable;
        }

        public int LevelNumber => _levelNumber;

        public bool IsAvailable => _isAvailable;

        public void SetAvailable(bool isAvailable)
        {
            _isAvailable = isAvailable;
        }
    }
}

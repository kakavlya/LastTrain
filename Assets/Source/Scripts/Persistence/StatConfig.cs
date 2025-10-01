using Assets.SimpleLocalization.Scripts;
using UnityEngine;

namespace LastTrain.Persistence
{
    [System.Serializable]
    public class StatConfig
    {
        [SerializeField] private string _localizationKey;
        [SerializeField] private StatType _statType;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private int _maxLevel;
        [SerializeField] private int[] _costs;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;
        [SerializeField] private bool _isShowFractionalValue;

        public StatType StatType => _statType;

        public int MaxLevel => _maxLevel;

        public bool IsShowFractionalValue => _isShowFractionalValue;

        public string Name
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_localizationKey) && LocalizationManager.HasKey(_localizationKey))
                {
                    return LocalizationManager.Localize(_localizationKey);
                }

                return _statType.ToString();
            }
        }

        public float GetValue(int level)
        {
            if (level < 0)
                return _minValue;

            if (level > _maxLevel)
                level = _maxLevel;

            float t = level / (float)_maxLevel;
            return Mathf.Lerp(_minValue, _maxValue, _curve.Evaluate(t));
        }

        public int GetCost(int level)
        {
            if (_costs != null && level >= 0 && level < _costs.Length)
                return _costs[level];

            return 0;
        }
    }
}

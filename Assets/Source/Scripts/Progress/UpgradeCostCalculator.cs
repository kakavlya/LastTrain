using UnityEngine;

public static class UpgradeCostCalculator
{
    private const int BaseCost = 100;
    private const float Multiplier = 1.5f;

    public static int Calculate(int level)
    {
        if (level < 1) return 0;
        return Mathf.RoundToInt(BaseCost * Mathf.Pow(Multiplier, level - 1));
    }
}

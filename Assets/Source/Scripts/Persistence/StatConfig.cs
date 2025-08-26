using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
public class StatConfig
{
    public string LocalizationKey;
    public StatType StatType;
    public AnimationCurve Curve;
    public int MaxLevel;

    public int[] Costs;

    public float MinValue;
    public float MaxValue;
    public bool IsShowFractionalValue;

    public string Name
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(LocalizationKey) && LocalizationManager.HasKey(LocalizationKey))
            {
                return LocalizationManager.Localize(LocalizationKey);
            }

            return StatType.ToString();
        }
    }

    public float GetValue(int level)
    {
        if (level < 0) 
            return MinValue;

        if (level > MaxLevel)
            level = MaxLevel;

        float t = level / (float)MaxLevel;

        return Mathf.Lerp(MinValue, MaxValue, Curve.Evaluate(t));
    }

    public int GetCost(int level)
    {
        if (Costs != null && level >= 0 && level < Costs.Length)
            return Costs[level];
        return 0;
    }
}

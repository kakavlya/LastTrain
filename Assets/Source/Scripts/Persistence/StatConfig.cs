using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
public class StatConfig
{
    public StatType StatType;
    public AnimationCurve Curve;
    public int MaxLevel;

    public int[] Costs;

    public float MinValue;
    public float MaxValue;

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

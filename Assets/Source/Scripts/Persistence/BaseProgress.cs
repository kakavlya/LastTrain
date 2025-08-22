using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseProgress
{
    public abstract int GetLevel(StatType stat);

    public abstract int GetSumLevels();

    public abstract void Increment(StatType stat);
}

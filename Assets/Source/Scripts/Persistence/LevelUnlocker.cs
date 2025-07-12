using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlocker : MonoBehaviour
{
    public void UnlockLevel(int levelIndex)
    {
        var data = SaveManager.Instance.Data;
        if (!data.UnlockedLevels.Contains(levelIndex))
        {
            data.UnlockedLevels.Add(levelIndex);
            SaveManager.Instance.Save();
        }
    }
}

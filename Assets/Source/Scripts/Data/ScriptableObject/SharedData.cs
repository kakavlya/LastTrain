using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Shared Data")]
public class SharedData : ScriptableObject
{
    private LevelSetting[] _allLevels;

    public LevelSetting LevelSetting;
    public List<WeaponInfo> WeaponInfos;

    public LevelSetting[] AllLevels => _allLevels;

    public void SetAllLevels(LevelSetting[] levelSettings)
    {
        _allLevels = levelSettings;
    }
}

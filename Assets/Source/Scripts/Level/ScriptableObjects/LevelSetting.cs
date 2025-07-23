using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Source.Scripts.Enemies;
using Level;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Setting")]
public class LevelSetting : ScriptableObject
{
    public string LevelName;
    public bool IsAvailable;
    public LevelElement[] LevelElements;
    public int LevelDurationSec;

    [Range(0, 100)]
    public int AmmunitionGeneratePercent;
    public SpawnerConfig SpawnerConfig;
}

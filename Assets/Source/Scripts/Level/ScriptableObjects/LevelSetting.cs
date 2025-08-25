using System;
using Assets.Source.Scripts.Enemies;
using Level;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Setting")]
public class LevelSetting : ScriptableObject
{
    public int LevelNumber;
    public bool IsAvailable;
    public LevelElement[] LevelElements;
    public int LevelDurationSec;
    public int LevelReward;

    [Range(0, 100)]
    public int AmmunitionGeneratePercent;
    public SpawnerConfig SpawnerConfig;
}

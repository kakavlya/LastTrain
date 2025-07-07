using System.Collections;
using System.Collections.Generic;
using Assets.Source.Scripts.Enemies;
using Level;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Setting")]
public class LevelSetting : ScriptableObject
{
    public LevelElement[] LevelElements;
    public float LevelDurationSec;
    public SpawnerConfig SpawnerConfig;
}

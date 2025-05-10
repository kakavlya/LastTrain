using Assets.Source.Scripts.Enemies;
using System.Linq;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;

    [SerializeField] private EnemyBehaviorSettings[] _allBehaviors;
    [SerializeField] private GameObject[] _allPrefabs;

    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform _playerTarget;

    private void Awake()
    {
        var entries = _allPrefabs
            .Select((prefab, i) => new EnemySpawnEntry
            {
                prefab = prefab,
                spawnInterval = 3f + i,          // можно свой расчёт
                randRangeXZ = new Vector2(2, 2),
                behaviorSettings = _allBehaviors[i]
            })
            .ToArray();

        _spawner.Init(entries, _spawnPoints, _playerTarget);
    }
}

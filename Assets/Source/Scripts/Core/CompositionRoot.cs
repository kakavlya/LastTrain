using Assets.Source.Scripts.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform _playerTarget;

    private void Awake()
    {
        _spawner.Init(
            _enemyPrefab,
            _spawnPoints,
            _playerTarget,
            spawnInterval: 5f,
            randRange: new Vector2(-2f, 2f)
        );
    }
}

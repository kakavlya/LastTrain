using System.Collections;
using UnityEngine;

namespace Assets.Source.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {

        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _spawnInterval = 3f;
        [SerializeField] private Transform _playerTarget;

        private float timer;

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= _spawnInterval)
            {
                SpawnEnemy();
                timer = 0f;
            }
        }

        private void SpawnEnemy()
        {
            var spawn = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            var enemy = Instantiate(_enemyPrefab, spawn.position, spawn.rotation);
            enemy.GetComponent<EnemyController>().Init(_playerTarget);
        }
    }
}
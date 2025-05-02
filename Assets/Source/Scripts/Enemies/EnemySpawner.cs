using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Source.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {

        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _spawnInterval = 3f;
        [SerializeField] private Transform _playerTarget;
        [SerializeField] private Vector2 _randRange = new Vector2(-2, 2);

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
            Vector3 randomizedPosition = RandomizePosition(spawn.position);
            var enemy = Instantiate(_enemyPrefab, randomizedPosition, spawn.rotation);
            enemy.GetComponent<EnemyController>().Init(_playerTarget);
        }

        private Vector3 RandomizePosition(Vector3 position)
        {
            float randomX = position.x + Random.Range(_randRange.x, _randRange.x);
            float randomY = position.y + Random.Range(_randRange.x, _randRange.x);

            return new Vector3(randomX, position.y, randomY);
        }
    }
}
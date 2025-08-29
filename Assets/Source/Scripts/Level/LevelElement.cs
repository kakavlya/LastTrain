using UnityEngine;

namespace LastTrain.Level
{
    public class LevelElement : MonoBehaviour
    {
        [SerializeField] private Transform[] _enemySpawnPoints;
        [SerializeField] private Transform[] _pickableAmmunitionSpawnPoints;

        public Transform[] EnemySpawnPoints => _enemySpawnPoints;
        public Transform[] PickableAmmunitionPoints => _pickableAmmunitionSpawnPoints;
    }
}

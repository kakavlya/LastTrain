using System.Collections.Generic;
using UnityEngine;
using LastTrain.Ammunition;

namespace Level
{
    public class LevelElement : MonoBehaviour
    {
        [SerializeField] private Transform[] _enemySpawnPoints;
        [SerializeField] private Transform[] _pickableAmmunitionSpawnPoints;

        public Transform[] EnemySpawnPoints => _enemySpawnPoints;
        public Transform[] PickableAmmunitionPoints => _pickableAmmunitionSpawnPoints;
    }
}

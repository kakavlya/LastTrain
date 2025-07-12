using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class LevelElement : MonoBehaviour
    {
        [SerializeField] private Transform[] _enemySpawnPoints;
        [SerializeField] private Transform[] _pickableAmmunitionSpawnPoints;

        public Transform[] EnemySpawnPoints => _enemySpawnPoints;
        public Transform[] PickableAmmunitionPoints => _pickableAmmunitionSpawnPoints;

        public void RandomSetPickableAmmunitions(PickableAmmunition[] pickableAmmunitions, float succesfullPersent)
        {
            foreach (Transform ammoPoint in _pickableAmmunitionSpawnPoints)
            {
                float persent = Random.Range(0, 1);

                if (succesfullPersent > persent)
                {
                    PickableAmmunitionPool.Instance.Spawn(
                        pickableAmmunitions[Random.Range(0, pickableAmmunitions.Length - 1)],
                        ammoPoint.position);
                }
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class LevelElement : MonoBehaviour
    {
        [SerializeField] private List<Transform> _enemySpawnPoints;
        [SerializeField] private List<Transform> _pickableAmmunitionSpawnPoints;

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

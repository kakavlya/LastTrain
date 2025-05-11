using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class LevelElementsCreator : MonoBehaviour
    {
        [SerializeField] private List<LevelElement> _levelElementsPrefabs;
        [SerializeField] private Transform _parentTransform;

        public LevelElement CreateElement(Vector3 position)
        {
            LevelElement element = Instantiate(_levelElementsPrefabs[GetRandomElementNumber()], _parentTransform);
            element.transform.position = position;
            return element;
        }

        private int GetRandomElementNumber()
        {
            int randomNumber = Random.Range(0, _levelElementsPrefabs.Count);
            return randomNumber;
        }
    }
}

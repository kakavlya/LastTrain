using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Level
{
    public class LevelElementsGenerator : MonoBehaviour
    {
        [SerializeField] private List<LevelElement> _levelElementsPrefabs;

        public LevelElement GenerateElement(Vector3 position)
        {
            LevelElement element = Instantiate(_levelElementsPrefabs[GetRandomElementNumber()]);
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

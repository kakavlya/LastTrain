using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class LevelElementsCreator : MonoBehaviour
    {
        [SerializeField] private List<LevelElement> _levelElementsPrefabs;
        [SerializeField] private Transform _parentTransformForElements;

        private float _pickableAmmoPersent;
        private PickableAmmunition[] _pickableAmmunitionsPrefabs;

        public void SetPickableAmmoParameters(PickableAmmunition[] pickableAmmunitions,float pickableAmmoPersent)
        {
            _pickableAmmunitionsPrefabs = pickableAmmunitions;
            _pickableAmmoPersent = pickableAmmoPersent;
        }

        public LevelElement CreateElement(Vector3 position)
        {
            LevelElement element = Instantiate(_levelElementsPrefabs[GetRandomElementNumber()], _parentTransformForElements);
            element.RandomSetPickableAmmunitions(_pickableAmmunitionsPrefabs, _pickableAmmoPersent);
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

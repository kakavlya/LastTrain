using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class LevelElementsCreator : MonoBehaviour
    {
        [SerializeField] private Transform _parentTransformForElements;
        [SerializeField] private SharedData _sharedData;

        private LevelElement[] _levelElementsPrefabs;
        private float _pickableAmmoPersent;
        private PickableAmmunition[] _pickableAmmunitionsPrefabs;

        public void Init()
        {
            _levelElementsPrefabs = _sharedData.LevelSetting.LevelElements;
        }

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
            int randomNumber = Random.Range(0, _levelElementsPrefabs.Length);
            return randomNumber;
        }
    }
}

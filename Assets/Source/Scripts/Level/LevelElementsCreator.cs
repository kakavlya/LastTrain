using LastTrain.Data;
using UnityEngine;

namespace LastTrain.Level
{
    public class LevelElementsCreator : MonoBehaviour
    {
        [SerializeField] private Transform _parentTransformForElements;

        private LevelElement[] _levelElementsPrefabs;

        public void Init()
        {
            _levelElementsPrefabs = TransferData.Instance.LevelSetting.LevelElements;
        }

        public LevelElement CreateElement(Vector3 position)
        {
            LevelElement element = Instantiate(_levelElementsPrefabs[GetRandomElementNumber()], _parentTransformForElements);
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

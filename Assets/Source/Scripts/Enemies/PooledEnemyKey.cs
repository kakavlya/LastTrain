using UnityEngine;

namespace LastTrain.Enemies
{
    public class PooledEnemyKey : MonoBehaviour
    {
        public GameObject PrefabKey { get; private set; }

        public void SetKey(GameObject prefabKey)
        {
            PrefabKey = prefabKey;
        }
    }
}

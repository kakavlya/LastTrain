using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledEnemyKey : MonoBehaviour
{
    public GameObject PrefabKey { get; private set; }

    public void SetKey(GameObject prefabKey)
    {
        PrefabKey = prefabKey;
    }
}

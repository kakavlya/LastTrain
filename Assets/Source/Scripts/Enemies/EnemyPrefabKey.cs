using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefabKey : MonoBehaviour
{
    private EnemyPrefabKey _enemyPrefabKey;

    public void SetPrefabKey(EnemyPrefabKey enemyPoolKey) => _enemyPrefabKey = enemyPoolKey;
}

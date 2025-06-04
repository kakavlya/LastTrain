using Assets.Source.Scripts.Enemies;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;

    private void Awake()
    {
        _spawner.Init();
    }
}
